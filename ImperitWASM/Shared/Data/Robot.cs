using System.Collections.Generic;
using System.Linq;
using ImperitWASM.Shared.Commands;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public record Robot : Player
	{
		public Robot(string name, long gameId, int order, Color color, int money, bool alive, Settings settings, bool isActive) : base(name, gameId, order, color, money, alive, settings, isActive) { }

		public virtual bool Equals(Robot? obj) => obj is not null && obj.Name == Name;
		public override int GetHashCode() => base.GetHashCode();

		static int Max(params int[] values) => values.Max();
		static int Clamp(int value, int min, int max) => value < min ? min : value > max ? max : value;
		static int Updiv(int a, int b) => (a + b - 1) / b;

		int[] ComputeEnemies(Provinces provinces)
		{
			return provinces.Select(province => provinces.NeighborsOf(province).Where(n => n.IsEnemyOf(this)).Sum(n => n.AttackPower)).ToArray();
		}
		int[] ComputeAllies(Provinces provinces, int[] enemies, int[] defense)
		{
			return provinces.Select(province => provinces.NeighborsOf(province).Where(n => n.IsAllyOf(this)).Sum(n => Clamp(n.DefensePower - enemies[n.RegionId] + defense[n.RegionId], 0, n.DefensePower))).ToArray();
		}
		IEnumerable<int> EnemyNeighborIndices(Provinces provinces, int i)
		{
			return provinces.NeighborIndices(provinces[i]).Where(n => provinces[n].IsEnemyOf(this));
		}
		IEnumerable<int> AllyNeighborIndices(Provinces provinces, int i)
		{
			return provinces.NeighborIndices(provinces[i]).Where(n => provinces[n].IsAllyOf(this));
		}
		int[] Attackable(Provinces provinces, IEnumerable<int> owned)
		{
			return owned.SelectMany(i => EnemyNeighborIndices(provinces, i)).Distinct().ToArray();
		}

		SoldierType BestDefender(int i) => Settings.RecruitableIn(i).OrderBy(type => Money / type.Price * type.DefensePower).First();
		Ship? BestShip(int i) => Settings.RecruitableIn(i).OfType<Ship>().Where(type => type.Price <= Money).OrderByDescending(type => type.Capacity).FirstOrDefault();

		int EnemiesAfterAttack(Provinces provinces, int[] enemies, int[] defense, int start, int attacked)
		{
			return Max(0, enemies[start] - defense[start] + (provinces[attacked].IsEnemyOf(this) ? enemies[attacked] : 0));
		}
		Soldiers AttackingSoldiers(Provinces provinces, int[] enemies, int[] defense, int start, int attacked)
		{
			var movable = provinces[start].MaxMovable(provinces, provinces[attacked]);
			return movable.FightAgainst(EnemiesAfterAttack(provinces, enemies, defense, start, attacked), type => type.DefensePower);
		}

		static (Robot, Provinces) Thinking(Robot ich, IReadOnlyList<Player> players, Provinces provinces, Settings settings)
		{
			(Robot, Provinces) Do(Robot doer, ICommand command)
			{
				var (new_players, new_provinces) = command.Perform(doer, players, provinces, settings);
				return (new_players.First(p => p == doer) as Robot, provinces.With(new_provinces))!;
			}
			int[] enemies = ich.ComputeEnemies(provinces);
			int[] defense = provinces.Select(province => province.DefensePower).ToArray();
			int[] allies = ich.ComputeAllies(provinces, enemies, defense); // Neighbor provinces which can send reinforcements
			int[] owned = provinces.Indices(province => province.IsAllyOf(ich)).ToArray();

			// Defensive reinforcements from the safe provinces to the endangered
			for (int i = 0; i < owned.Length; ++i)
			{
				if (enemies[owned[i]] > defense[owned[i]] && allies[owned[i]] > 0)
				{
					foreach (int supporter in ich.AllyNeighborIndices(provinces, owned[i]))
					{
						if (enemies[supporter] < defense[supporter])
						{
							// First, I take all soldiers who can move, but supporter province should not be endangered,
							// therefore I subtract soldiers whose defense power equals to the power of potential enemies
							var moving = provinces[supporter].MaxMovable(provinces, provinces[i]);
							moving = moving.FightAgainst(enemies[supporter], type => type.DefensePower);

							(ich, provinces) = Do(ich, new Move(provinces[supporter], provinces[i], moving));
							defense[i] += moving.DefensePower;
							allies[i] -= moving.DefensePower;
						}
					}
				}
			}

			// Defensive recruitment, if attack can be stopped
			for (int i = 0; i < owned.Length && ich.Money > 0; ++i)
			{
				var type = ich.BestDefender(i);
				if (enemies[i] > defense[i] && ich.Money / type.Price * type.DefensePower >= enemies[i] - defense[i])
				{
					var recruited = new Soldiers(type, Updiv(enemies[i] - defense[i], type.DefensePower));
					(ich, provinces) = Do(ich, new Recruit(provinces[i], recruited));
					defense[i] += recruited.DefensePower;
				}
			}

			// Recruitments reducing the probability of a revolution
			for (int i = 0; i < owned.Length && ich.Money > 0; ++i)
			{
				var type = ich.BestDefender(i);
				if (provinces[i].DefaultDefensePower > defense[i] && ich.Money >= type.Price)
				{
					var recruited = new Soldiers(type, Clamp((provinces[i].DefaultDefensePower - defense[i]) / type.DefensePower, 0, ich.Money / type.Price));
					(ich, provinces) = Do(ich, new Recruit(provinces[i], recruited));
					defense[i] += recruited.DefensePower;
				}
			}

			// Recruitments of ships
			for (int i = 0; i < owned.Length && ich.Money > 0; ++i)
			{
				if (ich.BestShip(i) is Ship ship)
				{
					(ich, provinces) = Do(ich, new Recruit(provinces[i], new Soldiers(ship, 1)));
					defense[i] += ship.DefensePower;
				}
			}

			// Attacks
			foreach (int attacked in ich.Attackable(provinces, owned))
			{
				int[] starts = ich.AllyNeighborIndices(provinces, attacked).ToArray();
				var armies = starts.Select(i => ich.AttackingSoldiers(provinces, enemies, defense, i, attacked)).ToArray();
				if (armies.Sum(soldiers => soldiers.AttackPower) > defense[attacked])
				{
					if (provinces[attacked].IsEnemyOf(ich))
					{
						foreach (int neighbour in provinces.NeighborIndices(provinces[attacked]))
						{
							enemies[neighbour] -= provinces[attacked].AttackPower;
						}
					}
					for (int i = 0; i < starts.Length; ++i)
					{
						(ich, provinces) = Do(ich, new Move(provinces[starts[i]], provinces[attacked], armies[i]));
					}
				}
			}

			return (ich, provinces);
		}

		public (Robot, Provinces) Think(IReadOnlyList<Player> players, Provinces provinces, Settings settings)
		{
			return Thinking(this, players, provinces, settings);
		}
	}
}
