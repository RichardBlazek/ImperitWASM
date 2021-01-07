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
		IEnumerable<IEnumerable<Power>> Get(int gameId);
		Task AddAsync(IEnumerable<Power> powers);
		int Count(int gameId);
	}
	public class PowerLoader : IPowers
	{
		readonly ImperitContext ctx;
		public PowerLoader(ImperitContext context) => ctx = context;

		public IEnumerable<IEnumerable<Power>> Get(int gameId)
		{
			int count = ctx.Players!.AsNoTracking().Count(p => p.GameId == gameId);
			return ctx.Powers!.AsNoTracking().Where(power => power.GameId == gameId).AsEnumerable().GroupBy(power => power.Order / count).OrderBy(powers => powers.Key).Select(powers => powers.OrderBy(power => power.Order));
		}
		public Task AddAsync(IEnumerable<Power> powers)
		{
			ctx.Powers!.AddRange(powers);
			return ctx.SaveChangesAsync();
		}
		public int Count(int gameId) => ctx.Powers!.AsNoTracking().Count(power => power.GameId == gameId);
	}
}
