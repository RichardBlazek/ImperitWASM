using System;
using System.Linq;
using System.Threading.Tasks;
using ImperitWASM.Shared;
using ImperitWASM.Shared.Data;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Server.Services
{
	public interface IGameCreator
	{
		Color NextColor(int gameId);
		Task<int> CreateAsync();
		Task StartAllAsync();
		Task RegisterAsync(Game game, string name, Password password, int land);
	}
	public class GameCreator : IGameCreator
	{
		readonly IProvinces provinces_loader;
		readonly IPlayers players_loader;
		readonly IGames gs;
		readonly ISettings sl;
		readonly IChangeSaver changes;
		public GameCreator(IProvinces provinces, IGames gs, ISettings sl, IChangeSaver changes, IPlayers players)
		{
			this.provinces_loader = provinces;
			this.gs = gs;
			this.sl = sl;
			this.changes = changes;
			this.players_loader = players;
		}
		public async Task<int> CreateAsync()
		{
			var game = await gs.AddAsync();
			changes.Attach(sl.Settings);
			gs.RemoveOld(DateTime.UtcNow.AddDays(-1.0));
			provinces_loader.Set(game.Id, sl.Provinces(game.Id));
			await changes.SaveAsync();
			return game.Id;
		}
		public Color NextColor(int gameId) => Player.ColorOf(players_loader[gameId].Length);
		async Task StartAsync(int gameId)
		{
			await gs.StartAsync(gameId);
			var provinces = provinces_loader[gameId];
			var players = players_loader[gameId];
			var robots = sl.GetRobots(gameId, players.Length, provinces.Inhabitable.Shuffled(), players_loader.ObsfuscateName);

			var new_provinces = provinces.Items.Alter(robots.Select(pair => (pair.Item1, provinces[pair.Item1].RuledBy(pair.Item2))));
			var new_players = players.AddRange(robots.Select(pair => pair.Item2));
			changes.Change(gameId, new_players, new_provinces);
		}
		public async Task StartAllAsync()
		{
			foreach (int gameId in gs.ShouldStart)
			{
				await StartAsync(gameId);
			}
			await changes.SaveAsync();
		}
		public async Task RegisterAsync(Game game, string name, Password password, int land)
		{
			var players = players_loader[game.Id];
			var provinces = provinces_loader[game.Id];
			var added = sl.CreateHuman(name, game.Id, players.Length, land, password);

			var new_players = players.Add(added);
			var new_provinces = provinces.Items.Alter(new[] { (land, provinces[land].RuledBy(added)) });
			changes.Change(game.Id, new_players, new_provinces);
			await changes.SaveAsync();

			if (new_players.Length == 2)
			{
				await gs.CountDownAsync(game.Id);
			}
			if (new_players.Length >= sl.Settings.PlayerCount)
			{
				await StartAsync(game.Id);
				await changes.SaveAsync();
			}
		}
	}
}