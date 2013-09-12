using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using AttributeRouting.Web.Http;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Connection;
using Raven.Json.Linq;
using Xemio.ImageUploader.Entities;
using Xemio.ImageUploader.Infrastructure.Authentication;
using Xemio.ImageUploader.Infrastructure.Extensions;
using Xemio.ImageUploader.Infrastructure.Raven.Indexes;
using Xemio.ImageUploader.Models;

namespace Xemio.ImageUploader.Infrastructure.Controllers
{
    /// <summary>
    /// Controller for interaction with uploaded images.
    /// </summary>
    public class UploadedImagesController : BaseController
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadedImagesController"/> class.
        /// </summary>
        /// <param name="documentSession">The document session.</param>
        public UploadedImagesController(IAsyncDocumentSession documentSession) 
            : base(documentSession)
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns all <see cref="UploadedImage"/> with the given <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        [GET("Users/{userId:int}/Images")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> GetAllUploadedImages(int userId)
        {
            User user = await this.DocumentSession.LoadAsync<User>(userId);
            if (user == null)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "No user was found.");
            
            var query = this.DocumentSession.Query<UploadedImage, UploadedImagesByUserId>().Where(f => f.UserId == user.Id);

            List<UploadedImage> result = new List<UploadedImage>();
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
        /// Returns the <see cref="UploadedImage"/> with the given <paramref name="imageId"/>.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="imageId">The image id.</param>
        [GET("Users/{userId:int}/Images/{imageId:int}")]
        public async Task<HttpResponseMessage> GetUploadedImage(int userId, int imageId)
        {
            UploadedImage image = await this.DocumentSession.LoadAsync<UploadedImage>(imageId);
            string userStringId = this.DocumentSession.Advanced.GetStringIdFor<User>(userId);

            if (image == null || image.UserId != userStringId)
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, string.Format("No image with id '{0}' was found.", imageId));

            Attachment attachment = await this.DocumentSession.Advanced.DocumentStore.AsyncDatabaseCommands.GetAttachmentAsync(image.Id);

            return Request.CreateImageResponse(attachment.Data(), image.ImageExtension);
        }
        /// <summary>
        /// Creates a new <see cref="UploadedImage"/> with the <paramref name="createImage"/> data.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="createImage">The create image.</param>
        [POST("Users/{userId:int}/Images")]
        [RequiresAuthentication]
        public async Task<HttpResponseMessage> PostUploadedImage(int userId, [FromBody]CreateUploadedImage createImage)
        {
            if (createImage == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The image is null.");

            if (createImage.Image == null || createImage.Image.Length == 0)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The image is null.");

            if (string.IsNullOrWhiteSpace(createImage.Extension))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The image extension is null or empty.");

            string userStringId = this.DocumentSession.Advanced.GetStringIdFor<User>(userId);

            if (userStringId != (await this.CurrentUser).Id)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "You can only create images for your own account.");

            var image = new UploadedImage
                            {
                                IsDeactivated = false,
                                UserId = userStringId,
                                ImageExtension = createImage.Extension,
                                CreatedDate = DateTime.Now
                            };
            await this.DocumentSession.StoreAsync(image);

            await this.DocumentSession.Advanced.DocumentStore.AsyncDatabaseCommands.PutAttachmentAsync(image.Id, null, createImage.Image, null);

            this.Logger.DebugFormat("Created image '{0}'.", image.Id);

            return Request.CreateResponse(HttpStatusCode.Created, image);
        }
        #endregion
    }
}
