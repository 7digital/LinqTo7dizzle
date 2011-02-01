using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LinqTo7Dizzle.ExpressionVisitors;
using LinqTo7Dizzle.RequestProcessors;

namespace LinqTo7Dizzle
{
	public class QueryProvider<T> : IQueryProvider 
	{
		private readonly string _baseUrl;
		private readonly IExecutor _executor;

		public QueryProvider(string baseUrl, IExecutor executor)
		{
			_baseUrl = baseUrl;
			_executor = executor;
		}

		public IQueryable CreateQuery(Expression expression)
		{
			var elementType = TypeSystem.GetElementType(expression.Type);
			try
			{
				var genericType = typeof(Queryable<>).MakeGenericType(elementType);
				var args = new object[] { _baseUrl, _executor };
				return (IQueryable)Activator.CreateInstance(genericType, args);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
		}

		public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
		{
			return new Queryable<TResult>(_baseUrl, _executor, expression);
		}

		public object Execute(Expression expression)
		{
			return Execute<T>(expression);
		}

		public TResult Execute<TResult>(Expression expression)
		{
			var requestProcessor = GetRequestProcessor();
			var parameters = requestProcessor.GetParameters(expression);
			var url = requestProcessor.BuildUri(parameters);

			var results = _executor.Query(url, requestProcessor);
			var queryableList = requestProcessor.ProcessResults(results);

			if (typeof(TResult) == typeof(Int32))
				return (TResult)Convert.ChangeType(requestProcessor.TotalItems, typeof(TResult));

			var queryableItems = queryableList.AsQueryable();
			var treeCopier = new ExpressionTreeModifier<T>(queryableItems);
			var newExpressionTree = treeCopier.CopyAndModify(expression);

			var isEnumerable = typeof(TResult).Name == "IEnumerable`1" || typeof(TResult).Name == "IEnumerable";

			return isEnumerable
				? (TResult)queryableItems.Provider.CreateQuery(newExpressionTree)
				: (TResult)queryableItems.Provider.Execute(newExpressionTree);
		}

		private ChartRequestProcessor<T> GetRequestProcessor()
		{
			return new ChartRequestProcessor<T>(_baseUrl);
		}
	}
}