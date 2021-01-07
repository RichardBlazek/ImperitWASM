using System.Text.Json.Serialization;

namespace ImperitWASM.Client.Data
{
	public sealed record Session
	{
		[JsonInclude] public string P { get; private set; }
		[JsonInclude] public string Key { get; private set; }
		public bool IsSet() => Key is { Length: > 0 };
		public Session(string p, string key) => (P, Key) = (p, key);
		public Session() : this("", "") { }
	}
}
