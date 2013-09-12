using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Xemio.ImageUploader.Client.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="HttpContent"/> class.
    /// </summary>
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Reads the content as an instance of the specified type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T">The content type.</typeparam>
        /// <param name="content">The content.</param>
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            string contentString = await content.ReadAsStringAsync();
            return await JsonConvert.DeserializeObjectAsync<T>(contentString);
        }
    }
}
