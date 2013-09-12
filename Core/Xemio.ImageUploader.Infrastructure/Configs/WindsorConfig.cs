using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Web.Http;
using Castle.Facilities.Logging;
using Castle.Services.Logging.NLogIntegration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using NLog.Config;
using NLog.Targets;
using Xemio.ImageUploader.Infrastructure.Windsor;

namespace Xemio.ImageUploader.Infrastructure.Configs
{
    /// <summary>
    /// Configures the windsor container.
    /// </summary>
    public static class WindsorConfig
    {
        /// <summary>
        /// Configures the specified configuration to use a <see cref="WindsorDependencyResolver"/>
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(HttpConfiguration configuration)
        {
            var windsorContainer = new WindsorContainer();
            
            windsorContainer.Install(FromAssembly.This());
            windsorContainer.AddFacility<LoggingFacility>(f => f.LogUsing<NLogFactory>().WithConfig("NLog.config"));

            configuration.DependencyResolver = new WindsorDependencyResolver(windsorContainer);
        }
    }
}
