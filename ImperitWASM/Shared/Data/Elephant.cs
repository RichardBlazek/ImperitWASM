namespace ImperitWASM.Shared.Data
{
	public record Elephant : Pedestrian
	{
		public int Capacity { get; private set; }
		public int Speed { get; private set; }
		public Elephant(string name, string symbol, string text, int attackPower, int defensePower, int weight, int price, int capacity, int speed)
			: base(name, symbol, text, attackPower, defensePower, weight, price) => (Capacity, Speed) = (capacity, speed);

		public override int CanMove(Provinces provinces, Province from, Province to)
		{
			return from.Mainland && to.Mainland && provinces.Passable(from, to, Speed, (_, dest) => dest.Dry ? 1 : Speed + 1) ? Weight + Capacity : 0;
		}
		public override int CanSustain(Province province) => province.Mainland ? Capacity + Weight : 0;
		public override bool IsRecruitable(Region region) => region.IsRecruitable(this);
	}
}
