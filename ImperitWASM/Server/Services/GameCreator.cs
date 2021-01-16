using System;
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
		readonly ImperitContext ctx;
		public GameCreator(IProvinces provinces, IGames gs, ISettings sl, ImperitContext ctx, IPlayers players)
		{
			this.provinces = provinces;
			this.gs = gs;
			this.sl = sl;
			this.ctx = ctx;
			this.players = players;
		}
		public async Task<int> CreateAsync()
		{
			var game = await gs.AddAsync();
			gs.RemoveOld(DateTime.UtcNow.AddDays(-1.0));
			await provinces.AddAsync(sl.Settings.Provinces(game.Id));
			return game.Id;
		}
		public Color NextColor(int gameId) => Settings.ColorOf(players[gameId].Length);
		void Start(Game g)
		{
			_ = gs.Update(g.Start());
			var prov = provinces[g.Id];
			foreach (var (land, robot) in sl.Settings.GetRobots(g.Id, players[g.Id].Length, prov.Inhabitable.Shuffled(), players.ObsfuscateName))
			{
				players.Add(robot);
				provinces.Update(prov[land].RuledBy(robot));
			}
		}
		public Task StartAllAsync()
		{
			gs.ShouldStart.Each(Start);
			return ctx.SaveChangesAsync();
		}
		public Task RegisterAsync(Game game, string name, Password password, int land)
		{
			int count = players.Count(game.Id);
			var player = sl.Settings.CreateHuman(name, game.Id, count, land, password);
			players.Add(player);
			provinces.Update(provinces[game.Id, land].RuledBy(player));

			if (count == 1)
			{
				_ = gs.Update(game.CountDown(DateTime.UtcNow.Add(sl.Settings.Countdown)));
			}
			else if (count + 1 >= sl.Settings.PlayerCount)
			{
				Start(game);
			}
			return ctx.SaveChangesAsync();
		}
	}
}