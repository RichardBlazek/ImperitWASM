using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using ImperitWASM.Server.Load;
using ImperitWASM.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace ImperitWASM.Server.Services
{
	public interface IGames
	{
		Task<Game> AddAsync();
		void RemoveOld(DateTime limit);
		Game? Find(int gameId);
		Task CountDownAsync(int gameId);
		Task StartAsync(int gameId);
		Task FinishAsync(int gameId);
		ImmutableArray<int> ShouldStart { get; }
		int? RegistrableGame { get; }
	}
	public class GameLoader : IGames
	{
		readonly ImperitContext ctx;
		readonly ISettings sl;
		public GameLoader(ImperitContext ctx, ISettings sl)
		{
			this.ctx = ctx;
			this.sl = sl;
		}

		public async Task<Game> AddAsync()
		{
			var game = ctx.Games!.Add(new Game()).Entity;
			_ = await ctx.SaveChangesAsync();
			return game;
		}
		public void RemoveOld(DateTime limit) => ctx.Games!.RemoveRange(ctx.Games.Where(game => game.Current == Game.State.Finished && game.FinishTime < limit));
		public Game? Find(int gameId) => ctx.Games!.SingleOrDefault(game => game.Id == gameId);
		public Task CountDownAsync(int gameId) => ctx.RunSqlAsync($"UPDATE Games SET Current={(int)Game.State.CountDown}, StartTime={DateTime.UtcNow.AddSeconds(sl.Settings.CountdownSeconds)} WHERE Id={gameId};");
		public Task StartAsync(int gameId) => ctx.RunSqlAsync($"UPDATE Games SET Current={(int)Game.State.Started} WHERE Id={gameId};");
		public Task FinishAsync(int gameId) => ctx.RunSqlAsync($"UPDATE Games SET Current={(int)Game.State.Finished}, FinishTime={DateTime.UtcNow} WHERE Id={gameId};");

		public ImmutableArray<int> ShouldStart => ctx.Games!.Where(game => game.Current == Game.State.CountDown && game.StartTime <= DateTime.UtcNow).Select(g => g.Id).ToImmutableArray();
		public int? RegistrableGame => ctx.Games!.FirstOrDefault(game => game.Current == Game.State.CountDown || game.Current == Game.State.Created)?.Id;
	}
}