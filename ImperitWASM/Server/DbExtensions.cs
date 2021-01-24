using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ImperitWASM.Server
{
	public static class DbExtensions
	{
		public static ReferenceReferenceBuilder<TP, TD> Required<TP, TD>(this ReferenceReferenceBuilder<TP, TD> builder) where TP : class where TD : class
		{
			return builder.IsRequired().OnDelete(DeleteBehavior.Cascade);
		}
		public static ReferenceCollectionBuilder<TP, TD> Required<TP, TD>(this ReferenceCollectionBuilder<TP, TD> builder) where TP : class where TD : class
		{
			return builder.IsRequired().OnDelete(DeleteBehavior.Cascade);
		}
		public static ReferenceCollectionBuilder<TP?, TD> Cascade<TP, TD>(this ReferenceCollectionBuilder<TP?, TD> builder) where TP : class where TD : class
		{
			return builder.OnDelete(DeleteBehavior.Cascade);
		}
		public static PropertyBuilder<object> HasCustomKey<T>(this EntityTypeBuilder<T> builder, Expression<Func<T, object>> property) where T : class
		{
			_ = builder.HasKey(property);
			return builder.Property(property).ValueGeneratedNever();
		}
		public static void ReplaceRange<T>(this DbSet<T> set, IEnumerable<T> entities) where T : class
		{
			set.RemoveRange(entities);
			set.AddRange(entities);
		}
	}
}