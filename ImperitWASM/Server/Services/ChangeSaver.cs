using System.Collections.Generic;
using System.Threading.Tasks;
using ImperitWASM.Server.Db;
using ImperitWASM.Shared.Data;

namespace ImperitWASM.Server.Services
{
	public interface IChangeSaver
	{
		Task SaveAsync();
		void Detach(System.Func<object, bool> entity);
		void Change(int gameId, IEnumerable<Player> players, IEnumerable<Province> provinces);
	}
	public class ChangeSaver : IChangeSaver
	{
		readonly Context ctx;
		readonly IProvinces provinces_loader;
		readonly IPlayers players_loader;
		readonly ISettings sl;
		public ChangeSaver(Context ctx, IProvinces provinces_loader, IPlayers players_loader, ISettings sl)
		{
			this.ctx = ctx;
			this.provinces_loader = provinces_loader;
			this.players_loader = players_loader;
			this.sl = sl;
		}
		public Task SaveAsync() => ctx.SaveChangesAsync();
		public void Detach(System.Func<object, bool> entity) => ctx.Detach(entity);
		public void Change(int gameId, IEnumerable<Player> players, IEnumerable<Province> provinces)
		{
			ctx.Detach(o => true);

			players_loader.Set(gameId, players);
			provinces_loader.Set(gameId, provinces);
		}
	}
}
