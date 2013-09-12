using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xemio.ImageUploader.Shared;

namespace Xemio.ImageUploader.Client.WebServices
{
    /// <summary>
    /// Provides basic functionality for webservice calls.
    /// </summary>
    public abstract class BaseClient
    {
        #region Fields
        protected readonly HttpClient _client;
        private readonly Session _session;
        #endregion
        
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersClient" /> class.
        /// </summary>
        /// <param name="client">The client to use to send requests.</param>
        /// <param name="session">The session.</param>
        protected BaseClient(HttpClient client, Session session)
        {
            this._client = client;
            this._session = session;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sets the authentication header.
        /// </summary>
        /// <param name="requestContent">The request content.</param>
        protected void SetAuthenticationHeader(string requestContent)
        {
            string authenticationHash = Hash.CreateContentHash(requestContent, this._session.Password, this._session.Username);
            string authenticationString = string.Format("{0}:{1}", this._session.Username, authenticationHash);

            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Xemio", authenticationString);
        }
        #endregion
    }
}
