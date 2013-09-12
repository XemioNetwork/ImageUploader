using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using AttributeRouting.Web.Http.SelfHost;
using Xemio.ImageUploader.Infrastructure.Controllers;

namespace Xemio.ImageUploader.Infrastructure.Configs
{
    /// <summary>
    /// Configures the routes for web api.
    /// </summary>
    public static class RoutingConfig
    {
        /// <summary>
        /// Configures the specified configuration to use attribute-routing.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(HttpConfiguration configuration)
        {
            configuration.Routes.MapHttpAttributeRoutes(f => f.AddRoutesFromAssemblyOf<BaseController>());
        }
    }
}
