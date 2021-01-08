namespace ImperitWASM.Shared.Data
{
	public sealed record RegionSoldierType
	{
		public int Id { get; private set; }
		public SoldierType SoldierType { get; private set; }
		public RegionSoldierType(SoldierType soldierType) => SoldierType = soldierType;

#pragma warning disable CS8618
		private RegionSoldierType() { }
#pragma warning restore CS8618
	}
}
