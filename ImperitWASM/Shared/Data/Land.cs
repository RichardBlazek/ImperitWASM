using System.Collections.Immutable;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public record Land : Region
	{
		public int Earnings { get; private set; }
		public bool IsFinal { get; private set; }
		public bool IsStart { get; private set; }
		public bool HasPort { get; private set; }
		public int DefaultInstabilityInt { get; private set; }

		public override bool Inhabitable => IsStart;
		public override int Score => IsFinal ? 1 : 0;
		public override int Income => Earnings;

		public override bool Sailable => HasPort;
		public override bool Port => HasPort;
		public override bool Mainland => true;
		public override bool Dry => true;

		public override int Price => Soldiers.Price + Soldiers.DefensePower + Earnings;
		public override Ratio Instability(Soldiers now) => new Ratio(DefaultInstabilityInt).Adjust(System.Math.Max(DefensePower - now.DefensePower, 0), DefensePower);
		public override ImmutableArray<string> Text(Soldiers present) => ImmutableArray.Create(Name + (IsFinal ? "\u2605" : "") + (HasPort ? "\u2693" : ""), present.ToString(), Earnings + "\uD83D\uDCB0");

		public virtual bool Equals(Land? region) => region is not null && Id == region.Id;
		public override int GetHashCode() => Id.GetHashCode();

#pragma warning disable CS8618
		private Land() { }
#pragma warning restore CS8618
	}
}