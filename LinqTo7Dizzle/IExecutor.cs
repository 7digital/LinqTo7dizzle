using System;

namespace LinqTo7Dizzle
{
	public interface IExecutor
    {
    	string Query<T>(Uri uri, IRequestProcessor<T> requestProcessor);
    }
}