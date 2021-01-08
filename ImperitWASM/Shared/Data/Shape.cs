using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ImperitWASM.Shared.Data
{
	public sealed record Shape
	{
		public int Id { get; private set; }
		public int CenterId { get; private set; }
		public Point Center { get; private set; }
		public ICollection<Point>? Points { get; private set; }
		public ImmutableArray<Point> Border => Points!.OrderBy(p => p.Id).ToImmutableArray();
		public Shape() => (Center, Points) = (new Point(), new List<Point>());
		public Shape(Point center, IEnumerable<Point> points) => (Center, Points) = (center, points.ToList());
	}
}