using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Xemio.ImageUploader.Client.UserInterface.ValueConverter
{
    /// <summary>
    /// Converts a <see cref="byte"/> <see cref="Array"/> too an <see cref="ImageSource"/>
    /// </summary>
    public class ByteArrayToImageConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the default source to use when the <see cref="byte"/> <see cref="Array"/> is null.
        /// </summary>
        public ImageSource DefaultSource { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] data = value as byte[];
            if (data == null)
            {
                return this.DefaultSource;
            }

            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = new MemoryStream(data);
            image.EndInit();

            return image;
        }

        /// <summary>
        /// Not possible. Don't call or use.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
