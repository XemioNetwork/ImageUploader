using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Windows.Input;
using AttributeRouting.Web.Http;
using Castle.Core.Logging;
using Raven.Client;
using Xemio.ImageUploader.Core.Services;
using Xemio.ImageUploader.Entities;
using Xemio.ImageUploader.Infrastructure.Authentication;
using Xemio.ImageUploader.Infrastructure.Raven.Indexes;
using Xemio.ImageUploader.Models;
using Xemio.ImageUploader.Infrastructure.Extensions;
using Xemio.ImageUploader.Shared;

namespace Xemio.ImageUploader.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for interaction with users.
    /// </summary>
    public class UsersController : BaseController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController" /> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public UsersController(IAsyncDocumentSession documentSession) 
            : base(documentSession)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns all users.
        /// </summary>
        [GET("Users")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> GetAllUsers()
        {
            var query = this.DocumentSession.Query<User, UsersByUsername>();
            
            List<User> result = new List<User>();
            using (var enumerator = await this.DocumentSession.Advanced.StreamAsync(query))
            { 
                while (await enumerator.MoveNextAsync())
                {
                    result.Add(enumerator.Current.Document);
                }
            }
            return Request.CreateResponse(HttpStatusCode.Found, result);
        }
        /// <summary>
        /// Returns the <see cref="User"/> with the given id.
        /// </summary>
        /// <param name="id">The id.</param>
        [GET("Users/{id:int}")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> GetUser(int id)
        {
            var user = await this.DocumentSession.LoadAsync<User>(id);

            if (user == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);

            return Request.CreateResponse(HttpStatusCode.Found, user);
        }
        /// <summary>
        /// Gets the current user.
        /// </summary>
        [GET("Users/Current")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> GetCurrent()
        {
            return Request.CreateResponse(HttpStatusCode.Found, await this.CurrentUser);
        }
        /// <summary>
        /// Creates a new <see cref="User"/>.
        /// </summary>
        /// <param name="createUser">The createUser.</param>
        [POST("Users")]
        public async Task<HttpResponseMessage> PostUser([FromBody]CreateUser createUser)
        {
            if (createUser == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The user is null.");

            if (string.IsNullOrWhiteSpace(createUser.Username))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The username is null or empty.");

            if (await this.IsUsernameAvailable(createUser.Username) == false)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The username is not available.");

            if (string.IsNullOrWhiteSpace(createUser.Password) || createUser.Password.Length < 7)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The password is too short. It has to be at least 8 characters long.");

            var user = new User
            {
                Username = createUser.Username,
                EMailAddress = createUser.EMailAddress,
                IsDeactivated = false,
                Settings = new UserSettings
                {
                    ShowNotifications = true,
                    StartWithWindows = true,
                    CurrentWindowKeys = new[] {Key.LeftCtrl, Key.LeftShift, Key.F1},
                    SelectFileKeys = new[] {Key.LeftCtrl, Key.LeftShift, Key.F2},
                    SelectAreaKeys = new[] {Key.LeftCtrl, Key.LeftShift, Key.F3},
                    WholeScreenKeys = new[] {Key.LeftCtrl, Key.LeftShift, Key.F4}
                }
            };
            await this.DocumentSession.StoreAsync(user);

            var authenticationData = new AuthenticationData
            {
                UserId = user.Id,
                PasswordHash = Hash.CreatePasswordHash(createUser.Password, user.Username)
            };
            await this.DocumentSession.StoreAsync(authenticationData);
            
            this.Logger.DebugFormat("Created user '{0}' with id '{1}'.", user.Username, user.Id);

            return Request.CreateResponse(HttpStatusCode.Created, user);
        }
        /// <summary>
        /// Updates the <see cref="User"/>.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="id">The id.</param>
        [PUT("Users/{id:int}")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> PutUser([FromBody]User user, int id)
        {
            if (user == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The user is null.");
            
            string userId = this.DocumentSession.Advanced.GetStringIdFor<User>(id);

            User currentUser = await this.CurrentUser;
            if (currentUser.Id != userId)
                return Request.CreateResponse(HttpStatusCode.Forbidden, "You can only update your own account details.");

            currentUser.EMailAddress = user.EMailAddress;
            currentUser.IsDeactivated = user.IsDeactivated;
            currentUser.Settings = user.Settings;

            this.Logger.DebugFormat("Updated user '{0}' with id '{1}'.", currentUser.Username, currentUser.Id);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Determines whetherthe given username is available.
        /// </summary>
        private async Task<bool> IsUsernameAvailable(string username)
        {
            return (await this.DocumentSession.Query<User, UsersByUsername>()
                              .AnyAsync(f => f.Username == username)) == false;
        }
        #endregion
    }
}
