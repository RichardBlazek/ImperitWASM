using System.IO;
using ImperitWASM.Shared.Value;
using Newtonsoft.Json;

namespace ImperitWASM.Server.Services
{
	public static class GraphLoader
	{
		static T Load<T>(string[] parts) => JsonConvert.DeserializeObject<T>(File.ReadAllText(Path.Combine(parts)));
		public static Graph Graph(params string[] parts) => Load<Graph>(parts);
	}
}
