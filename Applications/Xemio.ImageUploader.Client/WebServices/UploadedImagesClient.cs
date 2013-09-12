using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xemio.ImageUploader.Entities;
using Xemio.ImageUploader.Models;
using Xemio.ImageUploader.Shared;

namespace Xemio.ImageUploader.Client.WebServices
{
    /// <summary>
    /// Provides access to the <see cref="UploadedImage"/> part of the webservice.
    /// </summary>
    public class UploadedImagesClient : BaseClient
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UsersClient" /> class.
        /// </summary>
        /// <param name="client">The client to use to send requests.</param>
        /// <param name="session">The session.</param>
        public UploadedImagesClient(HttpClient client, Session session)
            : base(client, session)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets all uploaded images.
        /// </summary>
        /// <param name="user">The user.</param>
        public async Task<HttpResponseMessage> GetAllUploadedImages(User user)
        {
            this.SetAuthenticationHeader(string.Empty);

            string userId = user.Id.Split('/').Last();

            HttpResponseMessage response = await this._client.GetAsync(string.Format("Users/{0}/Images", userId));
            return response;
        }
        /// <summary>
        /// Gets the address of the given <see cref="UploadedImage" />.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="image">The image.</param>
        public string GetUploadedImageAddress(User user, UploadedImage image)
        {
            string url = this.GetRelativeUploadedImageAddress(user, image);
            return new Uri(this._client.BaseAddress, url).AbsoluteUri;
        }
        /// <summary>
        /// Gets the uploaded image.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="image">The image.</param>
        public async Task<HttpResponseMessage> GetUploadedImage(User user, UploadedImage image)
        {
            string url = this.GetRelativeUploadedImageAddress(user, image);

            HttpResponseMessage response = await this._client.GetAsync(url);
            return response;
        }
        /// <summary>
        /// Creates a new image.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="image">The image.</param>
        public async Task<HttpResponseMessage> PostUploadedImage(User user, CreateUploadedImage image)
        {
            string requestString = await JsonConvert.SerializeObjectAsync(image);
            this.SetAuthenticationHeader(requestString);

            string userId = user.Id.Split('/').Last();

            HttpResponseMessage response = await this._client.PostAsync(string.Format("Users/{0}/Images", userId), new StringContent(requestString, Encoding.UTF8, "application/json"));
            return response;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Gets the uploaded image address part.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="image">The image.</param>
        private string GetRelativeUploadedImageAddress(User user, UploadedImage image)
        {
            string userId = user.Id.Split('/').Last();
            string imageId = image.Id.Split('/').Last();

            return string.Format("Users/{0}/Images/{1}", userId, imageId);
        }
        #endregion
    }
}
