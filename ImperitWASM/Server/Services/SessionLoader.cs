using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ImperitWASM.Client.Data;
using ImperitWASM.Server.Load;

namespace ImperitWASM.Server.Services
{
	public interface ISessions
	{
		Task<string> AddAsync(string player);
		bool IsValid(Session session);
		Task RemoveAsync(Session session);
	}
	public class SessionLoader : ISessions
	{
		readonly RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
		readonly ImperitContext ctx;
		public SessionLoader(ImperitContext ctx) => this.ctx = ctx;
		string GenerateToken(int len)
		{
			byte[] buf = new byte[len];
			rng.GetBytes(buf);
			return Convert.ToBase64String(buf).TrimEnd('=').Replace('+', '-').Replace('/', '_');
		}
		public async Task<string> AddAsync(string player)
		{
			string token = GenerateToken(64);
			while (ctx.Sessions!.Any(session => session.Key == token))
			{
				token = GenerateToken(64);
			}
			_ = await ctx.Sessions!.Add(new Session(player, token)).Context.SaveChangesAsync();
			return token;
		}
		public bool IsValid(Session session) => ctx.Sessions!.Contains(session);
		public Task RemoveAsync(Session session)
		{
			if (IsValid(session))
			{
				ctx.Entry(session).State = EntityState.Deleted;
				return ctx.SaveChangesAsync();
			}
			return Task.Run(() => { });
		}
	}
}
