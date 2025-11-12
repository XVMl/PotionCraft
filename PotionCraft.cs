using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace PotionCraft
{
	// Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
	
	public enum Base
	{
		Water,
		Wine,
		Magic,
		None
	}
	public enum PotionUseSound
	{
		Item2=2,
		Item3=3,
        Item30 = 30
    }
	
	public static class TransExp<TIn, TOut>
	{
		private static readonly Func<TIn, TOut> cache = GetFunc();
		private static Func<TIn, TOut> GetFunc()
		{
			ParameterExpression parameterExpression = Expression.Parameter(typeof(TIn), "p");
			List<MemberBinding> memberBindingList = new List<MemberBinding>();

			foreach (var item in typeof(TOut).GetProperties())
			{
				if (!item.CanWrite) continue;
				MemberExpression property = Expression.Property(parameterExpression, typeof(TIn).GetProperty(item.Name));
				MemberBinding memberBinding = Expression.Bind(item, property);
				memberBindingList.Add(memberBinding);
			}

			MemberInitExpression memberInitExpression = Expression.MemberInit(Expression.New(typeof(TOut)), memberBindingList.ToArray());
			Expression<Func<TIn, TOut>> lambda = Expression.Lambda<Func<TIn, TOut>>(memberInitExpression, new ParameterExpression[] { parameterExpression });

			return lambda.Compile();
		}

		public static TOut Trans(TIn tIn)
		{
			return cache(tIn);
		}
	}

	
	public class PotionCraft : Mod
	{

	}
}
