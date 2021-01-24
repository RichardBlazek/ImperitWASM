using System;
using System.Collections.Immutable;
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
	public class GameController : ControllerBase
	{
		readonly IGames gs;
		readonly IGameCreator gameCreator;
		readonly IProvinces provinces;
		readonly IPlayers players;
		readonly ISessions session;
		readonly IEndOfTurn eot;
		readonly IPowers powers;
		public GameController(IGames gs, IGameCreator gameCreator, IProvinces provinces, ISessions session, IEndOfTurn eot, IPlayers players, IPowers powers)
		{
			this.gs = gs;
			this.gameCreator = gameCreator;
			this.provinces = provinces;
			this.session = session;
			this.eot = eot;
			this.players = players;
			this.powers = powers;
		}
		[HttpPost("Info")]
		public GameInfo Info([FromBody] string player) => players[player] is { IsHuman: true } H ? new(gs.Find(H.GameId)?.Current, H.IsActive) : new(null, false);
		[HttpPost("StartInfo")]
		public async Task<StartInfo> StartInfo([FromBody] int gameId)
		{
			await gameCreator.StartAllAsync();
			return gs.Find(gameId) is Game g ? new(g.Current, g.StartTime) : new(null, DateTime.MinValue);
		}
		[HttpPost("Winner")]
		public Winner? Winner([FromBody] int gameId) => provinces[gameId].Winner is ({ IsHuman: true } H, _) ? new Winner(H.Name, H.Color) : null;
		async Task<RegistrationErrors> DoRegistrationAsync(RegisteredPlayer player, Game game)
		{
			await gameCreator.RegisterAsync(game, player.N.Trim(), Password.FromPassword(player.P.Trim()), player.S);
			return RegistrationErrors.Ok;
		}
		[HttpPost("Register")]
		public async Task<RegistrationErrors> RegisterAsync([FromBody] RegisteredPlayer player) => player.N?.Trim() switch
		{
			null or { Length: 0 } => RegistrationErrors.NoName,
			string name when !players.IsFreeName(name) => RegistrationErrors.UsedName,
			_ when string.IsNullOrWhiteSpace(player.P) => RegistrationErrors.NoPassword,
			_ when !provinces[player.G, player.S].Inhabitable => RegistrationErrors.InvalidStart,
			_ when gs.Find(player.G) is Game game => await DoRegistrationAsync(player, game),
			_ => RegistrationErrors.BadGame
		};
		[HttpGet("RegistrableGame")]
		public async Task<int> RegistrableGameAsync()
		{
			await gameCreator.StartAllAsync();
			return gs.RegistrableGame ?? await gameCreator.CreateAsync();
		}
		[HttpPost("NextColor")]
		public Color NextColor([FromBody] int gameId) => gameCreator.NextColor(gameId);
		[HttpPost("NextTurn")]
		public async Task<bool> NextTurnAsync([FromBody] Session ses)
		{
			return players[ses.P] is { IsActive: true } player && session.IsValid(ses) && await eot.NextTurnAsync(player.GameId, ses.P);
		}
		[HttpPost("History")]
		public HistoryRecord? History([FromBody] string name)
		{
			if (players[name] is Player player)
			{
				var (play, prov) = (players[player.GameId], provinces[player.GameId]);
				var who = prov.Winner.Item1;
				return new HistoryRecord(powers.Get(player.GameId).Select(p => p.ToImmutableArray()).ToImmutableArray(), play.Select(p => p.Color).ToImmutableArray(), who?.Name ?? "", who?.Color ?? new Color());
			}
			return null;
		}
	}
}
