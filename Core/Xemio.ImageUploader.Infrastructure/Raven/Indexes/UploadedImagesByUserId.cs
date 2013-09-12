using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Raven.Client.Indexes;
using Xemio.ImageUploader.Entities;

namespace Xemio.ImageUploader.Infrastructure.Raven.Indexes
{
    /// <summary>
    /// Enables querying of <see cref="UploadedImage"/> by an user id.
    /// </summary>
    public class UploadedImagesByUserId : AbstractIndexCreationTask<UploadedImage>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UploadedImagesByUserId"/> class.
        /// </summary>
        public UploadedImagesByUserId()
        {
            this.Map = images => from image in images
                                 select new
                                            {
                                                image.UserId
                                            };
        }

        /// <summary>
        /// Gets the name of the index.
        /// </summary>
        /// <value>
        /// The name of the index.
        /// </value>
        public override string IndexName
        {
            get { return "UploadedImages/ByUserId"; }
        }
    }
}
