using RevitAddin.Core.DI.Interfaces;

namespace RevitAddin.Core.ViewModels
{

	/// <summary>
	/// Class MainWindowViewModel
	/// </summary>
	/// <seealso cref="RevitAddin.Core.ViewModels.ViewModelBase" />
	/// <seealso cref="RevitAddin.Core.DI.Interfaces.IMainWindowViewModel" />
	public partial class MainWindowViewModel : ViewModelBase, IMainWindowViewModel
	{

		#region Constructor

		/// <summary>
		/// Initializes a new instance of the <see cref="MainWindowViewModel"/> class.
		/// </summary>
		/// <param name="instanceCategories">The instance categories.</param>
		public MainWindowViewModel()
		{
		}

		#endregion

	}
}
