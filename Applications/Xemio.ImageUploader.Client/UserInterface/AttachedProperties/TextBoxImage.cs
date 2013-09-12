using System.Windows;
using System.Windows.Media;

namespace Xemio.ImageUploader.Client.UserInterface.AttachedProperties
{
    public static class TextBoxImage
    {
        public static readonly DependencyProperty ImageProperty = DependencyProperty.RegisterAttached("Image",
                                                                                                      typeof (ImageSource),
                                                                                                      typeof (TextBoxImage));

        public static ImageSource GetImage(DependencyObject dependencyObject)
        {
            return dependencyObject.GetValue(ImageProperty) as ImageSource;
        }
        public static void SetImage(DependencyObject dependencyObject, ImageSource imageSource)
        {
            dependencyObject.SetValue(ImageProperty, imageSource);
        }
    }
}
