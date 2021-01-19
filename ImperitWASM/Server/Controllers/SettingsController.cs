using System.Collections.Generic;
using System.Linq;
using ImperitWASM.Client.Data;
using ImperitWASM.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace ImperitWASM.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SettingsController : ControllerBase
	{
		readonly ISettings sl;
		public SettingsController(ISettings sl) => this.sl = sl;
		[HttpGet("Types")]
		public IEnumerable<SoldierItem> Types() => sl.Settings.SoldierTypes.Select(type => new SoldierItem(type.Name, type.Symbol, type.Text, type.Price));
	}
}
