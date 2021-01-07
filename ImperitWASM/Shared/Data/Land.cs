using System.Collections.Immutable;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public record Land : Region
	{
		public int Earnings { get; private set; }
		public bool IsStart { get; private set; }
		public bool IsFinal { get; private set; }
		public bool HasPort { get; private set; }
		public Land(string name, Shape shape, Soldiers soldiers, Settings settings, int earnings, bool isStart, bool isFinal, bool hasPort)
			: base(name, shape, soldiers, settings) => (Earnings, IsStart, IsFinal, HasPort) = (earnings, isStart, isFinal, hasPort);

		public override bool Inhabitable => IsStart;
		public override int Score => IsFinal ? 1 : 0;
		public override int Income => Earnings;

		public override bool Sailable => HasPort;
		public override bool Port => HasPort;
		public override bool Walkable => true;
		public override bool Dry => true;

		public override int Price => Settings.LandPrice(Soldiers, Earnings);
		public override Color Fill => Settings.LandColor;

		public override Ratio Instability(Soldiers present) => Settings.Instability(present, Soldiers);
		public override ImmutableArray<string> Text(Soldiers present) => ImmutableArray.Create(Name + (IsFinal ? "\u2605" : "") + (HasPort ? "\u2693" : ""), present.ToString(), Earnings + "\uD83D\uDCB0");

		public virtual bool Equals(Land? region) => region is not null && Id == region.Id;
		public override int GetHashCode() => Id.GetHashCode();

#pragma warning disable CS8618
		private Land() { }
#pragma warning restore CS8618
	}
}