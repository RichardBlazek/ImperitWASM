namespace ImperitWASM.Shared.Data
{
	public record Pedestrian : SoldierType
	{
		public Pedestrian(string name, string symbol, string text, int attackPower, int defensePower, int weight, int price)
			: base(name, symbol, text, attackPower, defensePower, weight, price) { }
		public override int CanMove(Provinces provinces, Province from, Province to)
		{
			return from.Walkable && to.Walkable && provinces.Passable(from, to, 1, (a, b) => a.Walkable && b.Walkable ? 1 : 2) ? Weight : 0;
		}
		public override bool IsRecruitable(Region region) => region.Walkable;
		public override int CanSustain(Province province) => province.Walkable ? Weight : 0;
	}
}
