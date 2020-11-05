﻿using System.Collections.Generic;
using System.Linq;
using ImperitWASM.Server.Services;
using ImperitWASM.Shared.State;
using Microsoft.AspNetCore.Mvc;

namespace ImperitWASM.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProvincesController : ControllerBase
	{
		readonly IPlayersProvinces pap;
		readonly IActive active;
		public ProvincesController(IPlayersProvinces pap, IActive active)
		{
			this.pap = pap;
			this.active = active;
		}
		[HttpGet("Shapes")]
		public IEnumerable<Client.Server.DisplayableShape> Shapes([FromBody] int gameId)
		{
			return pap[gameId].Provinces.Select(p => new Client.Server.DisplayableShape(p.ToArray(), p.Center, p.Fill, p.Stroke, p.StrokeWidth, p is Land land && !land.Occupied && land.IsStart, p.Text));
		}
		[HttpGet("Current")]
		public IEnumerable<Client.Server.ProvinceVariables> Current([FromBody] int gameId)
		{
			return pap[gameId].Provinces.Select(p => new Client.Server.ProvinceVariables(p.Text, p.Fill));
		}
		[HttpPost("Preview")]
		public IEnumerable<Client.Server.ProvinceVariables> Preview([FromBody] int gameId)
		{
			var preview = pap[gameId].Act(active[gameId], false).Provinces;
			return preview.Select(p => new Client.Server.ProvinceVariables(p.Text, p.Fill));
		}
		[HttpGet("Instabilities")]
		public IEnumerable<Client.Server.ProvinceInstability> Instabilities([FromBody] int gameId)
		{
			return pap[gameId].Provinces.OfType<Land>().Where(l => l.Occupied && l.Instability.IsZero).Select(l => new Client.Server.ProvinceInstability(l.Name, l.Fill, l.Instability));
		}
	}
}
