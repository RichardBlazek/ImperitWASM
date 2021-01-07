using System;
using Newtonsoft.Json;

namespace ImperitWASM.Shared.Value
{
	public class RatioConvertorNewtonsoft : JsonConverter<Ratio>
	{
		public override Ratio ReadJson(JsonReader reader, Type objectType, Ratio existingValue, bool hasExistingValue, JsonSerializer serializer) => new Ratio(serializer.Deserialize<int>(reader));
		public override void WriteJson(JsonWriter writer, Ratio value, JsonSerializer serializer) => serializer.Serialize(writer, value.ToInt());
	}
}
