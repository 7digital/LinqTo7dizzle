using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LinqTo7Dizzle.ExpressionVisitors;

namespace LinqTo7Dizzle.Chart
{
	public class ChartQueryProvider<T> : IQueryProvider 
	{
		private readonly string _baseUrl;
		private readonly IExecutor _executor;

		public ChartQueryProvider(string baseUrl, IExecutor executor)
		{
			_baseUrl = baseUrl;
			_executor = executor;
		}

		public IQueryable CreateQuery(Expression expression)
		{
			var elementType = TypeSystem.GetElementType(expression.Type);
			try
			{
				var genericType = typeof(ChartQueryable<>).MakeGenericType(elementType);
				var args = new object[] { this, expression };
				return (IQueryable)Activator.CreateInstance(genericType, args);
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
		}

		public IQueryable<TResult> CreateQuery<TResult>(Expression expression)
		{
			return new ChartQueryable<TResult>(_baseUrl, _executor, expression);
		}

		public object Execute(Expression expression)
		{
			return Execute<T>(expression);
		}

		public TResult Execute<TResult>(Expression expression)
		{
			var isEnumerable = typeof(TResult).Name == "IEnumerable`1" || typeof(TResult).Name == "IEnumerable";

			var requestProcessor = new ChartRequestProcessor<T>(_baseUrl);
			var parameters = requestProcessor.GetParameters(expression);
			var url = requestProcessor.BuildUri(parameters);

			var results = _executor.Query(url, requestProcessor);
			var queryableList = requestProcessor.ProcessResults(results);
			var queryableItems = queryableList.AsQueryable();

			var treeCopier = new ExpressionTreeModifier<T>(queryableItems);
			var newExpressionTree = treeCopier.CopyAndModify(expression);

			return isEnumerable
				? (TResult)queryableItems.Provider.CreateQuery(newExpressionTree)
				: (TResult)queryableItems.Provider.Execute(newExpressionTree);
		}
	}
}