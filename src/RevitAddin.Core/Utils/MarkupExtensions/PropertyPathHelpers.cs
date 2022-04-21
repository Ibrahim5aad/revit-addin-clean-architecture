using System.Windows;
using System.Windows.Data;

namespace RevitAddin.Core.Markup
{
    public static class PropertyPathHelpers
    {
        public static object Evaluate(PropertyPath path, object source)
        {
            var target = new DependencyTarget();
            var binding = new Binding() { Path = path, Source = source, Mode = BindingMode.OneTime };
            BindingOperations.SetBinding(target, DependencyTarget.ValueProperty, binding);

            return target.Value;
        }

        private class DependencyTarget : DependencyObject
        {
            public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(DependencyTarget));

            public object Value
            {
                get => GetValue(ValueProperty);
                set => SetValue(ValueProperty, value);
            }
        }
    }
}