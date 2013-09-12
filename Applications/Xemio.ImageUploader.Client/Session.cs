using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xemio.ImageUploader.Entities;

namespace Xemio.ImageUploader.Client
{
    /// <summary>
    /// Contains information about the current user.
    /// </summary>
    public class Session
    {
        /// <summary>
        /// Gets or sets the username of the <see cref="CurrentUser"/>.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password of the <see cref="CurrentUser"/>.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        public User CurrentUser { get; set; }
    }
}
