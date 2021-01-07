using System.Text.Json.Serialization;

namespace ImperitWASM.Shared.Data
{
	public sealed record Power
	{
		[JsonIgnore] public int Id { get; private set; }
		[JsonIgnore] public string PlayerName { get; private set; }
		[JsonIgnore] public int Turn { get; private set; }
		[JsonInclude] public bool Alive { get; private set; }
		[JsonInclude] public int Final { get; private set; }
		[JsonInclude] public int Income { get; private set; }
		[JsonInclude] public int Money { get; private set; }
		[JsonInclude] public int Soldiers { get; private set; }
		public Power(string playerName, int turn, bool alive, int final, int income, int money, int soldiers) => (PlayerName, Turn, Alive, Final, Income, Money, Soldiers) = (playerName, turn, alive, final, income, money, soldiers);
		[JsonIgnore] public int Total => Alive ? Soldiers + Money + (Income * 5) + (Final * 100) : 0;
		[JsonIgnore] public int Military => Alive ? Soldiers + Money : 0;
		public static int operator -(Power a, Power b) => a.Total - b.Total;
	}
}