using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public record Mountains : Region
	{
		public Mountains(string name, Shape shape, Soldiers soldiers, Settings settings) : base(name, shape, soldiers, settings) { }

		public override Color Stroke => Settings.MountainsColor;
		public override float StrokeWidth => Settings.MountainsWidth;

		public override bool Dry => true;

		public virtual bool Equals(Mountains? region) => region is not null && Id == region.Id;
		public override int GetHashCode() => Id.GetHashCode();

#pragma warning disable CS8618
		private Mountains() { }
#pragma warning restore CS8618
	}
}