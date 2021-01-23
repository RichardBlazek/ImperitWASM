using System.Collections.Immutable;
using System.Threading.Tasks;
using ImperitWASM.Shared.Commands;
using ImperitWASM.Shared.Data;

namespace ImperitWASM.Server.Services
{
	public interface ICommands
	{
		Task<(bool, ImmutableArray<Player>, Provinces)> PerformAsync(string actorName, ICommand command);
	}
	public class CommandProvider : ICommands
	{
		readonly IPlayers players;
		readonly IProvinces provinces;
		readonly ISettings sl;
		readonly IChangeSaver changes;
		public CommandProvider(IPlayers players, IProvinces provinces, ISettings sl, IChangeSaver changes)
		{
			this.players = players;
			this.provinces = provinces;
			this.sl = sl;
			this.changes = changes;
		}
		public async Task<(bool, ImmutableArray<Player>, Provinces)> PerformAsync(string actorName, ICommand command)
		{
			var actor = players[actorName]!;
			var loaded_players = players[actor.GameId];
			var loaded_provinces = provinces[actor.GameId];

			if (command.Allowed(actor, loaded_players, loaded_provinces, sl.Settings))
			{
				var (new_players, new_provinces) = command.Perform(actor, loaded_players, loaded_provinces, sl.Settings);
				var player_array = new_players.ToImmutableArray();
				var province_array = new_provinces.ToImmutableArray();

				players.Update(new_players);
				provinces.Update(new_provinces);
				await changes.SaveAsync();
				return (true, player_array, loaded_provinces.With(province_array));
			}
			return (false, loaded_players, loaded_provinces);
		}
	}
}
