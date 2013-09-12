using System;

namespace Xemio.ImageUploader.Entities
{
    /// <summary>
    /// Contains information about an <see cref="UploadedImage"/>
    /// </summary>
    public class UploadedImage : AggregateRoot
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="UploadedImage"/> is disabled.
        /// </summary>
        public bool IsDeactivated { get; set; }
        /// <summary>
        /// Gets or sets the image extension.
        /// </summary>
        public string ImageExtension { get; set; }
        /// <summary>
        /// Gets or sets the date when this <see cref="UploadedImage"/> was created.
        /// </summary>
        public DateTime CreatedDate { get; set; }
    }
}