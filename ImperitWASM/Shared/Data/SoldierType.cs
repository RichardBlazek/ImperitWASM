namespace ImperitWASM.Shared.Data
{
	public abstract record SoldierType
	{
		public string Name { get; private set; }
		public string Symbol { get; private set; }
		public string Text { get; private set; }
		public int AttackPower { get; private set; }
		public int DefensePower { get; private set; }
		public int Weight { get; private set; }
		public int Price { get; private set; }
		public SoldierType(string name, string symbol, string text, int attackPower, int defensePower, int weight, int price) => (Name, Symbol, Text, AttackPower, DefensePower, Weight, Price) = (name, symbol, text, attackPower, defensePower, weight, price);

		public int Power => AttackPower + DefensePower;
		public abstract bool IsRecruitable(Region region);
		public abstract int CanSustain(Province province);
		public abstract int CanMove(Provinces provinces, Province from, Province to);
		public bool CanMoveAlone(Provinces provinces, Province from, Province to) => CanMove(provinces, from, to) >= Weight;
	}
}
