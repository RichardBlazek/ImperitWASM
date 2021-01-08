﻿using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using ImperitWASM.Server.Load;
using ImperitWASM.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace ImperitWASM.Server.Services
{
	public interface IPlayers
	{
		ImmutableArray<Player> this[int gameId] { get; }
		Player? this[string? name] { get; }
		void Add(Player player);
		Task UpdateAsync(IEnumerable<Player> players);
		bool IsFreeName(string name);
		string ObsfuscateName(string name, int repetition);
	}
	public class PlayerLoader : IPlayers
	{
		readonly ImperitContext ctx;
		public PlayerLoader(ImperitContext ctx) => this.ctx = ctx;
		IQueryable<Player> Included => ctx.Players!.Include(p => p.Settings).Include(p => p.ActionList)
			.Include(p => p.ActionList!.Where(a => a as Manoeuvre != null)).ThenInclude<Player, Action, Province>(a => ((Manoeuvre)a)!.Province).ThenInclude(p => p.Region)
			.Include(p => p.ActionList!.Where(a => a as Manoeuvre != null)).ThenInclude<Player, Action, Soldiers>(a => ((Manoeuvre)a)!.Soldiers).ThenInclude(s => s.Regiments).ThenInclude(r => r.Type);
		public ImmutableArray<Player> this[int gameId] => Included.AsNoTracking().Where(player => player.GameId == gameId).OrderBy(player => player.Order).AsEnumerable().ToImmutableArray();
		public Player? this[string? name] => Included.AsNoTracking().SingleOrDefault(player => player.Name == name);
		public void Add(Player player) => ctx.Players!.Add(player);
		public Task UpdateAsync(IEnumerable<Player> players)
		{
			ctx.Players!.UpdateRange(players);
			return ctx.SaveChangesAsync();
		}
		public bool IsFreeName(string name) => name.All(c => !char.IsDigit(c)) && ctx.Players!.All(p => p.Name != name);
		public string ObsfuscateName(string original, int repetition)
		{
			original = original.Trim();
			return ctx.Players!.Select(p => p.Name).Where(name => name.StartsWith(original)).ToList().Select(name => name[original.Length..]).Where(suf => suf.All(c => c is >= '0' and <= '9')).DefaultIfEmpty("").Max(n => n ?? "") switch
			{
				{ Length: > 0 } suf when suf[^1] >= '0' && suf[^1] < (char)('9' - repetition) => original + suf[..^1] + (char)(suf[^1] + 1 + repetition),
				var suf => original + suf + (repetition + 1)
			};
		}
	}
}
