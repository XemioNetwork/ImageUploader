using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Xemio.ImageUploader.Infrastructure.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="HttpRequestMessage"/> class.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Creates a image response with the given <paramref name="image"/> and the <paramref name="extension"/>.
        /// </summary>
        /// <param name="requestMessage">The request message.</param>
        /// <param name="image">The image.</param>
        /// <param name="extension">The extension.</param>
        public static HttpResponseMessage CreateImageResponse(this HttpRequestMessage requestMessage, Stream image, string extension)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Found)
                                               {
                                                   Content = new StreamContent(image)
                                               };

            response.Content.Headers.ContentType = new MediaTypeHeaderValue("images/" + extension);

            return response;
        }
    }
}
