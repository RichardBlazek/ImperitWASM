using System.Collections.Generic;
using System.Linq;
using ImperitWASM.Shared.Data;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Commands
{
	public sealed record Donate(Player Recipient, int Amount) : ICommand
	{
		public bool Allowed(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings, IEnumerable<SoldierType> soldierTypes, IReadOnlyList<Region> regions)
		{
			return actor.Money >= Amount && Amount > 0;
		}
		public (IEnumerable<Player>, IEnumerable<Province>) Perform(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings, IEnumerable<SoldierType> soldierTypes, IReadOnlyList<Region> regions)
		{
			return (players.Select(altered => altered == Recipient ? altered.ChangeMoney(Amount) : altered == actor ? altered.ChangeMoney(-Amount) : altered), provinces);
		}
	}
}