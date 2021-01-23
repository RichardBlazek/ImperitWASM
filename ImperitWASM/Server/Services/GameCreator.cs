using System;
using System.Linq;
using System.Threading.Tasks;
using ImperitWASM.Server.Load;
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
		readonly IProvinces provinces;
		readonly IPlayers players;
		readonly IGames gs;
		readonly ISettings sl;
		readonly IChangeSaver changes;
		public GameCreator(IProvinces provinces, IGames gs, ISettings sl, IChangeSaver changes, IPlayers players)
		{
			this.provinces = provinces;
			this.gs = gs;
			this.sl = sl;
			this.changes = changes;
			this.players = players;
		}
		public async Task<int> CreateAsync()
		{
			var game = await gs.AddAsync();
			gs.RemoveOld(DateTime.UtcNow.AddDays(-1.0));
			provinces.Add(sl.Settings.Provinces(game.Id));
			await changes.SaveAsync();
			return game.Id;
		}
		public Color NextColor(int gameId) => Settings.ColorOf(players[gameId].Length);
		async Task StartAsync(int gameId)
		{
			await gs.StartAsync(gameId);
			var prov = provinces[gameId];
			foreach (var (land, robot) in sl.Settings.GetRobots(gameId, players[gameId].Length, prov.Inhabitable.Shuffled(), players.ObsfuscateName))
			{
				players.Add(robot);
				provinces.Update(prov[land].RuledBy(robot));
			}
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
			int count = players.Count(game.Id);
			var player = sl.Settings.CreateHuman(name, game.Id, count, land, password);
			players.Add(player);
			provinces.Update(provinces[game.Id, land].RuledBy(player));

			if (count == 1)
			{
				await gs.CountDownAsync(game.Id);
			}
			if (count + 1 >= sl.Settings.PlayerCount)
			{
				await StartAsync(game.Id);
			}
			await changes.SaveAsync();
		}
	}
}