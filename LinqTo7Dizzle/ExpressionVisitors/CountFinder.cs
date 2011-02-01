/***********************************************************
 * Credits:
 * 
 * MSDN Documentation -
 * Walkthrough: Creating an IQueryable LINQ Provider
 * 
 * http://msdn.microsoft.com/en-us/library/bb546158.aspx
 * 
 * Renamed and Documented By: Joe Mayo, 10/12/08
 * Lifted from LINQ to Twitter: GHay
 * *********************************************************/

using System;
using System.Linq.Expressions;

namespace LinqTo7Dizzle.ExpressionVisitors
{
	internal class CountFinder : ExpressionVisitor
	{
		private bool _found;

		public bool Find(Expression expression)
		{
			Visit(expression);

			return _found;
		}

		/// <summary>
		/// Custom processing of MethodCallExpression NodeType that finds a Take clause.
		/// Throws if more than one is present.
		/// </summary>
		/// <param name="expression">A MethodCallExpression node from the expression tree.</param>
		/// <returns>The expression that was passed in.</returns>
		/// <exception cref="Exception">Thrown if multiple Take clauses are present.</exception>
		protected override Expression VisitMethodCall(MethodCallExpression expression)
		{
			if (expression.Method.Name == "Count")
			{
				_found = true;
				return expression;
			}

			// look at extension source to see if there is an earlier where
			Visit(expression.Arguments[0]);

			return expression;
		}
	}
}
