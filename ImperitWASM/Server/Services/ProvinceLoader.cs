using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using ImperitWASM.Server.Load;
using ImperitWASM.Shared.Data;
using ImperitWASM.Shared.Value;
using Microsoft.EntityFrameworkCore;

namespace ImperitWASM.Server.Services
{
	public interface IProvinces
	{
		Provinces this[int gameId] { get; }
		Province this[int gameId, int regionId] { get; }
		Task AddAsync(IEnumerable<Province> provinces);
		Task UpdateAsync(IEnumerable<Province> provinces);
	}
	public class ProvinceLoader : IProvinces
	{
		readonly ImperitContext ctx;
		readonly Graph graph;
		public ProvinceLoader(ImperitContext ctx, Graph graph)
		{
			this.ctx = ctx;
			this.graph = graph;
		}
		public Provinces this[int gameId] => new Provinces(ctx.Provinces!.AsNoTracking().Where(province => province.GameId == gameId).OrderBy(province => province.RegionId).AsEnumerable().ToImmutableArray(), graph);
		public Province this[int gameId, int regionId] => ctx.Provinces!.AsNoTracking().Single(province => province.GameId == gameId && province.RegionId == regionId);

		public Task AddAsync(IEnumerable<Province> provinces)
		{
			ctx.Provinces!.AddRange(provinces);
			return ctx.SaveChangesAsync();
		}
		public Task UpdateAsync(IEnumerable<Province> provinces)
		{
			ctx.Provinces!.UpdateRange(provinces);
			return ctx.SaveChangesAsync();
		}
	}
}