using System;
using System.Collections.Immutable;
using System.Linq;
using ImperitWASM.Server.Load;
using ImperitWASM.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace ImperitWASM.Server.Services
{
	public interface IGames
	{
		Game? Find(long gameId);
		Game? Update(long gameId, Func<Game, Game> update);
		Game Update(Game game);
		void RemoveOld(DateTime limit);
		ImmutableArray<Game> ShouldStart { get; }
		long? RegistrableGame { get; }
	}
	public class GameLoader : IGames
	{
		readonly ImperitContext ctx;
		public GameLoader(ImperitContext ctx) => this.ctx = ctx;

		public void RemoveOld(DateTime limit) => ctx.Games!.RemoveRange(ctx.Games.Where(game => game.Current == Game.State.Finished && game.FinishTime < limit));
		public Game? Find(long gameId) => ctx.Games.AsNoTracking().SingleOrDefault(game => game.Id == gameId);
		public Game? Update(long gameId, Func<Game, Game> update) => Find(gameId) is Game g ? ctx.Games!.Update(g).Entity : null;
		public Game Update(Game game) => ctx.Games!.Update(game).Entity;

		public ImmutableArray<Game> ShouldStart => ctx.Games!.AsNoTracking().Where(game => game.Current == Game.State.CountDown && game.StartTime <= DateTime.UtcNow).ToImmutableArray();
		public long? RegistrableGame => ctx.Games!.AsNoTracking().FirstOrDefault(game => game.Current == Game.State.CountDown || game.Current == Game.State.Created)?.Id;
	}
}