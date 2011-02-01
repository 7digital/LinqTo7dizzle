using System;
using LinqTo7Dizzle.Entities;

namespace LinqTo7Dizzle
{
	public class SevenDizzleContext : IDisposable
	{
		private readonly IExecutor _executor;
		private readonly string _baseUrl;

		public SevenDizzleContext(string baseUrl)
		{
			_baseUrl = baseUrl;
			_executor = new SevenDizzleExecutor();
		}

		public Queryable<T> Chart<T>() where T : Entity
		{
			return new Queryable<T>(_baseUrl, _executor);
		}

		public Queryable<Tag> Tags()
		{
			return new Queryable<Tag>(_baseUrl, _executor);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		private void Dispose(bool disposing)
		{
			if (!disposing)
				return;

			//var disposableExecutor = _executor as IDisposable;
			//if (disposableExecutor != null)
			//{
			//    disposableExecutor.Dispose();
			//}
		}
	}
}
