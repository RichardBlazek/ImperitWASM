using System.Linq;
using ImperitWASM.Server.Load;
using ImperitWASM.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace ImperitWASM.Server.Services
{
	public interface ISettings
	{
		Settings Settings { get; }
	}
	public class SettingsLoader : ISettings
	{
		public Settings Settings { get; }
		public SettingsLoader()
		{
			using var ctx = new ImperitContext();
			Settings = ctx.Settings!.AsTracking().Include(settings => settings.SoldierTypeCollection)
				.Include(settings => settings.RegionCollection).ThenInclude(region => region.Settings)
				.Include(settings => settings.RegionCollection).ThenInclude(region => region.Shape).ThenInclude(shape => shape.Center)
				.Include(settings => settings.RegionCollection).ThenInclude(region => region.Shape).ThenInclude(shape => shape.Points)
				.Include(settings => settings.RegionCollection).ThenInclude(region => region.RegionSoldierTypes).ThenInclude(pst => pst.SoldierType)
				.Include(settings => settings.RegionCollection).ThenInclude(region => region.Soldiers).ThenInclude(soldiers => soldiers.Regiments).ThenInclude(regiment => regiment.Type).Single();
			ctx.DetachAllEntities();
		}
	}
}
