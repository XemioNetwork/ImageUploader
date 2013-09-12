using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xemio.ImageUploader.Entities;

namespace Xemio.ImageUploader.Models
{
    /// <summary>
    /// Used to create new <see cref="UploadedImage"/>.
    /// </summary>
    public class CreateUploadedImage
    {
        /// <summary>
        /// Gets or sets the image data.
        /// </summary>
        public byte[] Image { get; set; }
        /// <summary>
        /// Gets or sets the image extension.
        /// </summary>
        public string Extension { get; set; }
    }
}
