using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Xemio.ImageUploader.Entities
{
    /// <summary>
    /// Contains all settings of a <see cref="User"/>.
    /// </summary>
    public class UserSettings
    {
        /// <summary>
        /// Gets or sets the keys to select a file.
        /// </summary>
        public Key[] SelectFileKeys { get; set; }
        /// <summary>
        /// Gets or sets the keys to make a screenshot of the whole screen.
        /// </summary>
        public Key[] WholeScreenKeys { get; set; }
        /// <summary>
        /// Gets or sets the keys to make a screenshot of the current window.
        /// </summary>
        public Key[] CurrentWindowKeys { get; set; }
        /// <summary>
        /// Gets or sets the keys to make a screenshot of a selection.
        /// </summary>
        public Key[] SelectAreaKeys { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [show notifications].
        /// </summary>
        public bool ShowNotifications { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [start with windows].
        /// </summary>
        public bool StartWithWindows { get; set; }
    }
}
  