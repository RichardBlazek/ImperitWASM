using System.Threading.Tasks;
using ImperitWASM.Server.Load;

namespace ImperitWASM.Server.Services
{
	public interface IChangeSaver
	{
		Task SaveAsync();
	}
	public class ChangeSaver : IChangeSaver
	{
		readonly ImperitContext ctx;
		public ChangeSaver(ImperitContext ctx) => this.ctx = ctx;
		public Task SaveAsync() => ctx.SaveChangesAsync();
	}
}
