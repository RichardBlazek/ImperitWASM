using System.Collections.Immutable;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public record Sea : Region
	{
		public Sea(string name, Shape shape, Soldiers soldiers, Settings settings) : base(name, shape, soldiers, settings) { }

		public override Color Fill => Settings.SeaColor;
		public override ImmutableArray<string> Text(Soldiers present) => ImmutableArray.Create(Name, present.ToString());

		public override bool Sailable => true;

		public virtual bool Equals(Sea? region) => region is not null && Id == region.Id;
		public override int GetHashCode() => Id.GetHashCode();
	}
}