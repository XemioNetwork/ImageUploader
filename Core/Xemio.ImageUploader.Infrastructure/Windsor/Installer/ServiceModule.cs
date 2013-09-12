using System;
using Amazon.S3.Model;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.ImageUploader.Core.Services;
using Xemio.ImageUploader.Infrastructure.Services;

namespace Xemio.ImageUploader.Infrastructure.Windsor.Installer
{
    public class ServiceModule : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes
                                   .FromThisAssembly()
                                   .BasedOn<IService>()
                                   .WithServiceFirstInterface()
                                   .LifestyleScoped());
        }
    }
}
