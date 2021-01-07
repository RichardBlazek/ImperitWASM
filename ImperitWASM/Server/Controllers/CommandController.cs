using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using ImperitWASM.Client.Data;
using ImperitWASM.Server.Services;
using ImperitWASM.Shared.Commands;
using ImperitWASM.Shared.Data;
using Microsoft.AspNetCore.Mvc;

namespace ImperitWASM.Server.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class CommandController : ControllerBase
	{
		readonly IProvinces provinces;
		readonly IPlayers players;
		readonly ISessions session;
		readonly ISettings sl;
		readonly ICommands cmd;
		public CommandController(IProvinces provinces, ISessions session, ISettings sl, ICommands cmd, IPlayers players)
		{
			this.provinces = provinces;
			this.session = session;
			this.sl = sl;
			this.cmd = cmd;
			this.players = players;
		}

		[HttpPost("GiveUp")]
		public async Task GiveUp([FromBody] Session player)
		{
			if (session.IsValid(player))
			{
				_ = await cmd.PerformAsync(player.P, new GiveUp());
			}
		}
		[HttpPost("MoveInfo")]
		public MoveInfo MoveInfo([FromBody] MoveData move)
		{
			var prov = provinces[move.G];
			return new MoveInfo(prov[move.F].CanAnyMove(prov, prov[move.T]), prov[move.F].Name, prov[move.T].Name, prov[move.F].Soldiers.ToString(), prov[move.T].Soldiers.ToString(), sl.Settings.SoldierTypes.Select(type => prov[move.F].Soldiers.CountOf(type)).ToImmutableArray());
		}
		[HttpPost("Move")]
		public async Task<MoveErrors> Move([FromBody] MoveCmd m)
		{
			var move = new Move(provinces[m.G, m.From], provinces[m.G, m.To], new Soldiers(m.Counts.Select((count, i) => new Regiment(sl.Settings.SoldierTypes[i], count))));
			return (await cmd.PerformAsync(m.P, move)) switch
			{
				(true, _, _) => MoveErrors.Ok,
				(false, var new_players, var new_provinces) =>
						!new_provinces[m.From].Has(move.Soldiers) ? MoveErrors.FewSoldiers :
						!move.HasEnoughCapacity(new_provinces) ? MoveErrors.LittleCapacity :
																	MoveErrors.Else
			};
		}
		[HttpPost("PurchaseInfo")]
		public PurchaseInfo PurchaseInfo([FromBody] PurchaseData purchase)
		{
			var player = players[purchase.P]!;
			var prov = provinces[player.GameId];
			return prov[purchase.L].Walkable ? new PurchaseInfo(new Buy(prov[purchase.L]).Allowed(player, prov), prov[purchase.L].Name, prov[purchase.L].Price, player.Money) : new PurchaseInfo(false, "", 0, 0);
		}
		[HttpPost("Purchase")]
		public async Task Purchase([FromBody] PurchaseCmd purchase)
		{
			if (session.IsValid(new Session(purchase.P, purchase.Key)))
			{
				_ = await cmd.PerformAsync(purchase.P, new Buy(provinces[purchase.G, purchase.L]));
			}
		}
		[HttpPost("RecruitInfo")]
		public RecruitInfo RecruitInfo([FromBody] RecruitData p)
		{
			var player = players[p.P]!;
			var province = provinces[player.GameId, p.W];
			return new RecruitInfo(province.Name, province.Soldiers.ToString(), sl.Settings.SoldierTypes.Select(type => type.IsRecruitable(province.Region)).ToImmutableArray(), player.Money, province.Instability);
		}
		[HttpPost("Recruit")]
		public async Task Recruit([FromBody] RecruitCmd r)
		{
			if (session.IsValid(new Session(r.P, r.Key)))
			{
				var soldiers = new Soldiers(r.Counts.Zip(sl.Settings.SoldierTypes, (count, type) => new Regiment(type, count)));
				var result = await cmd.PerformAsync(r.P, new Recruit(provinces[r.G, r.Province], soldiers));
			}
		}
		[HttpPost("Donate")]
		public async Task<bool> Donate([FromBody] DonationCmd donation) => session.IsValid(new Session(donation.P, donation.Key)) && await cmd.PerformAsync(donation.P, new Donate(players[donation.To]!, donation.M)) is (true, _, _);
	}
}