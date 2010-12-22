using System.Linq;
using System.Linq.Expressions;

namespace LinqTo7Dizzle.ExpressionVisitors
{
	internal class ExpressionTreeModifier<T> : ExpressionVisitor
	{
		private readonly IQueryable<T> _queryableItems;

		internal ExpressionTreeModifier(IQueryable<T> items)
		{
			_queryableItems = items;
		}

		internal Expression CopyAndModify(Expression expression)
		{
			return Visit(expression);
		}

		protected override Expression VisitConstant(ConstantExpression expression)
		{
			return expression.Type.Name == "ChartQueryable`1" 
				? Expression.Constant(_queryableItems) 
				: expression;
		}
	}
}
