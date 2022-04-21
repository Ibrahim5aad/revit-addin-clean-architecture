using System;
using System.Windows;

namespace RevitAddin.Core.DI.Interfaces
{
    /// <summary>
    /// Interface IUIService
    /// </summary>
    /// <typeparam name="ViewModel">The type of the view model.</typeparam>
    public interface IUIService<ViewModel>
    {

        #region Events

        /// <summary>
        /// Occurs when [closed].
        /// </summary>
        event EventHandler Closed;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the window startup location.
        /// </summary>
        /// <value>The window startup location.</value>
        WindowStartupLocation WindowStartupLocation { get; set; }


        /// <summary>
        /// Gets or sets the data context.
        /// </summary>
        /// <value>The data context.</value>
        object DataContext { get; set; }


        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        Window Owner { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the view model.
        /// </summary>
        /// <param name="vm">The vm.</param>
        void SetViewModel(ViewModel vm);


        /// <summary>
        /// Determines whether this ui service is shown.
        /// </summary>
        /// <returns><c>true</c> if this ui service is shown; otherwise, <c>false</c>.</returns>
        bool IsShown();


        /// <summary>
        /// Shows the dialog.
        /// </summary>
        /// <returns><c>true</c> if showed, <c>false</c> otherwise.</returns>
        bool? ShowDialog();


        /// <summary>
        /// Focuses this instance.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool Focus();


        /// <summary>
        /// Shows this ui service.
        /// </summary>
        void Show();


        /// <summary>
        /// Hides this ui service.
        /// </summary>
        void Hide();


        /// <summary>
        /// Closes this ui service.
        /// </summary>
        void Close();

        #endregion

    }
}
