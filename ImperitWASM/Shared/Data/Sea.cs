using System.Collections.Immutable;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public record Sea : Region
	{
		public Sea(string name, Shape shape, Soldiers soldiers, Color color, double strokeWidth)
			: base(name, shape, soldiers, color, strokeWidth) { }

		public override ImmutableArray<string> Text(Soldiers present) => ImmutableArray.Create(Name, present.ToString());
		public override bool Sailable => true;

		public virtual bool Equals(Sea? region) => region is not null && Id == region.Id;
		public override int GetHashCode() => Id.GetHashCode();

#pragma warning disable CS8618
		private Sea() { }
#pragma warning restore CS8618
	}
}