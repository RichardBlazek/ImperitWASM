using System.Threading.Tasks;
using Blazored.SessionStorage;

namespace ImperitWASM.Client.Services
{
	public class SessionStorage
	{
		readonly ISessionStorageService iss;
		public Data.Session Session { get; private set; } = new Data.Session();
		public int? PlayerOrder { get; private set; } = null;
		public long? GameId { get; private set; } = null;
		public SessionStorage(ISessionStorageService iss) => this.iss = iss;
		public async Task LoadAsync() => Session = await iss.GetItemAsync<Data.Session?>("session") ?? new Data.Session();
		public Task LoginAsync(Data.LoginResult login)
		{
			(PlayerOrder, GameId) = login.S.IsSet() ? (login.I, login.G) : (null as int?, null as long?);
			return ResetAsync(login.S);
		}
		public Task ResetAsync(Data.Session? session = null)
		{
			if (session is null || !session.IsSet())
			{
				Session = new Data.Session();
				return iss.RemoveItemAsync("session");
			}
			Session = session;
			return iss.SetItemAsync("session", session);
		}
		public string Key => Session.Key;
		public string Name => Session.P;
		public bool IsSet => Session.IsSet();
	}
}
