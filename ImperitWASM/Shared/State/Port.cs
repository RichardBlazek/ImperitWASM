namespace ImperitWASM.Shared.State
{
	public class Port : Land
	{
		public Port(int id, string name, Shape shape, Army army, Army defaultArmy, bool isStart, int earnings, Settings settings)
			: base(id, name, shape, army, defaultArmy, isStart, earnings, settings) { }
		public override Province GiveUpTo(Army army) => new Port(Id, Name, Shape, army, DefaultArmy, IsStart, Earnings, settings);
		public override string[] Text => new[] { Name + "\u2693", Earnings + "\uD83D\uDCB0", Army.ToString() };
	}
}