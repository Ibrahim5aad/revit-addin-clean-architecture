using RevitAddin.Core.DI;
using RevitAddin.Core.DI.Interfaces;
using RevitAddin.Core.Utils.Extensions;
using System.Windows;

namespace RevitAddin.UI.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
         
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public MainWindow(IMainWindowViewModel viewModel)
        {
            InitializeComponent();
            SetTitle(Application.PluginName);
            DataContext = viewModel;
        }

        /// <summary>
        /// Sets the title of the window.
        /// </summary>
        /// <param name="title">The title</param>
        private void SetTitle(string title)
        {
            this.Title = title;
        }

        /// <summary>
        /// Determines whether this window is shown.
        /// </summary>
        /// <returns><c>true</c> if this window is shown; otherwise, <c>false</c>.</returns>
        public bool IsShown()
        {
            return this.IsOpen();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the current window.
        /// </summary>
        /// <value>The current window.</value>
        public static Window CurrentWindow { get; internal set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the view model.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void SetViewModel(IMainWindowViewModel viewModel)
        {
            DataContext = viewModel;
        }


        /// <summary>
        /// Handles the <see cref="E:WindowClosed" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void OnWindowClosed(object sender, System.EventArgs e)
        {
            IoC.Default.Unregister<IMainWindow>();
            IoC.Default.Unregister<IMainWindowViewModel>();
        }

        #endregion

    }
}
