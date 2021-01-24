using System.Collections.Generic;
using System.Linq;
using ImperitWASM.Shared.Data;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Commands
{
	public sealed record Buy(Province Province) : ICommand
	{
		public bool Allowed(Player actor, Provinces provinces, Settings settings)
		{
			return actor.MaxUsableMoney(settings) >= Province.Price && provinces.HasNeighborRuledBy(Province, actor);
		}
		public bool Allowed(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings, IEnumerable<SoldierType> soldierTypes, IReadOnlyList<Region> regions)
		{
			return Allowed(actor, provinces, settings);
		}
		public (IEnumerable<Player>, IEnumerable<Province>) Perform(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings, IEnumerable<SoldierType> soldierTypes, IReadOnlyList<Region> regions)
		{
			return (players.Select(altered => altered == actor ? altered.Pay(Province.Price) : altered), provinces.Select(altered => Province == altered ? altered.RuledBy(actor) : altered));
		}
	}
}