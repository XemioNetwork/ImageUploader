using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.MicroKernel.Lifestyle;
using Castle.Windsor;

namespace Xemio.ImageUploader.Infrastructure.Windsor
{
    public class WindsorDependencyScope : IDependencyScope
    {
        #region Fields
        protected readonly IWindsorContainer _windsorContainer;
        private readonly IDisposable _scope;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="WindsorDependencyScope"/> class.
        /// </summary>
        /// <param name="windsorContainer">The windsor container.</param>
        public WindsorDependencyScope(IWindsorContainer windsorContainer)
        {
            this._windsorContainer = windsorContainer;
            this._scope = this._windsorContainer.BeginScope();
        }
        #endregion Constructors

        #region IDisposable Member
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this._scope.Dispose();
        }
        #endregion IDisposable Member

        #region IDependencyScope Member
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
        #endregion IDependencyScope Member
    }
}