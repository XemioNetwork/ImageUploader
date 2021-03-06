﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace Xemio.ImageUploader.Infrastructure.Windsor
{
    public class WindsorDependencyResolver : IDependencyResolver
    {
        #region Fields
        private readonly IWindsorContainer _windsorContainer;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorDependencyResolver"/> class.
        /// </summary>
        /// <param name="windsorContainer">The windsor container.</param>
        public WindsorDependencyResolver(IWindsorContainer windsorContainer)
        {
            this._windsorContainer = windsorContainer;
        }
        #endregion Constructors
        
        #region IDependencyResolver Member
        /// <summary>
        /// Starts a resolution scope.
        /// </summary>
        public IDependencyScope BeginScope()
        {
            return new WindsorDependencyScope(this._windsorContainer);
        }
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this._windsorContainer.Dispose();
        }
        /// <summary>
        /// Retrieves a service from the scope.
        /// </summary>
        /// <param name="serviceType">The service to be retrieved.</param>
        public object GetService(Type serviceType)
        {
            if (this._windsorContainer.Kernel.HasComponent(serviceType) == false)
                return null;

            return this._windsorContainer.Resolve(serviceType);
        }
        /// <summary>
        /// Retrieves a collection of services from the scope.
        /// </summary>
        /// <param name="serviceType">The collection of services to be retrieved.</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (this._windsorContainer.Kernel.HasComponent(serviceType) == false)
                return new List<object>();

            return this._windsorContainer.ResolveAll(serviceType).Cast<object>();
        }
        #endregion IDependencyResolver Member
    }
}