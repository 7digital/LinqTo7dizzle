/***********************************************************
 * Credits:
 * 
 * MSDN Documentation -
 * Walkthrough: Creating an IQueryable LINQ Provider
 * 
 * http://msdn.microsoft.com/en-us/library/bb546158.aspx
 * 
 * Matt Warren's Blog -
 * LINQ: Building an IQueryable Provider:
 * 
 * http://blogs.msdn.com/mattwar/default.aspx
 * *********************************************************/

using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqTo7Dizzle
{
	internal static class TypeSystem
	{
		internal static Type GetElementType(Type seqType)
		{
			var ienum = FindIEnumerable(seqType);
			return ienum == null
				? seqType
				: ienum.GetGenericArguments()[0];
		}

		private static Type FindIEnumerable(Type seqType)
		{
			if (seqType == null || seqType == typeof(string))
				return null;

			if (seqType.IsArray)
				return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());

			if (seqType.IsGenericType)
			{
				var type = seqType.GetGenericArguments()
					.Select(arg => typeof(IEnumerable<>).MakeGenericType(arg))
					.FirstOrDefault(ienum => ienum.IsAssignableFrom(seqType));

				if (type != null)
				{
					return type;
				}
			}

			var interfaces = seqType.GetInterfaces();
			if (interfaces.Length > 0)
			{
				var type = interfaces.Select(FindIEnumerable)
					.FirstOrDefault(ienum => ienum != null);

				if (type != null)
				{
					return type;
				}
			}

			if (seqType.BaseType != null && seqType.BaseType != typeof(object))
			{
				return FindIEnumerable(seqType.BaseType);
			}

			return null;
		}
	}
}
