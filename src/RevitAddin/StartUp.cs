using RevitAddin.Core.DI;
using RevitAddin.Core.DI.Interfaces;
using RevitAddin.Core.ViewModels;
using RevitAddin.UI.Windows;

namespace RevitAddin
{
	/// <summary>
	/// Class StartUp.
	/// </summary>
	public class StartUp
	{

		/// <summary>
		/// Configures the IoC service registerations.
		/// </summary>
		public void ConfigureServices()
		{
			IoC.Default
			  .RegisterSingleton<IMainWindow, MainWindow>()
			  .RegisterSingleton<IMainWindowViewModel, MainWindowViewModel>()
			  ;
		}
	}
}
