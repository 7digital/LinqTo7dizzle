using System;
using System.IO;
using System.Net;
using System.Threading;

namespace LinqTo7Dizzle
{
	internal class SevenDizzleExecutor : IExecutor
	{
		private Exception _asyncException;
		private string _responseXml;
		private ManualResetEvent _resetEvent;

		public string Query<T>(Uri uri, IRequestProcessor<T> requestProcessor)
		{
			var request = WebRequest.Create(uri);

			_asyncException = null;
			_responseXml = string.Empty;

			_resetEvent = new ManualResetEvent(false);

			request.BeginGetResponse(ar => OnResponse(request, ar), null);

			_resetEvent.WaitOne();

			if (_asyncException != null)
			{
				throw _asyncException;
			}

			return _responseXml;
		}

		private void OnResponse(WebRequest request, IAsyncResult asyncResult)
		{
			try
			{
				var response = request.EndGetResponse(asyncResult) as HttpWebResponse;
				_responseXml = GetResponse(response);
			}
			catch (Exception ex)
			{
				_asyncException = ex;
			}
			finally
			{
				_resetEvent.Set();
			}
		}

		private static string GetResponse(WebResponse response)
		{
			using (var responseStream = response.GetResponseStream())
			using (var responseReader = new StreamReader(responseStream))
			{
				return responseReader.ReadToEnd();
			}
		}
	}
}