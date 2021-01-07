namespace ImperitWASM.Shared.Data
{
	public record ProvinceSoldierType
	{
		public int Id { get; private set; }
		public int SoldierTypeId { get; private set; }
		public virtual SoldierType SoldierType { get; private set; }
		public ProvinceSoldierType(SoldierType soldierType) => SoldierType = soldierType;
	}
}
