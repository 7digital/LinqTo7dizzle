/***********************************************************
 * Credits:
 * 
 * MSDN Documentation -
 * Walkthrough: Creating an IQueryable LINQ Provider
 * 
 * http://msdn.microsoft.com/en-us/library/bb546158.aspx
 * 
 * Renamed and Documented By: Joe Mayo, 10/12/08
 * *********************************************************/

using System;
using System.Linq.Expressions;

namespace LinqTo7Dizzle.ExpressionVisitors
{
	internal class TakeFinder : ExpressionVisitor
	{
		/// <summary>
		/// holds first where expression when found
		/// </summary>
		private MethodCallExpression _expression;

		/// <summary>
		/// initiates search for first where clause
		/// </summary>
		/// <param name="expression">expression tree to search</param>
		/// <returns>MethodCallExpression for first where clause</returns>
		public int Find(Expression expression)
		{
			Visit(expression);

			return expression != null
			       	? (int) ((ConstantExpression) _expression.Arguments[1]).Value
			       	: 10;
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
			if (expression.Method.Name == "Take")
			{
				if (_expression != null)
					throw new Exception("Take 2!!1!");

				_expression = expression;
			}

			// look at extension source to see if there is an earlier where
			Visit(expression.Arguments[0]);

			return expression;
		}
	}
}
