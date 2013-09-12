using System;
using System.Dynamic;
using System.Net;
using System.Net.Http;
using System.Windows;
using Caliburn.Micro;
using Xemio.ImageUploader.Client.Extensions;
using Xemio.ImageUploader.Client.UserInterface.Views.About;
using Xemio.ImageUploader.Client.UserInterface.Views.Register;
using Xemio.ImageUploader.Client.WebServices;
using Xemio.ImageUploader.Entities;

namespace Xemio.ImageUploader.Client.UserInterface.Views.Login
{
    public class LoginViewModel : Screen
    {
        #region Fields
        private readonly IWindowManager _windowManager;
        private readonly UsersClient _usersClient;
        private readonly Session _session;
        private readonly Func<RegisterViewModel> _registerViewModelFactory;
        private readonly Func<AboutViewModel> _aboutViewModelFactory;

        private string _username;
        private string _password;
        #endregion
        
        #region Properties
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username
        {
            get { return this._username; }
            set
            {
                this._username = value;
                this.NotifyOfPropertyChange(() => this.Username);
                this.NotifyOfPropertyChange(() => this.CanLogin);
            }
        }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password
        {
            get { return this._password; }
            set
            {
                this._password = value;
                this.NotifyOfPropertyChange(() => this.Password);
                this.NotifyOfPropertyChange(() => this.CanLogin);
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginViewModel" /> class.
        /// </summary>
        /// <param name="windowManager">The window manager.</param>
        /// <param name="usersClient">The users client.</param>
        /// <param name="session">The session.</param>
        /// <param name="registerViewModelFactory">The register viewmodel.</param>
        /// <param name="aboutViewModelFactory">The about viewmodel.</param>
        public LoginViewModel(IWindowManager windowManager, UsersClient usersClient, Session session, Func<RegisterViewModel> registerViewModelFactory, Func<AboutViewModel> aboutViewModelFactory)
        {
            this._windowManager = windowManager;
            this._usersClient = usersClient;
            this._session = session;
            this._registerViewModelFactory = registerViewModelFactory;
            this._aboutViewModelFactory = aboutViewModelFactory;
        }
        #endregion

        #region IScreen Member
        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            this.DisplayName = "Xemio Share - Login";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a value indicating whether the "Login" method is executable.
        /// </summary>
        public bool CanLogin
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Username) == false &&
                       string.IsNullOrWhiteSpace(this.Password) == false;
            }
        }
        /// <summary>
        /// Logins the current user.
        /// </summary>
        public async void Login()
        {
            this._session.Username = this.Username;
            this._session.Password = this.Password;

            HttpResponseMessage response = await this._usersClient.GetCurrentUserAsync();
            if (response.StatusCode == HttpStatusCode.Found)
            {
                this._session.CurrentUser = await response.Content.ReadAsAsync<User>();
                this.TryClose(true);
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                MessageBox.Show(message);
            }
        }
        /// <summary>
        /// Shows the register view.
        /// </summary>
        public void Register()
        {
            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            this._windowManager.ShowDialog(this._registerViewModelFactory(), null, settings);
        }
        /// <summary>
        /// Shows the about view.
        /// </summary>
        public void About()
        {
            dynamic settings = new ExpandoObject();
            settings.ResizeMode = ResizeMode.CanMinimize;

            this._windowManager.ShowDialog(this._aboutViewModelFactory(), null, settings);
        }
        #endregion
    }
}
