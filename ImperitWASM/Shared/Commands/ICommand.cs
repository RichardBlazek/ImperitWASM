using System.Collections.Generic;
using ImperitWASM.Shared.Data;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Commands
{
	public interface ICommand
	{
		bool Allowed(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings, IEnumerable<SoldierType> soldierTypes, IReadOnlyList<Region> regions) => true;
		(IEnumerable<Player>, IEnumerable<Province>) Perform(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings, IEnumerable<SoldierType> soldierTypes, IReadOnlyList<Region> regions) => (players, provinces);
	}
}
