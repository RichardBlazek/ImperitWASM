namespace ImperitWASM.Shared.Data
{
	public record Ship : SoldierType
	{
		public int Capacity { get; private set; }
		public Ship(string name, string symbol, string text, int attackPower, int defensePower, int weight, int price, int capacity)
			: base(name, symbol, text, attackPower, defensePower, weight, price) => Capacity = capacity;
		public override int CanMove(Provinces provinces, Province from, Province dest)
		{
			return provinces.Passable(from, dest, 1, (a, b) => a.Sailable && b.Sailable ? 1 : 2) ? Capacity + Weight : 0;
		}
		public override bool IsRecruitable(Region region) => region.Port;
		public override int CanSustain(Province p) => p.Sailable ? Capacity + Weight : 0;
	}
}
