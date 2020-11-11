using System.Linq;
using ImperitWASM.Shared.State;

namespace ImperitWASM.Shared.Motion.Commands
{
	public class Buy : ICommand
	{
		public readonly Player Player;
		public readonly Province Province;
		public readonly int Price;
		public Buy(Player player, Province province, int price)
		{
			Player = player;
			Province = province;
			Price = price;
		}
		public bool Allowed(PlayersAndProvinces pap)
			=> Player.Money >= Price && pap.NeighborsOf(Province).Any(prov => prov is Land land && land.IsAllyOf(Player));
		public Province Perform(Province province)
		{
			return province == Province ? province.GiveUpTo(Player) : province;
		}
		public Player Perform(Player player, PlayersAndProvinces pap)
		{
			return Player == player ? player.ChangeMoney(-Price) : player;
		}
	}
}