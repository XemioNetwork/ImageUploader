using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.Facilities.TypedFactory;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Xemio.ImageUploader.Client.Windsor
{
    /// <summary>
    /// A inversion of control container.
    /// </summary>
    public class AppContainer : WindsorContainer
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AppContainer"/> class.
        /// </summary>
        public AppContainer()
        {
            this.Install(FromAssembly.This());

            this.AddFacility<TypedFactoryFacility>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Builds a instance and fills it's properties.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void BuildUp(object instance)
        {
            IEnumerable<PropertyInfo> propertyInfos = instance.GetType().GetProperties().Where(f => f.CanWrite && f.CanRead && f.GetValue(instance) == null);
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (this.Kernel.HasComponent(propertyInfo.PropertyType))
                {
                    var resolvedInstance = this.Resolve(propertyInfo.PropertyType);
                    propertyInfo.SetValue(propertyInfos, resolvedInstance);
                }
            }
        }
        #endregion
    }
}
