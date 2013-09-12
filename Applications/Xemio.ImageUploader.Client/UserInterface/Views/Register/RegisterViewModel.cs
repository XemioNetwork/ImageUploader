using System.Net;
using System.Net.Http;
using System.Windows;
using Caliburn.Micro;
using Xemio.ImageUploader.Client.WebServices;
using Xemio.ImageUploader.Entities;
using Xemio.ImageUploader.Models;

namespace Xemio.ImageUploader.Client.UserInterface.Views.Register
{
    public class RegisterViewModel : Screen
    {
        #region Fields
        private readonly UsersClient _usersClient;
        private string _username;
        private string _password;
        private string _emailAddress;
        #endregion Fields

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
                this.NotifyOfPropertyChange(() => this.CanRegister);
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
                this.NotifyOfPropertyChange(() => this.CanRegister);
            }
        }
        /// <summary>
        /// Gets or sets the email address.
        /// </summary>
        public string EmailAddress
        {
            get { return this._emailAddress; }
            set
            {
                this._emailAddress = value;
                this.NotifyOfPropertyChange(() => this.EmailAddress);
                this.NotifyOfPropertyChange(() => this.CanRegister);
            }
        }
        #endregion Properties

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterViewModel"/> class.
        /// </summary>
        /// <param name="usersClient">The users client.</param>
        public RegisterViewModel(UsersClient usersClient)
        {
            this._usersClient = usersClient;
        }
        #endregion Constructors

        #region IScreen Member
        /// <summary>
        /// Called when initializing.
        /// </summary>
        protected override void OnInitialize()
        {
            this.DisplayName = "Xemio Share - Register";
        }
        #endregion IScreen Member

        #region Methods
        /// <summary>
        /// Gets a value indicating whether the "Register" method is executable.
        /// </summary>
        public bool CanRegister
        {
            get
            {
                return string.IsNullOrWhiteSpace(this.Username) == false &&
                       string.IsNullOrWhiteSpace(this.Password) == false &&
                       string.IsNullOrWhiteSpace(this.EmailAddress) == false;
            }
        }
        /// <summary>
        /// Registers a new user.
        /// </summary>
        public async void Register()
        {
            HttpResponseMessage response = await this._usersClient.PostUserAsync(new CreateUser
                                                                                        {
                                                                                            EMailAddress = this.EmailAddress,
                                                                                            Username = this.Username,
                                                                                            Password = this.Password
                                                                                        });

            if (response.StatusCode == HttpStatusCode.Created)
            {
                this.TryClose(true);
            }
            else
            {
                string message = await response.Content.ReadAsStringAsync();
                MessageBox.Show(message);
            }
        }
        #endregion Methods
    }
}
