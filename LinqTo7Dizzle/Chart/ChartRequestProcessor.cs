using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using LinqTo7Dizzle.Entities;
using LinqTo7Dizzle.ExpressionVisitors;

namespace LinqTo7Dizzle.Chart
{
	internal class ChartRequestProcessor<T> : IRequestProcessor<T>
	{
		private readonly string _baseUrl;

		public ChartRequestProcessor(string baseUrl)
		{
			_baseUrl = baseUrl;
		}

		public Uri BuildUri(Dictionary<string, string> parameters)
		{
			var queryString = string.Join("&", parameters.Select(p => string.Format("{0}={1}", p.Key, p.Value)).ToArray());
			var uriBuilder = new UriBuilder(_baseUrl + "release/chart")
			{
				Query = queryString
			};
			return uriBuilder.Uri;
		}

		public IEnumerable<T> ProcessResults(string results)
		{
			var response = XElement.Parse(results);
			var chartItems = response.Element("chart").Elements("chartItem");

			switch (typeof(T).Name)
			{
				case "Release":
					return chartItems.Select(Release.CreateFromChartItem).Cast<T>();
				default:
					throw new Exception("Unknown type");
			}
		}

		public Dictionary<string, string> GetParameters(Expression expression)
		{
			var parameters = new Dictionary<string, string>();

			parameters.Add("oauth_consumer_key", "test-api");

			var pageSize = new TakeFinder().Find(expression);
			parameters.Add("pageSize", pageSize.ToString());

			return parameters;
		}
	}
}
