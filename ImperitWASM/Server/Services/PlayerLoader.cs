using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ImperitWASM.Server.Load;
using ImperitWASM.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace ImperitWASM.Server.Services
{
	public interface IPlayers
	{
		ImmutableArray<Player> this[int gameId] { get; }
		void Set(int gameId, IEnumerable<Player> players);
		Player? this[string? name] { get; }
		bool IsFreeName(string name);
		string ObsfuscateName(string name, int repetition);
		int Count(int gameId);
	}
	public class PlayerLoader : IPlayers
	{
		readonly ImperitContext ctx;
		public PlayerLoader(ImperitContext ctx) => this.ctx = ctx;
		IQueryable<Player> Included => ctx.Players!.Include(p => p.ActionList).Include(p => p.ActionList!.Where(a => a as Manoeuvre != null)).ThenInclude<Player, Action, Soldiers>(a => ((Manoeuvre)a)!.Soldiers).ThenInclude(s => s.Regiments).ThenInclude(r => r.Type);
		public ImmutableArray<Player> this[int gameId] => Included.Where(player => player.GameId == gameId).OrderBy(player => player.Order).AsEnumerable().ToImmutableArray();
		public void Set(int gameId, IEnumerable<Player> players)
		{
			ctx.Players!.RemoveRange(ctx.Players!.Where(p => p.GameId == gameId));
			ctx.Players!.AddRange(players);
		}
		public Player? this[string? name] => Included.SingleOrDefault(player => player.Name == name);
		public void Add(Player player) => ctx.Players!.Add(player);
		public void Update(IEnumerable<Player> players) => ctx.Players!.ReplaceRange(players);
		public bool IsFreeName(string name) => name.Any(char.IsLetter) && !ctx.Players!.Any(p => p.Name == name);
		public string ObsfuscateName(string original, int repetition) => ctx.Players!.Select(p => p.Name).Where(name => name.StartsWith(original)).ToList().Select(name => name[original.Length..]).Where(suf => suf.All(c => c is >= '0' and <= '9')).DefaultIfEmpty("").Max(n => n ?? "") switch
		{
			{ Length: 0 } when repetition == 0 => original,
			{ Length: > 0 } suf when suf[^1] >= '0' && suf[^1] < (char)('9' - repetition) => original + suf[..^1] + (char)(suf[^1] + 1 + repetition),
			var suf => original + suf + (repetition + 2)
		};
		public int Count(int gameId) => ctx.Players!.Count(p => p.GameId == gameId);
	}
}
