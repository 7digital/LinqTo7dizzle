using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace LinqTo7Dizzle
{
	public class TagsRequestProcessor<T> : IRequestProcessor<T>
	{
		private readonly string _baseUrl;

		public int Page { get; private set; }
		public int PageSize { get; private set; }
		public int TotalItems { get; private set; }

		public TagsRequestProcessor(string baseUrl)
		{
			_baseUrl = baseUrl;
		}

		public Uri BuildUri(Dictionary<string, string> parameters)
		{
			return null;
		}

		public IEnumerable<T> ProcessResults(string results)
		{
			yield break;
		}

		public Dictionary<string, string> GetParameters(Expression expression)
		{
			return null;
		}
	}
}