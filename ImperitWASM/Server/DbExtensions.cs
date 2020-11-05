using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ImperitWASM.Shared;
using System.Linq;
using ImperitWASM.Server.Load;

namespace ImperitWASM.Server
{
	public static class DbExtensions
	{
		public static void UpdateAt<T>(this DbSet<T> set, int id, Action<T> mod) where T : class, IEntity, new() => mod(set.Attach(new T { Id = id }).Entity);
		public static void UpdateAt<T>(this DbSet<T> set, Func<T, bool> cond, Action<T> mod) where T : class => set.Where(cond).ToArray().Each(mod);
		public static void RemoveAt<T>(this DbSet<T> set, int id) where T : class, IEntity, new() => set.Remove(set.Attach(new T { Id = id }).Entity);
		public static void RemoveAt<T>(this DbSet<T> set, Func<T, bool> cond) where T : class => set.RemoveRange(set.Where(cond).ToArray());
		public static T FirstIfOrFirst<T>(this IEnumerable<T> e, Func<T, bool> cond) => e.Where(cond).DefaultIfEmpty(e.FirstOrDefault()).First();
	}
}