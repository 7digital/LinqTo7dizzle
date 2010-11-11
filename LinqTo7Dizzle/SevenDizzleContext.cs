using System;
using LinqTo7Dizzle.OAuth;

namespace LinqTo7Dizzle
{
    public class SevenDizzleContext : IDisposable
    {
        private readonly IExecute _execute;
        private readonly string _baseUrl;

        public SevenDizzleContext(IAuthorize authorize, string baseUrl)
        {
            _execute = new SevenDizzleExecute(authorize);
            _baseUrl = baseUrl;
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
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                //var disposableExecutor = this.TwitterExecutor as IDisposable;
                //if (disposableExecutor != null)
                //{
                //    disposableExecutor.Dispose();
                //}
            }
        }
    }
}
