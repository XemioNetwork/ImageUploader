using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;

namespace Xemio.ImageUploader.Infrastructure.Windsor.Installer
{
    public class RavenInstaller : IWindsorInstaller
    {
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer" />.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register
                (
                    Component.For<IDocumentStore>().Instance(this.GetDocumentStore()).LifestyleSingleton(),
                    Component.For<IDocumentSession>().UsingFactoryMethod(this.GetDocumentSession).LifestyleScoped()
                );
        }

        #region Private Methods
        /// <summary>
        /// Creates the document store.
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
        /// <summary>
        /// Creates a new document session.
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        /// <param name="creationContext">The creation context.</param>
        private IDocumentSession GetDocumentSession(IKernel kernel, CreationContext creationContext)
        {
            return kernel.Resolve<IDocumentStore>().OpenSession();
        }
        #endregion Private Methods
    }
}
