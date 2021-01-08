using ImperitWASM.Server.Load;
using ImperitWASM.Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ImperitWASM.Server
{
	public class Startup
	{
		public Startup(IConfiguration configuration) => Configuration = configuration;

		public IConfiguration Configuration { get; }
		public void ConfigureServices(IServiceCollection services)
		{
			_ = services.AddControllersWithViews();
			_ = services.AddRazorPages();
			_ = services.AddDbContext<ImperitContext>(ServiceLifetime.Singleton).AddSingleton<ISettings, SettingsLoader>()
						.AddSingleton(_ => GraphLoader.Graph(System.AppDomain.CurrentDomain.BaseDirectory ?? ".", "Files/Graph.json"))
						.AddTransient<IPowers, PowerLoader>().AddTransient<ISessions, SessionLoader>()
						.AddTransient<IProvinces, ProvinceLoader>().AddTransient<IGames, GameLoader>()
						.AddTransient<IPlayers, PlayerLoader>().AddTransient<IGameCreator, GameCreator>()
						.AddTransient<ICommands, CommandProvider>().AddTransient<IEndOfTurn, EndOfTurn>()
						.AddTransient<ISessions, SessionLoader>();
		}
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			_ = app.UseHsts().UseHttpsRedirection().UseBlazorFrameworkFiles().UseStaticFiles().UseRouting().UseEndpoints(endpoints =>
			{
				_ = endpoints.MapRazorPages();
				_ = endpoints.MapControllers();
				_ = endpoints.MapFallbackToFile("index.html");
				_ = endpoints.MapDefaultControllerRoute();
			});
		}
	}
}
