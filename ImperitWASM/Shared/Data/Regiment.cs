namespace ImperitWASM.Shared.Data
{
	public sealed record Regiment
	{
		public int Id { get; private set; }
		public SoldierType Type { get; private set; }
		public int Count { get; private set; }
		public Regiment(SoldierType type, int count) => (Type, Count) = (type, count);

		public int AttackPower => Count * Type.AttackPower;
		public int DefensePower => Count * Type.DefensePower;
		public int Power => Count * Type.Power;
		public int Weight => Count * Type.Weight;
		public int Price => Count * Type.Price;

		public int CanMove(Provinces pap, Province from, Province to) => Type.CanMove(pap, from, to) * Count;
		public bool CanMoveAlone(Provinces pap, Province from, Province to) => Type.CanMoveAlone(pap, from, to);
		public int CanSustain(Province province) => Type.CanSustain(province) * Count;
		public override string ToString() => Count + Type.Symbol;

#pragma warning disable CS8618
		private Regiment() { }
#pragma warning restore CS8618
	}
}
