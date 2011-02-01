using System;
using System.IO;
using System.Net;
using System.Threading;

namespace LinqTo7Dizzle
{
	internal class SevenDizzleExecutor : IExecutor
	{
		public string Query<T>(Uri uri, IRequestProcessor<T> requestProcessor)
		{
			var request = WebRequest.Create(uri);

			Exception asyncException = null;
			var responseXml = string.Empty;

			var resetEvent = new ManualResetEvent(false);

			request.BeginGetResponse(ar =>
			{
				try
				{
					var response = request.EndGetResponse(ar) as HttpWebResponse;
					responseXml = GetResponse(response);
				}
				catch (Exception ex)
				{
					asyncException = ex;
				}
				finally
				{
					resetEvent.Set();
				}
			}, null);

			resetEvent.WaitOne();

			if (asyncException != null)
			{
				throw asyncException;
			}

			return responseXml;
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