using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImperitWASM.Server.Load;
using ImperitWASM.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace ImperitWASM.Server.Services
{
	public interface IPowers
	{
		IEnumerable<IEnumerable<Power>> Get(long gameId);
		Task AddAsync(IEnumerable<Power> powers);
		int Count(long gameId);
	}
	public class PowerLoader : IPowers
	{
		readonly ImperitContext ctx;
		public PowerLoader(ImperitContext context) => ctx = context;

		public IEnumerable<IEnumerable<Power>> Get(long gameId)
		{
			var array = ctx.Powers!.AsNoTracking().Join(ctx.Players!.Where(player => player.GameId == gameId), power => power.PlayerName, player => player.Name, (power, player) => power).ToArray();
			return array.GroupBy(power => power.Turn).OrderBy(powers => powers.Key).Select(powers => powers.OrderBy(power => power.PlayerName));
		}
		public Task AddAsync(IEnumerable<Power> powers)
		{
			ctx.Powers!.AddRange(powers);
			return ctx.SaveChangesAsync();
		}
		public int Count(long gameId) => ctx.Powers!.AsNoTracking().Join(ctx.Players!.Where(player => player.GameId == gameId), power => power.PlayerName, player => player.Name, (power, player) => power).Count();
	}
}
