using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Xemio.ImageUploader.Client.UserInterface.Caliburn;

namespace Xemio.ImageUploader.Client.Windsor.Installers
{
    public class CaliburnInstaller : IWindsorInstaller
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
                    Component.For<IWindowManager>().Instance(new XemioWindowManager()),
                    Component.For<IEventAggregator>().Instance(new EventAggregator())
                );
        }
        #endregion
    }
}
