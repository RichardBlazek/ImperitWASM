using System.Collections.Generic;
using System.Linq;
using ImperitWASM.Shared.Data;

namespace ImperitWASM.Shared.Commands
{
	public sealed record Recruit(Province Province, Soldiers Soldiers) : ICommand
	{
		public bool Allowed(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings)
		{
			return actor.IsActive && Province.IsAllyOf(actor) && actor.MaxUsableMoney >= Soldiers.Price && Soldiers.Any;
		}
		public (IEnumerable<Player>, IEnumerable<Province>) Perform(Player actor, IReadOnlyList<Player> players, Provinces provinces, Settings settings)
		{
			return (players.Select(altered => altered == actor ? altered.Pay(Soldiers.Price).Add(new Manoeuvre(Province, Soldiers)) : altered), provinces);
		}
	}
}