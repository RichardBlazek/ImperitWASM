using ImperitWASM.Server.Load;
using ImperitWASM.Shared.State;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace ImperitWASM.Server.Services
{
	public interface IFormerPlayersLoader : IReadOnlyList<Player>
	{
		void Reset(IEnumerable<Player> players);
	}
	public class FormerPlayersLoader : IFormerPlayersLoader
	{
		readonly JsonWriter<JsonPlayer, Player, Settings> loader;
		ImmutableArray<Player> players;
		public FormerPlayersLoader(IServiceIO io, ISettingsLoader sl)
		{
			loader = new JsonWriter<JsonPlayer, Player, Settings>(io.FormerPlayers, sl.Settings, JsonPlayer.From);
			players = loader.Load().ToImmutableArray();
		}
		public int Count => players.Length;
		public Player this[int i] => players[i];
		public IEnumerator<Player> GetEnumerator() => (players as IEnumerable<Player>).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public void Reset(IEnumerable<Player> new_players) => loader.Save(players = new_players.ToImmutableArray());
	}
}