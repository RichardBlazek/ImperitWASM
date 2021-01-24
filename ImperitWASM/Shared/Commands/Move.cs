using System.Collections.Generic;
using System.Linq;
using ImperitWASM.Shared.Data;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Commands
{
	public sealed record Move(Province From, Province To, Soldiers Soldiers) : ICommand
	{
		public bool Allowed(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings, IEnumerable<SoldierType> soldierTypes, IReadOnlyList<Region> regions)
		{
			return actor.IsActive && From.IsAllyOf(actor) && From.CanMove(provinces, To, Soldiers);
		}
		public (IEnumerable<Player>, IEnumerable<Province>) Perform(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings, IEnumerable<SoldierType> soldierTypes, IReadOnlyList<Region> regions)
		{
			return (players.Select(altered => altered == actor ? altered.Add(new Manoeuvre(To.RegionId, Soldiers)) : altered), provinces.Select(altered => altered == From ? altered.Subtract(Soldiers) : altered));
		}
		public bool HasEnoughCapacity(Provinces provinces) => Soldiers.Capacity(provinces, From, To) >= 0;
	}
}