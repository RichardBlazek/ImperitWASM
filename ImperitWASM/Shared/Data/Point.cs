using System.Text.Json.Serialization;

namespace ImperitWASM.Shared.Data
{
	public sealed record Point
	{
		[JsonIgnore] public int Id { get; private set; }
		[JsonInclude] public double X { get; private set; }
		[JsonInclude] public double Y { get; private set; }
		public Point(double x = 0.0, double y = 0.0)
		{
			X = x;
			Y = y;
		}
	}
}
