using System;
using System.Collections.Generic;
using System.Linq;

namespace ImperitWASM.Shared
{
	public static class ListExtensions
	{
		static readonly Random rand = new Random();
		public static IEnumerable<int> Indices<T>(this IEnumerable<T> en, Func<T, bool> pred) => en.Select((v, i) => (v, i)).Where(x => pred(x.v)).Select(x => x.i);
		public static T Must<T>(this T? value) where T : class => value ?? throw new ArgumentNullException(typeof(T).FullName);
		public static T FirstOr<T>(this IEnumerable<T> e, T x) => e.DefaultIfEmpty(x).First();
		public static IEnumerable<(int i, T v)> Index<T>(this IEnumerable<T> e) => e.Select((v, i) => (i, v));
		public static T? MinBy<T, TC>(this IEnumerable<T> e, Func<T, TC> selector, T? v = default) where T : class => e.OrderBy(selector).FirstOr(v);
		public static (List<A>, P) Fold<A, P>(this IEnumerable<A> e, P init, Func<P, A, (P, A?)> fn) where A : class
		{
			var result = new List<A>();
			foreach (var item in e)
			{
				var (next_init, added) = fn(init, item);
				init = next_init;
				if (added is not null)
				{
					result.Add(added);
				}
			}
			return (result, init);
		}
		public static IEnumerable<T> Replace<T, TC>(this IEnumerable<T> e, Func<TC, bool> cond, Func<TC, TC, TC> interact, TC init) where T : class where TC : T
		{
			var selected = e.OfType<TC>().Where(cond);
			var remaining = e.Where(x => x is not TC y || !cond(y));
			return remaining.Append(selected.Aggregate(init, interact));
		}
		public static void Each<T>(this IEnumerable<T> e, Action<T> action)
		{
			foreach (var item in e)
			{
				action(item);
			}
		}
		public static void Each<T>(this IEnumerable<T> e, Action<T, int> action)
		{
			int i = 0;
			foreach (var item in e)
			{
				action(item, i);
				++i;
			}
		}
		public static List<T> Combine<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T, T, bool> eq, Func<T, T, T> match)
		{
			var result = new List<T>(first);
			foreach (var t2 in second)
			{
				int i = result.FindIndex(t1 => eq(t1, t2));
				if (i != -1)
				{
					result[i] = match(result[i], t2);
				}
				else
				{
					result.Add(t2);
				}
			}
			return result;
		}
		public static IEnumerable<T> Range<T>(this int count, Func<int, T> selector) => Enumerable.Range(0, count).Select(selector);
		public static void Shuffle<T>(this IList<T> list)
		{
			for (int i = 0, len = list.Count; i < len - 1; ++i)
			{
				int r = i + rand.Next(len - i);
				var tmp = list[r];
				list[r] = list[i];
				list[i] = tmp;
			}
		}
		public static List<T> Shuffled<T>(this IEnumerable<T> e)
		{
			var list = e.ToList();
			list.Shuffle();
			return list;
		}
		public static IEnumerable<TR> SelectAccumulate<T, TA, TR>(this IEnumerable<T> e, TA accumulator, Func<T, TA, (TR, TA)> selector)
		{
			foreach (var item in e)
			{
				var (new_item, new_accumulator) = selector(item, accumulator);
				accumulator = new_accumulator;
				yield return new_item;
			}
		}
	}
}