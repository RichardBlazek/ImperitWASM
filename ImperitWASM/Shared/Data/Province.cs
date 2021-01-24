using System.Collections.Immutable;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public sealed record Province
	{
		public int GameId { get; private set; }
		public int RegionId { get; private set; }
		public Region Region { get; private set; }
		public Player? Player { get; private set; }
		public Soldiers Soldiers { get; private set; }
		public Province(int gameId, Region region, Soldiers soldiers)
		{
			GameId = gameId;
			(Region, RegionId) = (region, region.Id);
			Soldiers = soldiers;
		}

		public int Score => Region.Score;
		public int Income => Region.Income;
		public int Price => Region.Price;
		public int DefaultDefensePower => Region.DefensePower;

		public Point Center => Region.Center;
		public ImmutableArray<Point> Border => Region.Border;
		public string Name => Region.Name;
		public ImmutableArray<string> Text => Region.Text(Soldiers);

		public Province RuledBy(Player player) => this with { Player = player };
		public Province Revolt() => this with { Player = null, Soldiers = DefaultSoldiers };
		public bool IsShaky(Player active) => !CanPersist || !Soldiers.Any || (IsAllyOf(active) && Region.IsShaky(Soldiers));
		public Province RevoltIfShaky(Player active) => IsShaky(active) ? Revolt() : this;

		public Ratio Instability => Region.Instability(Soldiers);
		public int AttackPower => Soldiers.AttackPower;
		public int DefensePower => Soldiers.DefensePower;
		public int Power => Soldiers.Power;
		public int SoldierPrice => Soldiers.Price;
		public Soldiers MaxMovable(Provinces provinces, Province to) => Soldiers.MaxMovable(provinces, this, to);
		public Soldiers DefaultSoldiers => Region.Soldiers;

		public Province Add(Soldiers another) => this with { Soldiers = Soldiers.Add(another) };
		public Province Subtract(Soldiers army) => this with { Soldiers = Soldiers.Subtract(army) };
		Province AttackedBy(Player p, Soldiers s) => this with { Player = s.AttackPower > Soldiers.DefensePower ? p : Player, Soldiers = Soldiers.AttackedBy(s) };
		public Province VisitedBy(Player p, Soldiers s) => IsAllyOf(p) ? Add(s) : AttackedBy(p, s);

		public bool Inhabited => Player is not null;
		public bool Inhabitable => Region.Inhabitable && !Inhabited;
		public bool IsAllyOf(Player? other) => Inhabited && other == Player;
		public bool IsAllyOf(Province province) => province.IsAllyOf(Player);
		public bool IsEnemyOf(Player other) => Inhabited && !IsAllyOf(other);
		public bool IsRecruitable(SoldierType type) => Region.IsRecruitable(type);

		public bool Has(Soldiers soldiers) => Soldiers.Contains(soldiers);
		public bool HasSoldiers => Soldiers.Any;
		public bool CanPersist => Soldiers.CanSurviveIn(this);
		public bool CanPersistWithout(Soldiers s) => Subtract(s).CanPersist;
		public bool CanAnyMove(Provinces provinces, Province to) => Soldiers.CanAnyMove(provinces, this, to);
		public bool CanMove(Provinces provinces, Province to, Soldiers soldiers) => soldiers.CanMove(provinces, this, to) && CanPersistWithout(soldiers);

		public bool Sailable => Region.Sailable;
		public bool Mainland => Region.Mainland;
		public bool Dry => Region.Dry;
		public bool Port => Region.Port;

		public Color Fill => Player.ColorOf(Player).Over(Region.Color);
		public Color Stroke => Region.Stroke;
		public double StrokeWidth => Region.StrokeWidth;

		public bool Equals(Province? other) => other is not null && other.RegionId == RegionId;
		public override int GetHashCode() => Region.GetHashCode();

#pragma warning disable CS8618
		private Province() { }
#pragma warning restore CS8618
	}
}