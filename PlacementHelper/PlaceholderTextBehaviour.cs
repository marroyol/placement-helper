using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlacementHelper
{
    public static class PlaceholderTextBehaviour
    {
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.RegisterAttached(
                "PlaceholderText",
                typeof(string),
                typeof(PlaceholderTextBehaviour),
                new PropertyMetadata(string.Empty, OnPlaceholderTextChanged));

        public static string GetPlaceholderText(DependencyObject obj)
        {
            return (string)obj.GetValue(PlaceholderTextProperty);
        }

        public static void SetPlaceholderText(DependencyObject obj, string value)
        {
            obj.SetValue(PlaceholderTextProperty, value);
        }

        private static void OnPlaceholderTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                textBox.GotFocus += TextBox_GotFocus;
                textBox.LostFocus += TextBox_LostFocus;
                UpdatePlaceholderText(textBox, (string)e.NewValue);
            }
        }

        private static void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                if (textBox.Text == GetPlaceholderText(textBox))
                {
                    textBox.Text = string.Empty;
                    textBox.Foreground = Brushes.White; // Set to your primary text color
                }
            }
        }

        private static void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                UpdatePlaceholderText(textBox, GetPlaceholderText(textBox));
            }
        }

        private static void UpdatePlaceholderText(TextBox textBox, string placeholderText)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = placeholderText;
                textBox.Foreground = Brushes.Gray; // Set to a muted color
            }
            else if (textBox.Text != placeholderText)
            {
                textBox.Foreground = Brushes.White; // Set to your primary text color
            }
        }
    }
}
