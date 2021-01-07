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
		Color NextColor(long gameId);
		Task<long> CreateAsync();
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
		public GameCreator(IProvinces provinces, IGames game, ISettings sl, ImperitContext ctx, IPlayers players)
		{
			this.provinces = provinces;
			this.gs = game;
			this.sl = sl;
			this.ctx = ctx;
			this.players = players;
		}
		public async Task<long> CreateAsync()
		{
			long id = ctx.Games!.Add(new Game()).Entity.Id;
			gs.RemoveOld(DateTime.UtcNow.AddDays(-1.0));
			await provinces.AddAsync(sl.Settings.Provinces(id));
			return id;
		}
		public Color NextColor(long gameId) => Settings.ColorOf(players[gameId].Length);
		void Start(Game g)
		{
			var prov = provinces[g.Id];
			foreach (var (land, robot) in sl.Settings.GetRobots(g.Id, players[g.Id].Length, prov.Inhabitable, players.ObsfuscateName))
			{
				players.Add(robot);
				_ = ctx.Provinces!.Update(prov[land].RuledBy(robot));
			}
		}
		public Task StartAllAsync()
		{
			gs.ShouldStart.Each(Start);
			return ctx.SaveChangesAsync();
		}
		public Task RegisterAsync(Game game, string name, Password password, int land)
		{
			int count = players[game.Id].Length;
			var player = sl.Settings.CreateHuman(name, game.Id, count, land, password);
			players.Add(player);
			_ = ctx.Provinces!.Update(provinces[game.Id, land].RuledBy(player));

			if (count == 2)
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