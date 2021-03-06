﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Xemio.ImageUploader.Client.Windsor.Installers
{
    /// <summary>
    /// Installs all viewmodels.
    /// </summary>
    public class ViewModelInstaller : IWindsorInstaller
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
                                   .BasedOn<PropertyChangedBase>()
                                   .WithServiceSelf()
                                   .LifestyleTransient());
        }
        #endregion
    }
}
