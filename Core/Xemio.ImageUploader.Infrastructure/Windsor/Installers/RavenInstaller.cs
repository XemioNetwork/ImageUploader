using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Xemio.ImageUploader.Entities;

namespace Xemio.ImageUploader.Infrastructure.Windsor.Installers
{
    /// <summary>
    /// Installs the <see cref="IDocumentStore"/> and the <see cref="IDocumentSession"/>.
    /// </summary>
    public class RavenInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param><param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register
                (
                    Component.For<IDocumentStore>().Instance(this.GetDocumentStore()),
                    Component.For<IAsyncDocumentSession>().UsingFactoryMethod((kernel, context) => kernel.Resolve<IDocumentStore>().OpenAsyncSession())
                );
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates the document store and all indexes.
        /// </summary>
        private IDocumentStore GetDocumentStore()
        {
            IDocumentStore documentStore = new DocumentStore
                                               {
                                                   Url = "http://localhost:8080",
                                                   DefaultDatabase = "ImageUploader"
                                               }.Initialize();

            IndexCreation.CreateIndexes(this.GetType().Assembly, documentStore);

            return documentStore;
        }
        #endregion
    }
}
