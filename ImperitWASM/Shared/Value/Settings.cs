namespace ImperitWASM.Shared.Value
{
	public sealed record Settings(int CountdownSeconds, int DebtLimit, int DefaultMoney, int FinalLandsCount, Ratio Interest, int PlayerCount)
	{
		public int CalculateDebt(int amount) => Interest.Interest(amount);
		public int Discount(int amount) => Interest.Discount(amount);
	}
}