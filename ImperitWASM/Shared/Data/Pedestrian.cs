namespace ImperitWASM.Shared.Data
{
	public record Pedestrian : SoldierType
	{
		public Pedestrian(string name, string symbol, string text, int attackPower, int defensePower, int weight, int price)
			: base(name, symbol, text, attackPower, defensePower, weight, price) { }
		public override int CanMove(Provinces provinces, Province from, Province to)
		{
			return from.Mainland && to.Mainland && provinces.Passable(from, to, 1, (a, b) => a.Mainland && b.Mainland ? 1 : 2) ? Weight : 0;
		}
		public override bool IsRecruitable(Region region) => region.Mainland;
		public override int CanSustain(Province province) => province.Mainland ? Weight : 0;
	}
}
