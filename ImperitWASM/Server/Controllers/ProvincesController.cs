using System.Collections.Generic;
using System.Linq;
using ImperitWASM.Client.Data;
using ImperitWASM.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImperitWASM.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProvincesController : ControllerBase
	{
		readonly IProvinces provinces;
		readonly ISettings sl;
		readonly IPlayers players;
		public ProvincesController(IProvinces provinces, ISettings sl, IPlayers players)
		{
			this.provinces = provinces;
			this.sl = sl;
			this.players = players;
		}
		[HttpGet("Default")]
		public IEnumerable<ProvinceDisplay> Default()
		{
			return sl.Settings.Regions.Select(region => new ProvinceDisplay(region.Border, region.Center, region.Fill, region.Stroke, region.StrokeWidth, region.Text(region.Soldiers)));
		}
		[HttpPost("Free")]
		public IEnumerable<bool> Free([FromBody] long gameId) => provinces[gameId].Select(p => p.Inhabitable);
		[HttpPost("Current")]
		public IEnumerable<ProvinceUpdate> Current([FromBody] long gameId)
		{
			return provinces[gameId].Select(p => new ProvinceUpdate(p.Text, p.Fill));
		}
		[HttpPost("Preview")]
		public IEnumerable<ProvinceUpdate> Preview([FromBody] long gameId)
		{
			return players[gameId].Single(player => player.IsActive).Act(provinces[gameId], sl.Settings).Item2.Select(p => new ProvinceUpdate(p.Text, p.Fill));
		}
	}
}
