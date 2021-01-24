using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ImperitWASM.Client.Data;
using ImperitWASM.Server.Services;
using ImperitWASM.Shared.Data;
using ImperitWASM.Shared.Value;
using Microsoft.AspNetCore.Mvc;

namespace ImperitWASM.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class PlayerController : ControllerBase
	{
		readonly ISessions session;
		readonly IProvinces provinces;
		readonly IPlayers players;
		readonly IGameCreator gameCreator;
		readonly IGames gs;
		public PlayerController(IProvinces provinces, ISessions session, IPlayers players, IGameCreator gameCreator, IGames gs)
		{
			this.provinces = provinces;
			this.session = session;
			this.players = players;
			this.gameCreator = gameCreator;
			this.gs = gs;
		}
		[HttpPost("Active")]
		public int Active([FromBody] int gameId) => players[gameId].First(player => player.IsActive).Order;
		[HttpPost("Colors")]
		public IEnumerable<Color> Colors([FromBody] int gameId) => players[gameId].Select(p => p.Color);
		[HttpPost("Money")]
		public int Money([FromBody] string name) => players[name]?.Money ?? 0;
		[HttpPost("Color")]
		public Color ColorFn([FromBody] string name) => players[name]?.Color ?? new Color();
		[HttpPost("Infos")]
		public IEnumerable<PlayerInfo> Infos([FromBody] int gameId)
		{
			var prov = provinces[gameId];
			return players[gameId].Select(p => new PlayerInfo(p.Name, p.Color, p.Alive, p.Money, p.Debt, prov.IncomeOf(p)));
		}
		[HttpPost("Correct")]
		public Game.State? Correct([FromBody] Session user) => session.IsValid(user) && gs.Find(players[user.P]?.GameId ?? 0) is Game g ? g.Current : null;
		[HttpPost("Login")]
		public async Task<LoginResult> Login([FromBody] Login trial)
		{
			await gameCreator.StartAllAsync();
			return players[trial.N] is { IsHuman: true } h && h.Password.IsCorrect(trial.P) ? new LoginResult(new Session(trial.N, await session.AddAsync(trial.N)), h.Order, h.GameId) : new LoginResult(new Session(), -1, -1);
		}
		[HttpPost("Logout")]
		public Task Logout([FromBody] Session user) => session.RemoveAsync(user);
	}
}