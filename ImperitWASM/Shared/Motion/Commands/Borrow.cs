using ImperitWASM.Shared.State;

namespace ImperitWASM.Shared.Motion.Commands
{
	public record Borrow(Player Player, int Amount, Settings Settings) : ICommand
	{
		public bool Allowed(PlayersAndProvinces pap)
		{
			return Amount <= Settings.DebtLimit && Amount > 0;
		}
		public Player Perform(Player player, PlayersAndProvinces pap)
		{
			return player == Player ? player.Borrow(Amount) : player;
		}
	}
}