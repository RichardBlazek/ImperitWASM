using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ImperitWASM.Server.Db;
using ImperitWASM.Shared.Data;
using ImperitWASM.Shared.Value;
using Microsoft.EntityFrameworkCore;

namespace ImperitWASM.Server.Services
{
	public interface IProvinces
	{
		Provinces this[int gameId] { get; }
		void Set(int gameId, IEnumerable<Province> provinces);
		Province this[int gameId, int regionId] { get; }
	}
	public class ProvinceLoader : IProvinces
	{
		readonly Context ctx;
		readonly Graph graph;
		public ProvinceLoader(Context ctx, ISettings sl)
		{
			this.ctx = ctx;
			graph = sl.Graph;
		}
		IQueryable<Province> Included => ctx.Provinces!.Include(r => r.Region).ThenInclude(r => r.RegionSoldierTypes).ThenInclude(t => t.SoldierType)
			.Include(p => p.Player).Include(p => p.Soldiers).ThenInclude(s => s.Regiments).ThenInclude(r => r.Type);
		public Provinces this[int gameId] => new Provinces(Included.Where(province => province.GameId == gameId).OrderBy(province => province.RegionId).AsEnumerable().ToImmutableArray(), graph);
		public void Set(int gameId, IEnumerable<Province> provinces)
		{
			ctx.Provinces!.RemoveRange(ctx.Provinces!.Where(p => p.GameId == gameId));
			ctx.Provinces!.AddRange(provinces);
		}
		public Province this[int gameId, int regionId] => Included.Single(province => province.GameId == gameId && province.RegionId == regionId);
	}
}