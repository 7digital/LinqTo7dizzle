using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace LinqTo7Dizzle.Chart
{
	public class ChartQueryable<T> : IOrderedQueryable<T>
	{
		private readonly IQueryProvider _provider;

		public ChartQueryable(string baseUrl, IExecutor executor)
		{
			Expression = Expression.Constant(this);
			_provider = new ChartQueryProvider<T>(baseUrl, executor);
		}

		public ChartQueryable(string baseUrl, IExecutor executor, Expression expression)
		{
			Expression = expression;
			_provider = new ChartQueryProvider<T>(baseUrl, executor);
		}

		IEnumerator<T> IEnumerable<T>.GetEnumerator()
		{
			return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
		}

		public IEnumerator GetEnumerator()
		{
			return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
		}

		public Expression Expression { get; private set; }

		public Type ElementType { get { return typeof (T); } }

		public IQueryProvider Provider
		{
			get { return _provider; }
		}
	}
}