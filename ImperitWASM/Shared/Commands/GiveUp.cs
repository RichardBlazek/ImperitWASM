using System.Collections.Generic;
using System.Linq;
using ImperitWASM.Shared.Data;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Commands
{
	public sealed record GiveUp : NextTurn
	{
		public override bool Allowed(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings, IEnumerable<SoldierType> soldierTypes, IReadOnlyList<Region> regions) => true;
		public override (IEnumerable<Player>, IEnumerable<Province>) Perform(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings, IEnumerable<SoldierType> soldierTypes, IReadOnlyList<Region> regions)
		{
			var (new_players, new_provinces) = actor.IsActive ? base.Perform(actor, players, provinces, settings, soldierTypes, regions) : (players, provinces);
			return (new_players.Select(altered => actor == altered ? altered.Die() : altered), new_provinces.Select(altered => altered.IsAllyOf(actor) ? altered.Revolt() : altered));
		}
	}
}
