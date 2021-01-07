namespace ImperitWASM.Shared.Data
{
	public record Loan : Action
	{
		public int Debt { get; private set; }
		public Loan(int debt) => Debt = debt;
		public static Loan operator +(Loan a, Loan b) => new Loan(a.Debt + b.Debt);

		public override (Player, Provinces, Action?) Perform(Player active, Provinces provinces, Settings settings)
		{
			int next_debt = settings.CalculateDebt(Debt);
			return next_debt <= active.Money
				? (active.ChangeMoney(-next_debt), provinces, null)
				: next_debt <= settings.DebtLimit
				? (active.ChangeMoney(-active.Money), provinces, new Loan(next_debt - active.Money))
				: (active.ChangeMoney(-active.Money), provinces.With(provinces.Shuffled().SelectAccumulate(next_debt - active.Money, (p, acc) => p.IsAllyOf(active) && acc > 0 ? (p.Revolt(), acc - p.Price) : (p, acc))), null);
		}
	}
}