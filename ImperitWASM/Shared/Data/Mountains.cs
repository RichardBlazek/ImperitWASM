namespace ImperitWASM.Shared.Data
{
	public record Mountains : Region
	{
		public override bool Dry => true;
		public virtual bool Equals(Mountains? region) => region is not null && Id == region.Id;
		public override int GetHashCode() => Id.GetHashCode();

#pragma warning disable CS8618
		private Mountains() { }
#pragma warning restore CS8618
	}
}