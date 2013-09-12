using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Xemio.ImageUploader.Client.UserInterface.ValueConverter
{
    /// <summary>
    /// Converts a <see cref="ImageSource"/> to a <see cref="Thickness"/>.
    /// Used fot 
    /// </summary>
    public class ImageSourceToMarginConverter : IValueConverter
    {
        #region Properties
        /// <summary>
        /// Gets or sets the margin.
        /// </summary>
        public Thickness Margin { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSourceToMarginConverter"/> class.
        /// </summary>
        public ImageSourceToMarginConverter()
        {
            this.Margin = new Thickness(5, 0, 0, 0);
        }
        #endregion

        #region IValueConverter Member
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ImageSource imageSource = value as ImageSource;
            if (imageSource == null)
                return new Thickness(0);

            return this.Margin;
        }
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
