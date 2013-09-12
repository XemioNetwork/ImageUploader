using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using Xemio.ImageUploader.Client.UserInterface.Views.Login;
using Xemio.ImageUploader.Client.UserInterface.Views.Shell;
using Xemio.ImageUploader.Client.Windsor;
using Xemio.ImageUploader.Entities;

namespace Xemio.ImageUploader.Client
{
    /// <summary>
    /// Contains basic logic for setting up the client.
    /// </summary>
    public class AppBootstrapper : BootstrapperBase
    {
        #region Fields
        private readonly AppContainer _container = new AppContainer();
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="AppBootstrapper"/> class.
        /// </summary>
        public AppBootstrapper()
        {
            this.Start();
        }
        #endregion

        #region BootstrapperBase Methods
        /// <summary>
        /// Override this to add custom behavior to execute after the application starts.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The args.</param>
        protected override void OnStartup(object sender, System.Windows.StartupEventArgs e)
        {
            bool? loggedIn = this.ShowLoginWindow();

            if (loggedIn.HasValue && loggedIn.Value)
            {
                this.ShowShellWindow();
            }
            else
            {
                Application.Shutdown();
            }
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="service">The service to locate.</param>
        /// <param name="key">The key to locate.</param>
        protected override object GetInstance(Type service, string key)
        {
            if (this._container.Kernel.HasComponent(service) == false)
            {
                return base.GetInstance(service, key);
            }

            return this._container.Resolve(service);
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation
        /// </summary>
        /// <param name="service">The service to locate.</param>
        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            if (this._container.Kernel.HasComponent(service) == false)
            {
                return base.GetAllInstances(service);
            }

            return this._container.ResolveAll(service).Cast<object>();
        }
        /// <summary>
        /// Override this to provide an IoC specific implementation.
        /// </summary>
        /// <param name="instance">The instance to perform injection on.</param>
        protected override void BuildUp(object instance)
        {
            this._container.BuildUp(instance);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Shows the login window with the given viewmodel as data context.
        /// </summary>
        private bool? ShowLoginWindow()
        {
            var loginViewModel = this._container.Resolve<LoginViewModel>();

            IWindowManager windowManager = this._container.Resolve<IWindowManager>();

            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            Application.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            bool? loggedIn = windowManager.ShowDialog(loginViewModel, null, settings);
            Application.ShutdownMode = ShutdownMode.OnLastWindowClose;

            return loggedIn;
        }
        /// <summary>
        /// Shows the shell with the given user user.
        /// </summary>
        private void ShowShellWindow()
        {
            IWindowManager windowManager = this._container.Resolve<IWindowManager>();

            var shellViewModel = this._container.Resolve<ShellViewModel>();
            windowManager.ShowWindow(shellViewModel);
        }
        #endregion Private Methods
    }
}
