﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ImperitWASM.Shared.Motion;
using ImperitWASM.Shared.Motion.Commands;
using static System.Math;

namespace ImperitWASM.Shared.State
{
	public class Robot : Player
	{
		private readonly Settings settings;
		public Robot(Color color, int money, bool alive, ImmutableList<IPlayerAction> actions, Settings settings)
			: base(color, new Description(), money, alive, actions) => this.settings = settings;
		public override Player ChangeMoney(int amount) => new Robot(Color, Money + amount, Alive, Actions, settings);
		public override Player Die() => new Robot(Color, 0, false, ImmutableList<IPlayerAction>.Empty, settings);
		protected override Player WithActions(ImmutableList<IPlayerAction> new_actions) => new Robot(Color, Money, Alive, new_actions, settings);

		private static Soldiers NextSoldiers(PlayersAndProvinces pap, Province p) => p.ActOnYourself(pap).Soldiers;
		private static int NextDefensePower(PlayersAndProvinces pap, Province p) => NextSoldiers(pap, p).DefensePower;
		private static int NextAttackPower(PlayersAndProvinces pap, Province p) => NextSoldiers(pap, p).AttackPower;
		private bool IsEnemy(Province p) => !p.IsAllyOf(this) && p.Occupied;
		private IEnumerable<Province> EnemyNeighbors(PlayersAndProvinces pap, Province prov) => pap.NeighborsOf(prov).Where(IsEnemy);
		private int EnemiesPower(PlayersAndProvinces pap, Province prov) => EnemyNeighbors(pap, prov).GroupBy(p => p.Player).Max(p => p.Sum(neighbor => NextAttackPower(pap, neighbor)));
		private int Bilance(PlayersAndProvinces pap, Province prov) => NextDefensePower(pap, prov) - EnemiesPower(pap, prov);

		private void Recruit(ref PlayersAndProvinces pap, ref int spent, Province province, Soldiers soldiers)
		{
			(pap, _) = pap.Do(new Recruit(this, province, soldiers));
			spent += soldiers.Price;
		}

		private SoldierType? BestDefender(Province p, int money)
		{
			return settings.SoldierTypes.Where(type => type.IsRecruitable(p)).MinBy(type => -money / type.Price * type.DefensePower);
		}

		private static int DivUp(int a, int b) => (a / b) + (a % b > 0 ? 1 : 0);

		private void DefensiveRecruits(ref PlayersAndProvinces pap, ref int spent, Province[] my)
		{
			foreach (var place in my)
			{
				if (EnemiesPower(pap, place) - NextDefensePower(pap, place) is int shortage && shortage > 0 && BestDefender(place, Money - spent) is SoldierType type && shortage <= (Money - spent) / type.Price * type.DefensePower)
				{
					Recruit(ref pap, ref spent, place, new Soldiers(type, DivUp(shortage, type.DefensePower)));
				}
			}
		}

		private void StabilisatingRecruits(ref PlayersAndProvinces pap, ref int spent, Province[] my)
		{
			foreach (var place in my)
			{
				if (Money <= spent)
				{
					break;
				}
				if (place.DefaultSoldiers.DefensePower - NextDefensePower(pap, place) is int shortage && shortage > 0 && BestDefender(place, Money - spent) is SoldierType type)
				{
					Recruit(ref pap, ref spent, place, new Soldiers(type, Min((Money - spent) / type.Price, DivUp(shortage, type.DefensePower))));
				}
			}
		}

		private void Recruits(ref PlayersAndProvinces pap, Province[] my)
		{
			int spent = 0;
			DefensiveRecruits(ref pap, ref spent, my);
			StabilisatingRecruits(ref pap, ref spent, my);
			if (my.Any() && BestDefender(my[0], Money - spent) is SoldierType type && Money - spent > 0)
			{
				Recruit(ref pap, ref spent, my[0], new Soldiers(type, (Money - spent) / type.Price));
			}
		}

		private static bool CanConquer(Soldiers from, Soldiers to) => from.AttackPower > to.DefensePower;
		private static bool CanKeepConqueredProvince(Soldiers from, Soldiers to, int enemies_to) => from.AttackPower >= to.DefensePower + enemies_to;
		private static bool CanAttackSuccesfully(Soldiers attackers, Soldiers defenders, int enemies_to) => CanConquer(attackers, defenders) && CanKeepConqueredProvince(attackers, defenders, enemies_to);
		private bool ShouldAttack(Soldiers attackers, Province to, int enemies) => !to.IsAllyOf(this) && CanAttackSuccesfully(attackers, to.Soldiers, enemies);

