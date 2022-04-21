using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace RevitAddin.Core.ViewModels
{
	/// <summary>
	/// This is a base of ViewModel.
	/// Implements the <see cref="System.ComponentModel.INotifyPropertyChanged" />
	/// Implements the <see cref="System.ComponentModel.INotifyDataErrorInfo" />
	/// </summary>
	/// <seealso cref="System.ComponentModel.INotifyPropertyChanged" />
	/// <seealso cref="System.ComponentModel.INotifyDataErrorInfo" />
	public abstract class ViewModelBase : INotifyPropertyChanged, INotifyDataErrorInfo
	{

		#region INotifyPropertyChanged

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;


		/// <summary>
		/// Notify that property has changed.
		/// </summary>
		/// <param name="property">The property.</param>
		protected virtual void OnPropertyChanged([CallerMemberName] string property = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));


		/// <summary>
		/// Notify that property has changed
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="property">The property.</param>
		/// <param name="propertyName">Name of the property.</param>
		protected virtual void OnPropertyChanged<T>(Func<T> property, [CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


		/// <summary>
		/// Notify that Property has changed and also set the value of backing field
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="backingField">Backing Field of the Property</param>
		/// <param name="value">New Value</param>
		/// <param name="propertyName">Name of The Property</param>
		/// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
		protected virtual bool OnPropertyChanged<T>(ref T backingField, T value, [CallerMemberName] string propertyName = "")
		{
			if (EqualityComparer<T>.Default.Equals(backingField, value))
				return false;

			backingField = value;
			OnPropertyChanged(propertyName);
			return true;
		}


		#endregion

		#region INotifyDataErrorInfo

		/// <summary>
		/// Occurs when the validation errors have changed for a property or for the entire entity.
		/// </summary>
		public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;


		/// <summary>
		/// The validation errors.
		/// </summary>
		public readonly Dictionary<string, List<string>> ValidationErrors = new Dictionary<string, List<string>>();



		/// <summary>
		/// Gets a value that indicates whether the view model has validation errors.
		/// </summary>
		/// <value><c>true</c> if this instance has errors; otherwise, <c>false</c>.</value>
		public bool HasErrors
		{
			get
			{
				var propErrorsCount = ValidationErrors.Values.FirstOrDefault(r => r.Count > 0);
				if (propErrorsCount != null)
					return true;
				else
					return false;
			}
		}



		/// <summary>
		/// Gets the validation errors for a specified property or for the entire entity.
		/// </summary>
		/// <param name="propertyName">The name of the property to retrieve validation errors for; or <see langword="null" /> or <see cref="F:System.String.Empty" />, to retrieve entity-level errors.</param>
		/// <returns>The validation errors for the property or entity.</returns>
		public IEnumerable GetErrors(string propertyName)
		{
			IEnumerable result;
			if (string.IsNullOrEmpty(propertyName))
			{
				List<string> allErrors = new List<string>();

				foreach (var keyValuePair in this.ValidationErrors)
				{
					allErrors.AddRange(keyValuePair.Value);
				}

				result = allErrors;
			}
			else
			{
				if (ValidationErrors.ContainsKey(propertyName))
				{
					result = ValidationErrors[propertyName];
				}
				else
				{
					result = new List<string>();
				}
			}

			return result;
		}



		/// <summary>
		/// Called when [property errors changed].
		/// </summary>
		/// <param name="name">The name.</param>
		protected void OnPropertyErrorsChanged(string name)
		{
			ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(name));
		}

		#endregion

	}
}
