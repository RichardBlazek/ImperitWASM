using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace ImperitWASM.Shared.Value
{
	public sealed record Graph(ImmutableArray<int> Edges, ImmutableArray<int> Starts) : IReadOnlyList<IEnumerable<int>>
	{
		public int NeighborCount(int vertex) => Starts[vertex + 1] - Starts[vertex];
		public IEnumerable<int> this[int vertex] => Edges.Take(Starts[vertex + 1]).Skip(Starts[vertex]);
		public int Count => Starts.Length - 1;
		public IEnumerator<IEnumerable<int>> GetEnumerator() => Count.Range(i => this[i]).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public bool Passable(int from, int to, int limit, Func<int, int, int> difficulty)
		{
			var stack = new List<(int Pos, int Distance)>() { (from, 0) };
			bool[]? visited = new bool[Count];
			visited[from] = true;
			for (int i = 0; i < stack.Count; ++i)
			{
				if (stack[i].Pos == to)
				{
					return true;
				}
				for (int n = Starts[stack[i].Pos], stop = Starts[stack[i].Pos + 1], vertex = Edges[n]; n < stop; ++n, vertex = Edges[n])
				{
					if (!visited[n] && stack[i].Distance + difficulty(stack[i].Pos, vertex) <= limit)
					{
						stack.Add((vertex, stack[i].Distance + difficulty(stack[i].Pos, vertex)));
						visited[vertex] = true;
					}
				}
			}
			return false;
		}

		public IEnumerable<List<int>> Split(Func<int, bool> relevant, Func<int, int, bool> passable)
		{
			bool[] visited = new bool[Count];
			for (int from = 0; from < visited.Length; ++from)
			{
				if (!visited[from] && relevant(from))
				{
					visited[from] = true;
					var points = new List<int> { from };
					for (int i = 0; i < points.Count; ++i)
					{
						for (int n = Starts[points[i]], stop = Starts[points[i] + 1], vertex = Edges[n]; n < stop; ++n, vertex = Edges[n])
						{
							if (!visited[vertex] && passable(points[i], vertex))
							{
								visited[vertex] = true;
								points.Add(vertex);
							}
						}
					}
					yield return points;
				}
			}
		}
	}
}
