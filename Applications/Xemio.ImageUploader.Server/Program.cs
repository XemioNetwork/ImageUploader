using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Xemio.ImageUploader.Infrastructure.Configs;

namespace Xemio.ImageUploader.Server
{
    /// <summary>
    /// Contains startup logic for the console-server.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The startup method.
        /// </summary>
        static void Main()
        {
            var config = new HttpSelfHostConfiguration("http://localhost");

            RoutingConfig.Configure(config);
            WindsorConfig.Configure(config);

            using (HttpSelfHostServer server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Xemio ImageUploader Server started at {0}...", config.BaseAddress);
                Console.WriteLine("Press <Enter> to close...");

                Console.ReadLine();
            }
        }
    }
}
