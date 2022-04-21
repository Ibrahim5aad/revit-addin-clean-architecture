using System.Linq;
using System.Windows;

namespace RevitAddin.Core.Utils.Extensions
{
    /// <summary>
    /// Class WindowExtensions.
    /// </summary>
    public static class WindowExtensions
    {
        /// <summary>
        /// Determines whether the specified window is open.
        /// </summary>
        /// <param name="window">The window.</param>
        /// <returns><c>true</c> if the specified window is open; otherwise, <c>false</c>.</returns>
        public static bool IsOpen(this Window window)
        {
            return Application.Current?.Windows?.Cast<Window>().Any(x => x == window) ?? false;
        }
    }
}
