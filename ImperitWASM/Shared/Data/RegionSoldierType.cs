namespace ImperitWASM.Shared.Data
{
	public record RegionSoldierType
	{
		public int Id { get; private set; }
		public virtual SoldierType SoldierType { get; private set; }
		public RegionSoldierType(SoldierType soldierType) => SoldierType = soldierType;

#pragma warning disable CS8618
		private RegionSoldierType() { }
#pragma warning restore CS8618
	}
}
