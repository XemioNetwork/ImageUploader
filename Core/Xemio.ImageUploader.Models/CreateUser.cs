using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xemio.ImageUploader.Models
{
    /// <summary>
    /// Used to create a new user.
    /// </summary>
    public class CreateUser
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the E mail address.
        /// </summary>
        public string EMailAddress { get; set; }
    }
}
