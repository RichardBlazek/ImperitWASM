namespace ImperitWASM.Shared.Data
{
	public record OutlandishShip : Ship
	{
		public int Speed { get; private set; }
		public OutlandishShip(string name, string symbol, string text, int attackPower, int defensePower, int weight, int price, int capacity, int speed)
			: base(name, symbol, text, attackPower, defensePower, weight, price, capacity) => Speed = speed;

		int Difficulty(Province to) => to.Sailable ? 1 : Speed + 1;
		public override int CanMove(Provinces provinces, Province from, Province dest)
		{
			return from.Sailable && dest.Sailable && provinces.Passable(from, dest, Speed, (_, to) => Difficulty(to)) ? Capacity + Weight : 0;
		}
		public override bool IsRecruitable(Region region) => region.IsRecruitable(this);
	}
}
