using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public record Human : Player
	{
		public string StringPassword { get; private set; }
		public Password Password => new Password(StringPassword);
		protected Human(string name, long gameId, int order, Color color, int money, bool alive, Settings settings, bool isActive)
			: base(name, gameId, order, color, money, alive, settings, isActive) => StringPassword = "";
		public Human(string name, long gameId, int order, Color color, int money, bool alive, Settings settings, bool isActive, Password password)
			: base(name, gameId, order, color, money, alive, settings, isActive) => StringPassword = password.ToString();

		public virtual bool Equals(Human? obj) => obj is not null && Name == obj.Name;
		public override int GetHashCode() => Name.GetHashCode();
	}
}
