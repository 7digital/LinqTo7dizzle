using System;
using System.Configuration;
using System.Linq;
using LinqTo7Dizzle;
using LinqTo7Dizzle.Entities;

namespace LinqTo7DizzleDemo
{
    class Program
    {
        static void Main()
        {
            var consumerKey = ConfigurationManager.AppSettings["consumerKey"];
            var consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];

            if (string.IsNullOrEmpty(consumerKey) || string.IsNullOrEmpty(consumerSecret))
            {
                Console.WriteLine("Please set the consumer key and secret values in the app.config file and run again.");
                Console.WriteLine("If you don't have keys yet, you can visit http://access.7digital.com/partnerprogram and register.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            const string baseUrl = "http://api.7digital.com/1.2/";

			using (var context = new SevenDizzleContext(baseUrl))
			{
				var releases = context.Chart<Release>().Count();
			}
        }
    }
}
