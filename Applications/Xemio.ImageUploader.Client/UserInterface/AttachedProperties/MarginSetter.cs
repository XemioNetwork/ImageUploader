using System.Windows;
using System.Windows.Controls;

namespace Xemio.ImageUploader.Client.UserInterface.AttachedProperties
{
    public class MarginSetter
    {
        public static readonly DependencyProperty MarginProperty = DependencyProperty.RegisterAttached("Margin",
                                                                                                       typeof(Thickness),
                                                                                                       typeof(MarginSetter),
                                                                                                       new UIPropertyMetadata(new Thickness(), MarginChangedCallback));
        public static Thickness GetMargin(DependencyObject obj)
        {
             return (Thickness)obj.GetValue(MarginProperty);
        }
        public static void SetMargin(DependencyObject obj, Thickness value)
        {
             obj.SetValue(MarginProperty, value);
        }
       
       
        public static void MarginChangedCallback(object sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;
       
            if (panel == null) 
                return;
       
            panel.Loaded += panel_Loaded;
        }
       
        private static void panel_Loaded(object sender, RoutedEventArgs e)
        {            
            var panel = sender as Panel;
       
            foreach (var child in panel.Children)
            {
                var frameworkElement = child as FrameworkElement;
       
                if (frameworkElement == null) 
                    continue;
       
                frameworkElement.Margin = MarginSetter.GetMargin(panel);
            }
        }
    }
}
