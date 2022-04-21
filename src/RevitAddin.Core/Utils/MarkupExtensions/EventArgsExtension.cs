using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace RevitAddin.Core.Markup
{
	/// <summary>
	/// A XAML markup extension to allow for providing access to the event arguments.
	/// For example: passing the event arguments or an event property to the bounded method.
	/// Implements the <see cref="System.Windows.Markup.MarkupExtension" />
	/// </summary>
	/// <seealso cref="System.Windows.Markup.MarkupExtension" />
	public class EventArgsExtension : MarkupExtension
	{
		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgsExtension"/> class.
		/// </summary>
		public EventArgsExtension()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EventArgsExtension"/> class.
		/// </summary>
		/// <param name="path">The path.</param>
		public EventArgsExtension(string path)
		{
			Path = new PropertyPath(path);
		}

		#endregion

		#region Properties

		public PropertyPath Path { get; set; }

		public IValueConverter Converter { get; set; }

		public object ConverterParameter { get; set; }

		public Type ConverterTargetType { get; set; }


		[TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
		public CultureInfo ConverterCulture { get; set; }

		#endregion

		#region Methods


		/// <summary>
		/// When implemented in a derived class, returns an object that is provided as the value of the target property for this markup extension.
		/// </summary>
		/// <param name="serviceProvider">A service provider helper that can provide services for the markup extension.</param>
		/// <returns>The object value to set on the property where the extension is applied.</returns>
		public override object ProvideValue(IServiceProvider serviceProvider)
		{
			return this;
		}


		/// <summary>
		/// Gets the argument value.
		/// </summary>
		/// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
		/// <param name="language">The language.</param>
		/// <returns>System.Object.</returns>
		internal object GetArgumentValue(EventArgs eventArgs, XmlLanguage language)
		{
			if (Path == null)
				return eventArgs;

			object value = PropertyPathHelpers.Evaluate(Path, eventArgs);

			if (Converter != null)
				value = Converter.Convert(value, ConverterTargetType ?? typeof(object), ConverterParameter, ConverterCulture ?? language.GetSpecificCulture());

			return value;
		}

		#endregion
	}
}
