using System.Windows;
using System.Windows.Controls;

namespace Xemio.ImageUploader.Client.UserInterface.AttachedProperties
{
    public static class PasswordBoxBinding
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
                                                typeof(string),
                                                typeof(PasswordBoxBinding),
                                                new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static string GetPassword(DependencyObject dependencyObject)
        {
            return (string)dependencyObject.GetValue(PasswordProperty);
        }
        public static void SetPassword(DependencyObject dependencyObject, string value)
        {
            dependencyObject.SetValue(PasswordProperty, value);
        }

        private static readonly DependencyProperty IsUpdatingProperty =
           DependencyProperty.RegisterAttached("IsUpdating", 
                                               typeof(bool),
                                               typeof(PasswordBoxBinding));
        
        private static bool GetIsUpdating(DependencyObject dependencyObject)
        {
            return (bool)dependencyObject.GetValue(IsUpdatingProperty);
        }
        private static void SetIsUpdating(DependencyObject dependencyObject, bool value)
        {
            dependencyObject.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (GetIsUpdating(passwordBox) == false)
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }
        
        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
}
