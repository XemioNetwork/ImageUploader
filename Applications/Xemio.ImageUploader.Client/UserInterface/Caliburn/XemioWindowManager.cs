using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace Xemio.ImageUploader.Client.UserInterface.Caliburn
{
    /// <summary>
    /// A custom <see cref="WindowManager"/> setting the window icon.
    /// </summary>
    public class XemioWindowManager : WindowManager
    {
        /// <summary>
        /// Creates a window.
        /// </summary>
        /// <param name="rootModel">The view model.</param>
        /// <param name="isDialog">Whether or not the window is being shown as a dialog.</param>
        /// <param name="context">The view context.</param>
        /// <param name="settings">The optional popup settings.</param>
        protected override Window CreateWindow(object rootModel, bool isDialog, object context, IDictionary<string, object> settings)
        {
            Window window = base.CreateWindow(rootModel, isDialog, context, settings);

            window.Icon = new BitmapImage(new Uri("pack://application:,,,/UserInterface/Images/Icon.png"));

            return window;
        }
    }
}
