using System.Windows;
using System.Windows.Controls;

namespace ViewModelSupport_SL
{
    public static class Update
    {
        public static DependencyProperty PropertyChangedProperty =
            DependencyProperty.RegisterAttached(
                "PropertyChanged", 
                typeof (bool), 
                typeof (Update),
                new PropertyMetadata(WhenPropertyChanged));

        private static DependencyProperty BehaviorProperty =
            DependencyProperty.RegisterAttached(
                "Behavior",
                typeof (PropertyChangedBehavior),
                typeof (TextBox), null);

        private static void WhenPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if(textBox == null) return;

            var addBehavior = (bool)e.NewValue;

            textBox.SetValue(BehaviorProperty, 
                addBehavior ? new PropertyChangedBehavior(textBox) : null);
        }

        public static bool GetPropertyChanged(DependencyObject d) { return (bool)d.GetValue(PropertyChangedProperty); }
        public static void SetPropertyChanged(DependencyObject d, bool value) { d.SetValue(PropertyChangedProperty, value); }

        public class PropertyChangedBehavior
        {
            public PropertyChangedBehavior(TextBox textBox)
            {
                textBox.TextChanged += (s, e) => {
                    var binding = textBox.GetBindingExpression(TextBox.TextProperty);
                    if(binding != null)
                        binding.UpdateSource();
                };
            }
        }
    }
}
