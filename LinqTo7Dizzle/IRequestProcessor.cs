using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqTo7Dizzle
{
	public interface IRequestProcessor<T>
	{
		int Page { get; }
		int PageSize { get; }
		int TotalItems { get; }

		Uri BuildUri(Dictionary<string, string> parameters);
		IEnumerable<T> ProcessResults(string results);
		Dictionary<string, string> GetParameters(Expression expression);
	}
}