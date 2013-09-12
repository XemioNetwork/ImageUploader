using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.ImageUploader.Client.WebServices;

namespace Xemio.ImageUploader.Client.Windsor.Installers
{
    /// <summary>
    /// Installs all webservice clients.
    /// </summary>
    public class ClientInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param><param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            HttpClient httpClient = new HttpClient
                                    {
                                        BaseAddress = new Uri("http://localhost")
                                    };

            container.Register
                (
                    Component.For<UsersClient>()
                        .ImplementedBy<UsersClient>()
                        .LifestyleSingleton()
                        .DependsOn(new
                                       {
                                           client = httpClient
                                       }),
                    Component.For<UploadedImagesClient>()
                        .ImplementedBy<UploadedImagesClient>()
                        .LifestyleSingleton()
                        .DependsOn(new
                                       {
                                           client = httpClient
                                       }),
                    Component.For<Session>()
                        .ImplementedBy<Session>()
                        .LifestyleSingleton()
                );
        }

        #endregion
    }
}
