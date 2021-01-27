using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public abstract record Region
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public int ShapeId { get; private set; }
		public Shape Shape { get; private set; }
		public int SoldiersId { get; private set; }
		public Soldiers Soldiers { get; private set; }
		public ICollection<RegionSoldierType>? RegionSoldierTypes { get; private set; }
		public byte Color_A { get; private set; }
		public byte Color_B { get; private set; }
		public byte Color_G { get; private set; }
		public byte Color_R { get; private set; }
		public double StrokeWidth { get; private set; }

		public int AttackPower => Soldiers.AttackPower;
		public int DefensePower => Soldiers.DefensePower;
		public int Power => Soldiers.Power;
		public ImmutableArray<Point> Border => Shape.Border;
		public Point Center => Shape.Center;
		public Color Color => new Color(Color_R, Color_G, Color_B, Color_A);
		public Color Stroke => Color.WithAlpha(255).Darken(128);

		public bool IsRecruitable(SoldierType type) => RegionSoldierTypes!.Any(t => t.SoldierType == type);
		public bool IsShaky(Soldiers present) => Instability(present).RandomBool;

		public virtual bool Inhabitable => false;
		public virtual bool Sailable => false;
		public virtual bool Mainland => false;
		public virtual bool Dry => false;
		public virtual bool Port => false;

		public virtual int Price => int.MaxValue;
		public virtual int Score => 0;
		public virtual int Income => 0;

		public virtual Ratio Instability(Soldiers present) => Ratio.Zero;
		public virtual ImmutableArray<string> Text(Soldiers present) => ImmutableArray<string>.Empty;

		public virtual bool Equals(Region? region) => region is not null && Id == region.Id;
		public override int GetHashCode() => Id.GetHashCode();

#pragma warning disable CS8618
		protected Region() { }
#pragma warning restore CS8618
	}
}
