using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xemio.ImageUploader.Client.Extensions;
using Xemio.ImageUploader.Entities;
using Xemio.ImageUploader.Models;
using Xemio.ImageUploader.Shared;

namespace Xemio.ImageUploader.Client.WebServices
{
    /// <summary>
    /// Provides access to the <see cref="User"/> part of the webservice.
    /// </summary>
    public class UsersClient : BaseClient
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersClient" /> class.
        /// </summary>
        /// <param name="client">The client to use to send requests.</param>
        /// <param name="session">The session.</param>
        public UsersClient(HttpClient client, Session session)
            : base(client, session)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets the current user.
        /// </summary>
        public async Task<HttpResponseMessage> GetCurrentUserAsync()
        {
            this.SetAuthenticationHeader(string.Empty);

            HttpResponseMessage response = await this._client.GetAsync("Users/Current");
            return response;
        }
        /// <summary>
        /// Updates the given user.
        /// </summary>
        /// <param name="user">The user.</param>
        public async Task<HttpResponseMessage> PutUserAsync(User user)
        {
            string requestString = await JsonConvert.SerializeObjectAsync(user);
            this.SetAuthenticationHeader(requestString);

            string userId = user.Id.Split('/').Last();

            HttpResponseMessage response = await this._client.PutAsync(string.Format("Users/{0}", userId), new StringContent(requestString, Encoding.UTF8, "application/json"));
            return response;
        }
        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="userData">The user data.</param>
        public async Task<HttpResponseMessage> PostUserAsync(CreateUser userData)
        {
            string requestString = await JsonConvert.SerializeObjectAsync(userData);

            HttpResponseMessage response = await this._client.PostAsync("Users", new StringContent(requestString, Encoding.UTF8, "application/json"));
            return response;
        }
        #endregion
    }
}
