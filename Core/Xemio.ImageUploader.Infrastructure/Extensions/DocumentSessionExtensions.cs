using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client;

namespace Xemio.ImageUploader.Infrastructure.Extensions
{
    /// <summary>
    /// Contains extension methods for the <see cref="IDocumentStore"/> class.
    /// </summary>
    public static class DocumentSessionExtensions
    {
        /// <summary>
        /// Gets the string id for the given type.
        /// </summary>
        /// <typeparam name="T">The entity type.</typeparam>
        /// <param name="session">The session.</param>
        /// <param name="id">The id.</param>
        public static string GetStringIdFor<T>(this IAdvancedDocumentSessionOperations session, int id)
        {
            return session.DocumentStore.Conventions.FindFullDocumentKeyFromNonStringIdentifier(id, typeof(T), false);
        } 
    }
}
