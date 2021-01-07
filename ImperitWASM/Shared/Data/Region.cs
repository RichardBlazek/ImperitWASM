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
		public virtual Shape Shape { get; private set; }
		public virtual Soldiers Soldiers { get; private set; }
		public virtual ICollection<ProvinceSoldierType>? ProvinceSoldierTypes { get; private set; }
		public virtual Settings Settings { get; private set; }
		public Region(string name, Shape shape, Soldiers soldiers, Settings settings)
		{
			Name = name;
			Shape = shape;
			Soldiers = soldiers;
			Settings = settings;
		}

		public int AttackPower => Soldiers.AttackPower;
		public int DefensePower => Soldiers.DefensePower;
		public int Power => Soldiers.Power;
		public ImmutableArray<Point> Border => Shape.Border;
		public Point Center => Shape.Center;
		public bool IsRecruitable(SoldierType type) => ProvinceSoldierTypes!.Any(t => t.SoldierType == type);
		public bool IsShaky(Soldiers present) => Instability(present).RandomBool;

		public virtual bool Inhabitable => false;
		public virtual bool Sailable => false;
		public virtual bool Walkable => false;
		public virtual bool Dry => false;
		public virtual bool Port => false;

		public virtual int Price => int.MaxValue;
		public virtual int Score => 0;
		public virtual int Income => 0;

		public virtual Color Fill => new Color();
		public virtual Color Stroke => new Color();
		public virtual float StrokeWidth => 0.0f;

		public virtual Ratio Instability(Soldiers present) => Ratio.Zero;
		public virtual ImmutableArray<string> Text(Soldiers present) => ImmutableArray<string>.Empty;

		public virtual bool Equals(Region? region) => region is not null && Id == region.Id;
		public override int GetHashCode() => Id.GetHashCode();
	}
}
