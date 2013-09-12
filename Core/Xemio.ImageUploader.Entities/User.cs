using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xemio.ImageUploader.Entities
{
    /// <summary>
    /// Contains basic information about the <see cref="User"/>.
    /// </summary>
    public class User : AggregateRoot
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Gets or sets the EMail address.
        /// </summary>
        public string EMailAddress { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="User"/> is disabled.
        /// </summary>
        public bool IsDeactivated { get; set; }
        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public UserSettings Settings { get; set; }
    }
}
