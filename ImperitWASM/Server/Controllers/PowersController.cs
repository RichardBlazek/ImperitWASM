using System.Collections.Generic;
using ImperitWASM.Server.Services;
using ImperitWASM.Shared.Data;
using Microsoft.AspNetCore.Mvc;

namespace ImperitWASM.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PowersController : ControllerBase
	{
		readonly IPowers ctx;
		public PowersController(IPowers ctx) => this.ctx = ctx;
		[HttpPost("List")] public IEnumerable<IEnumerable<Power>> List([FromBody] int gameId) => ctx.Get(gameId);
	}
}
