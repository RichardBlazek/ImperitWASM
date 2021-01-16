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
		void Update(Province province);
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
		IQueryable<Province> Included => ctx.Provinces!.Include(p => p.Player).Include(p => p.Settings).Include(p => p.Soldiers).ThenInclude(s => s.Regiments).ThenInclude(r => r.Type)
			.Include(p => p.Region).ThenInclude(r => r.Settings).Include(r => r.Region).ThenInclude(r => r.RegionSoldierTypes).ThenInclude(t => t.SoldierType).AsNoTracking();
		public Provinces this[int gameId] => new Provinces(Included.Where(province => province.GameId == gameId).OrderBy(province => province.RegionId).AsEnumerable().ToImmutableArray(), graph);
		public Province this[int gameId, int regionId] => Included.Single(province => province.GameId == gameId && province.RegionId == regionId);

		public Task AddAsync(IEnumerable<Province> provinces)
		{
			ctx.Provinces!.AddRange(provinces);
			return ctx.SaveChangesAsync();
		}
		public void Update(Province province)
		{
			ctx.Entry(province).State = EntityState.Modified;
		}
		public Task UpdateAsync(IEnumerable<Province> provinces)
		{
			ctx.Provinces!.UpdateRange(provinces);
			return ctx.SaveChangesAsync();
		}
	}
}