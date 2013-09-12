using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.ImageUploader.Infrastructure.Controllers;

namespace Xemio.ImageUploader.Infrastructure.Windsor.Installers
{
    /// <summary>
    /// Installs all classes inheriting from <see cref="BaseController"/>.
    /// </summary>
    public class ControllerInstaller : IWindsorInstaller
    {
        #region Implementation of IWindsorInstaller
        /// <summary>
        /// Performs the installation in the <see cref="T:Castle.Windsor.IWindsorContainer"/>.
        /// </summary>
        /// <param name="container">The container.</param><param name="store">The configuration store.</param>
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes
                                   .FromThisAssembly()
                                   .BasedOn<BaseController>()
                                   .LifestyleScoped());
        }
        #endregion
    }
}
