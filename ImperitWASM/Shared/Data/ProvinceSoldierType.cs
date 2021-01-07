namespace ImperitWASM.Shared.Data
{
	public record ProvinceSoldierType
	{
		public int Id { get; private set; }
		public virtual SoldierType SoldierType { get; private set; }
		public ProvinceSoldierType(SoldierType soldierType) => SoldierType = soldierType;

#pragma warning disable CS8618
		private ProvinceSoldierType() { }
#pragma warning restore CS8618
	}
}
