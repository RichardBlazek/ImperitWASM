using System.Text.Json.Serialization;

namespace ImperitWASM.Shared.Data
{
	public sealed record Power
	{
		[JsonIgnore] public int Id { get; private set; }
		[JsonIgnore] public int GameId { get; private set; }
		[JsonIgnore] public int Order { get; private set; }
		[JsonInclude] public bool Alive { get; private set; }
		[JsonInclude] public int Final { get; private set; }
		[JsonInclude] public int Income { get; private set; }
		[JsonInclude] public int Money { get; private set; }
		[JsonInclude] public int Soldiers { get; private set; }
		public Power(int gameId, int order, bool alive, int final, int income, int money, int soldiers) => (GameId, Order, Alive, Final, Income, Money, Soldiers) = (gameId, order, alive, final, income, money, soldiers);
		[JsonIgnore] public int Total => Alive ? Soldiers + Money + (Income * 5) + (Final * 100) : 0;
		[JsonIgnore] public int Military => Alive ? Soldiers + Money : 0;
		public static int operator -(Power a, Power b) => a.Total - b.Total;
	}
}