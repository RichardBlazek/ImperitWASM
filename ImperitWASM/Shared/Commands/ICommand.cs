using System.Collections.Generic;
using ImperitWASM.Shared.Data;

namespace ImperitWASM.Shared.Commands
{
	public interface ICommand
	{
		bool Allowed(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings) => true;
		(IEnumerable<Player>, IEnumerable<Province>) Perform(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings) => (players, provinces);
	}
}
