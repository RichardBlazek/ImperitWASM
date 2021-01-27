﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImperitWASM.Server.Db;
using ImperitWASM.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace ImperitWASM.Server.Services
{
	public interface IPowers
	{
		IEnumerable<IEnumerable<Power>> Get(int gameId);
		Task AddAsync(IEnumerable<Power> powers);
		int Count(int gameId);
	}
	public class PowerLoader : IPowers
	{
		readonly Context ctx;
		readonly IPlayers players;
		readonly IChangeSaver changes;
		public PowerLoader(Context ctx, IPlayers players, IChangeSaver changes)
		{
			this.ctx = ctx;
			this.players = players;
			this.changes = changes;
		}

		public IEnumerable<IEnumerable<Power>> Get(int gameId)
		{
			int count = players.Count(gameId);
			return ctx.Powers!.AsNoTracking().Where(power => power.GameId == gameId).AsEnumerable().GroupBy(power => power.Order / count).OrderBy(powers => powers.Key).Select(powers => powers.OrderBy(power => power.Order));
		}
		public Task AddAsync(IEnumerable<Power> powers)
		{
			ctx.Powers!.AddRange(powers);
			return changes.SaveAsync();
		}
		public int Count(int gameId) => ctx.Powers!.Count(power => power.GameId == gameId);
	}
}
