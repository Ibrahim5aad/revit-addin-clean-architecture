using System.Windows;

namespace RevitAddin.Core.Utils.BindingExtensions
{

    /// <summary>
    /// Since the binding will not work on objects that is not part of the
    /// Visual Tree, an object of Class BindingProxy acts as a proxy between the 
    /// binding target and source.
    /// Implements the <see cref="System.Windows.Freezable" />
    /// </summary>
    /// <seealso cref="System.Windows.Freezable" />
    public class BindingProxy : Freezable
    {

        /// <summary>
        /// When implemented in a derived class, creates a new instance of
        /// the <see cref="T:System.Windows.Freezable" /> derived class.
        /// </summary>
        /// <returns>The new instance.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }


        /// <summary>
        /// Gets or sets the data proxied
        /// by this proxy.
        /// </summary>
        /// <value>The data.</value>
        public object Data
        {
            get { return GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }


        /// <summary>
        /// The data dependency property.
        /// </summary>
        public static readonly DependencyProperty DataProperty
                           = DependencyProperty.Register(nameof(Data),
                                                         typeof(object),
                                                         typeof(BindingProxy),
                                                         new UIPropertyMetadata(null));
    }
}
