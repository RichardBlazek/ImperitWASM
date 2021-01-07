using System.Linq;
using System.Threading.Tasks;
using ImperitWASM.Shared.Commands;
using ImperitWASM.Shared.Data;

namespace ImperitWASM.Server.Services
{
	public interface IEndOfTurn
	{
		Task<bool> NextTurnAsync(long gameId, string actorName);
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
		public async Task<bool> NextTurnAsync(long gameId, string actorName)
		{
			if (await commands.PerformAsync(actorName, new NextTurn()) is (true, var players, var provinces))
			{
				bool finish = !players.Any(player => player is Human { Alive: true }) || (provinces.Winner is (Human, int finals) && finals >= sl.Settings.FinalLandsCount);
				int turn = powers.Count(gameId);
				_ = gs.Update(gameId, game => finish ? game.Finish() : game);
				await powers.AddAsync(players.Select(player => player.Power(turn, provinces.ControlledBy(player))));
				return finish;
			}
			return false;
		}
	}
}
