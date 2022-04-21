using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace RevitAddin.ExternalCommands
{
    /// <summary>
    /// Class to see if command is availble if a document is open
    /// </summary>
    public class CommandAvailability : IExternalCommandAvailability
    {
        /// <summary>
        /// Check the given command is availble in document or not.
        /// </summary>
        /// <param name="applicationData"></param>
        /// <param name="selectedCategories"></param>
        /// <returns></returns>
        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            if (applicationData.ActiveUIDocument != null && applicationData.ActiveUIDocument.Document != null)
            {
                if (!applicationData.ActiveUIDocument.Document.IsFamilyDocument)
                {
                    return true;
                }
            }

            return false;
        }
    }

}
