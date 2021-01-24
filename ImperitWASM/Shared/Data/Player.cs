using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public sealed record Player
	{
		public string Name { get; private set; }
		public int GameId { get; private set; }
		public int Order { get; private set; }
		public int Money { get; private set; }
		public bool Alive { get; private set; }
		public ICollection<Action>? ActionList { get; private set; }
		public ImmutableArray<Action> Actions => ActionList!.ToImmutableArray();
		public bool IsActive { get; private set; }
		public bool IsHuman { get; private set; }
		public string StringPassword { get; private set; }
		public Password Password => new Password(StringPassword);
		public Player(string name, int gameId, int order, int money, bool alive, bool isActive, bool isHuman, Password password)
		{
			(Name, GameId, Order, Money, Alive, IsActive, IsHuman) = (name, gameId, order, money, alive, isActive, isHuman);
			StringPassword = password.ToString();
		}

		public static Color ColorOf(int i) => Color.Generate(i, 120.0, 1.0, 1.0);
		public Color Color => ColorOf(Order);
		public int MaxBorrowable(Settings s) => s.Discount(s.DebtLimit - Debt);
		public int MaxUsableMoney(Settings s) => Money + MaxBorrowable(s);
		public Player ChangeMoney(int amount) => this with { Money = amount + Money };
		public Player Earn(Provinces provinces) => ChangeMoney(provinces.IncomeOf(this));
		public Player InvertActive() => this with { IsActive = !IsActive };

		public Player Die() => this with { Money = 0, Alive = false, ActionList = new List<Action>() };
		public Player Add(params Action[] actions) => this with { ActionList = new List<Action>(ActionList!.Concat(actions)) };
		public Player Replace<T>(System.Func<T, bool> cond, T value, System.Func<T, T, T> interact) where T : Action
		{
			return this with { ActionList = Actions.Replace(cond, interact, value).ToList() };
		}
		public Player Borrow(int amount) => ChangeMoney(amount).Replace(a => true, new Loan(amount), (x, y) => x + y);
		public Player Pay(int amount) => (amount > Money ? Borrow(amount - Money) : this).ChangeMoney(-amount);

		public (Player, Provinces) Act(Provinces provinces, Settings settings)
		{
			var (new_player, new_provinces) = Actions.Aggregate((this with { ActionList = new List<Action>() }, provinces), (pair, action) =>
			{
				var (player, new_provinces, new_action) = action.Perform(pair.Item1, pair.provinces, settings);
				return (new_action is not null ? player.Add(new_action) : player, new_provinces);
			});
			return (new_player, new_provinces);
		}

		public int Debt => Actions.OfType<Loan>().Sum(a => a.Debt);
		public Power Power(int turn, IEnumerable<Province> provinces) => new Power(GameId, turn, Alive, provinces.Sum(p => p.Score), provinces.Sum(p => p.Income), Money - Debt, provinces.Sum(p => p.Power));
		public bool Equals(Player? obj) => obj is not null && Name == obj.Name;
		public override int GetHashCode() => Name.GetHashCode();

		public static Color ColorOf(Player? player) => player?.Color ?? new Color();

#pragma warning disable CS8618
		private Player() { }
#pragma warning restore CS8618
	}
}