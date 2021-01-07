using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ImperitWASM.Shared.Value;

namespace ImperitWASM.Shared.Data
{
	public record Provinces(ImmutableArray<Province> Items, Graph Graph) : IReadOnlyList<Province>
	{
		public Provinces With(ImmutableArray<Province> new_items) => this with { Items = new_items };
		public Provinces With(IEnumerable<Province> new_items) => With(new_items.OrderBy(p => p.RegionId).ToImmutableArray());
		public int NeighborCount(Province p) => Graph.NeighborCount(p.RegionId);
		public IEnumerable<int> NeighborIndices(Province p) => Graph[p.RegionId];
		public IEnumerable<Province> NeighborsOf(Province p) => Graph[p.RegionId].Select(vertex => Items[vertex]);

		public IEnumerable<Province> ControlledBy(Player player) => Items.Where(p => p.IsAllyOf(player));
		public bool HasNeighborRuledBy(Province province, Player player) => NeighborsOf(province).Any(p => p.IsAllyOf(player));
		public bool HasAny(Player player) => ControlledBy(player).Any();

		public Province this[int key] => Items[key];
		public int Count => Items.Length;
		public IEnumerator<Province> GetEnumerator() => (Items as IEnumerable<Province>).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		public ImmutableArray<Province>.Builder ToBuilder() => Items.ToBuilder();

		public bool Passable(Province from, Province to, int distance, Func<Province, Province, int> difficulty) => Graph.Passable(from.RegionId, to.RegionId, distance, (start, dest) => difficulty(this[start], this[dest]));
		IEnumerable<IEnumerable<Province>> Split(Func<Province, bool> relevant, Func<Province, Province, bool> passable) => Graph.Split(i => relevant(this[i]), (from, to) => passable(this[from], this[to])).Select(list => list.Select(i => this[i]));
		public int IncomeOf(Player player) => Split(p => p.IsAllyOf(player), (from, to) => to.IsAllyOf(player)).DefaultIfEmpty().Max(prov => prov?.OfType<Land>()?.Sum(p => p.Earnings) ?? 0);

		public IReadOnlyList<int> Inhabitable => Items.Indices(province => province.Inhabitable).Shuffled();
		public (Human?, int) Winner => Items.GroupBy(province => province.Player).Where(g => g.Key is Human).Select(g => (g.Key, g.Sum(province => province.Score))).OrderBy(p => p.Item2).FirstOrDefault() is (Human human, int finals) ? (human, finals) : (null, 0);
	}
}