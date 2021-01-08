using System.Linq;
using ImperitWASM.Client.Data;
using ImperitWASM.Server.Services;
using ImperitWASM.Shared.Commands;
using ImperitWASM.Shared.Data;
using Microsoft.AspNetCore.Mvc;

namespace ImperitWASM.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SwitchController : ControllerBase
	{
		readonly IProvinces provinces;
		readonly IPlayers players;
		readonly Settings settings;
		public SwitchController(IProvinces provinces, Settings settings, IPlayers players)
		{
			this.provinces = provinces;
			this.settings = settings;
			this.players = players;
		}

		bool IsPossible(Provinces provinces, Player player, Switch s) => s.From is int from && s.To is int to && s.View switch
		{
			View.Recruit => settings.RecruitableIn(to).Any(),
			View.Move => provinces[from].CanAnyMove(provinces, provinces[to]),
			View.Purchase => provinces[to].Mainland && new Buy(provinces[to]).Allowed(player, provinces),
			_ => false
		};
		Switch IfPossible(Provinces provinces, Player player, Switch s) => IsPossible(provinces, player, s) ? s : new Switch(s.Select, View.Map, null, null);
		static Switch ClickedResult(Provinces provinces, Player player, Click c) => c.From switch
		{
			int start => new Switch(null, start == c.Clicked ? View.Recruit : View.Move, start, c.Clicked),
			_ when provinces[c.Clicked].IsAllyOf(player) => new Switch(c.Clicked, View.Map, null, null),
			_ when provinces[c.Clicked] is { Mainland: true, Inhabited: false } => new Switch(null, View.Purchase, c.Clicked, c.Clicked),
			_ => new Switch(null, View.Map, null, null)
		};
		[HttpPost("Clicked")]
		public Switch Clicked([FromBody] Click c)
		{
			var player = players[c.P]!;
			var provs = provinces[player.GameId];
			return IfPossible(provs, player, player.IsActive ? ClickedResult(provs, player, c) : new Switch(null, View.Map, null, null));
		}
	}
}