		private void Attack(ref PlayersAndProvinces pap, Province from, Province to, Soldiers soldiers)
		{
			(pap, _) = pap.Do(new Move(this, from, to, soldiers));
		}

		private static Soldiers Units(int num) => new Soldiers(new Pedestrian(new Description("", "", ""), 1, 1, 1, 1), num);

		private IEnumerable<(Province, Soldiers)> GetAttacks(PlayersAndProvinces pap, Province from)
		{
			int enemies = EnemiesPower(pap, from);
			return pap.NeighborsOf(from).Select(to => (to, from.Soldiers.MaxAttackers(pap, from, to).AttackedBy(Units(enemies - (IsEnemy(to) ? to.Soldiers.AttackPower : 0))))).Where(to => ShouldAttack(to.Item2, to.to, EnemiesPower(pap, to.to)));
		}

		private void Attacks(ref PlayersAndProvinces pap, Province[] my)
		{
			foreach (var from in my)
			{
				foreach (var (to, soldiers) in GetAttacks(pap, from))
				{
					Attack(ref pap, from, to, soldiers);
				}
			}
		}

		private IEnumerable<Province> NeighborAllies(PlayersAndProvinces pap, Province pr) => pap.NeighborsOf(pr).Where(n => n.IsAllyOf(this));
		private IEnumerable<Province> AttackPlaces(PlayersAndProvinces pap, Province[] my) => my.SelectMany(place => pap.NeighborsOf(place).Where(p => p.Occupied && !p.IsAllyOf(this)));

		private Soldiers AttackersFrom(PlayersAndProvinces pap, IEnumerable<Province> starts, Province to)
		{
			return starts.Aggregate(new Soldiers(), (s, from) => s.Add(from.Soldiers.MaxAttackers(pap, from, to).AttackedBy(Units(EnemiesPower(pap, from) - to.Soldiers.AttackPower))));
		}

		private void MultiAttacks(ref PlayersAndProvinces pap, Province[] my)
		{
			foreach (var to in AttackPlaces(pap, my).Distinct())
			{
				var starts = NeighborAllies(pap, to);
				int total = AttackersFrom(pap, starts, to).AttackPower;
				if (total > to.Soldiers.DefensePower + EnemiesPower(pap, to))
				{
					foreach (var from in starts)
					{
						Attack(ref pap, from, to, from.Soldiers.MaxAttackers(pap, from, to).AttackedBy(Units(EnemiesPower(pap, from) - to.Soldiers.AttackPower)));
					}
				}
			}
		}

		private void Transport(ref PlayersAndProvinces pap, Province from, Province to, Soldiers soldiers)
		{
			(pap, _) = pap.Do(new Move(this, from, to, soldiers));
		}

		private void SpreadSoldiers(ref PlayersAndProvinces pap, Province[] my)
		{
			foreach (var from in my)
			{
				int enemies_from = EnemiesPower(pap, from);
				int bilance_from = NextDefensePower(pap, from) - enemies_from;
				var p_p = pap;
				foreach (var (dest, bilance) in pap.NeighborsOf(from).Where(n => n.IsAllyOf(this)).Select(p => (p, Bilance(p_p, p))).OrderBy(n => n.Item2))
				{
					if (enemies_from <= 0 && EnemyNeighbors(pap, dest).Any())
					{
						Transport(ref pap, from, dest, from.Soldiers);
					}
					else if (bilance_from > 0 && bilance_from >= bilance && from.Soldiers.DefensePower >= from.DefaultSoldiers.DefensePower)
					{
						Transport(ref pap, from, dest, from.Soldiers.AttackedBy(from.DefaultSoldiers));
					}
				}
			}
		}
		public PlayersAndProvinces Think(PlayersAndProvinces pap)
		{
			var my = pap.Provinces.ControlledBy(this).ToArray();
			Recruits(ref pap, my);
			Attacks(ref pap, my);
			MultiAttacks(ref pap, my);
			SpreadSoldiers(ref pap, my);
			return pap;
		}
	}
}
