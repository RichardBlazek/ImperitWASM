using System.Linq;
using System.Threading.Tasks;
using ImperitWASM.Shared.Commands;

namespace ImperitWASM.Server.Services
{
	public interface IEndOfTurn
	{
		Task<bool> NextTurnAsync(int gameId, string actorName);
	}
	public class EndOfTurn : IEndOfTurn
	{
		readonly ISettings sl;
		readonly IGames gs;
		readonly IPowers powers;
		readonly ICommands commands;
		public EndOfTurn(ISettings sl, IGames gs, IPowers powers, ICommands commands)
		{
			this.sl = sl;
			this.gs = gs;
			this.powers = powers;
			this.commands = commands;
		}
		public async Task<bool> NextTurnAsync(int gameId, string actorName)
		{
			if (await commands.PerformAsync(actorName, new NextTurn()) is (true, var players, var provinces))
			{
				bool finish = !players.Any(player => player is { IsHuman: true, Alive: true }) || (provinces.Winner is ({ IsHuman: true }, int finals) && finals >= sl.Settings.FinalLandsCount);
				int turn = powers.Count(gameId);
				if (finish)
				{
					await gs.FinishAsync(gameId);
				}
				await powers.AddAsync(players.Select((player, i) => player.Power(turn + i, provinces.ControlledBy(player))));
				return finish;
			}
			return false;
		}
	}
}
