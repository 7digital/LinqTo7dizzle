using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using LinqTo7Dizzle.Entities;
using LinqTo7Dizzle.ExpressionVisitors;

namespace LinqTo7Dizzle.RequestProcessors
{
	internal class ChartRequestProcessor<T> : IRequestProcessor<T>
	{
		private readonly string _baseUrl;

		public int Page { get; private set; }
		public int PageSize { get; private set; }
		public int TotalItems { get; private set; }

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
			var chart = response.Element("chart");
			
			Page = Convert.ToInt32(chart.Element("page").Value);
			PageSize = Convert.ToInt32(chart.Element("pageSize").Value);
			TotalItems = Convert.ToInt32(chart.Element("totalItems").Value);

			var chartItems = chart.Elements("chartItem");

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

			if (new CountFinder().Find(expression))
			{
				parameters.Add("pageSize", "1");
			}
			else
			{
				var pageSize = new TakeFinder().Find(expression);
				parameters.Add("pageSize", pageSize.ToString());
			}

			return parameters;
		}
	}
}
