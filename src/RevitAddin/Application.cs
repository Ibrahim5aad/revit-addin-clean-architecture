using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using RevitAddin.Core.ExternalEvents;
using RevitAddin.UI;
using System;

namespace RevitAddin
{

	[Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]
	[Autodesk.Revit.Attributes.Regeneration(Autodesk.Revit.Attributes.RegenerationOption.Manual)]
	public class Application : IExternalApplication
	{

		/// <summary>
		/// Gets or sets the name of the plugin.
		/// </summary>
		/// <value>The name of the plugin.</value>
		public static string PluginName { get; set; }


		/// <summary>
		/// Executes some tasks when Autodesk Revit starts.
		/// </summary>
		/// <param name="application">A handle to the application being started.</param>
		/// <returns>Indicates if the external application completes its work successfully.</returns>
		public Result OnStartup(UIControlledApplication application)
		{
			try
			{
				Int32.TryParse(application.ControlledApplication.VersionNumber, out int revitVersion);

				PluginName = application.ActiveAddInId.GetAddInName();

				var tabName = "Custom Tab";

				RevitUIFactory.AddRibbonTab(application, tabName);

				RibbonPanel detailingPanel = RevitUIFactory.AddRibbonPanel
								(application, tabName, "Custom Panel", true);

				RevitUIFactory.AddRibbonButton
				  (PluginName, tabName, detailingPanel,
				  typeof(MainCommand),
				  Properties.Resources.Icon_32x32,
				  Properties.Resources.Icon_16x16,
				  "Custom command discription.");

				ExternalExecutor.CreateExternalEvent();

				return Result.Succeeded;
			}
			catch (Exception)
			{
				return Result.Failed;
			}
		}


		/// <summary>
		/// Executes some tasks when Autodesk Revit shuts down.
		/// </summary>
		/// <param name="application">A handle to the application being shut down.</param>
		/// <returns>Indicates if the external application completes its work successfully.</returns>
		public Result OnShutdown(UIControlledApplication application)
		{
			return Result.Succeeded;
		}

	}


	/// <summary>
	/// Class AvailableIfOpenDoc.
	/// Implements the <see cref="Autodesk.Revit.UI.IExternalCommandAvailability" />
	/// </summary>
	/// <seealso cref="Autodesk.Revit.UI.IExternalCommandAvailability" />
	public class CommandAvailability : IExternalCommandAvailability
	{
		/// <summary>
		/// Provides control over whether your external command is enabled or disabled.
		/// </summary>
		/// <param name="applicationData">An ApplicationServices.Application object which contains reference to Application
		/// needed by external command.</param>
		/// <param name="selectedCategories">An list of categories of the elements which have been selected in Revit in the active document,
		/// or an empty set if no elements are selected or there is no active document.</param>
		/// <returns>Indicates whether Revit should enable or disable the corresponding external command.</returns>
		/// <remarks>This callback will be called by Revit's user interface any time there is a contextual change. Therefore, the callback
		/// must be fast and is not permitted to modify the active document and be blocking in any way.</remarks>
		public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
		{
			if (applicationData.ActiveUIDocument != null && applicationData.ActiveUIDocument.Document != null &&
			  !applicationData.ActiveUIDocument.Document.IsFamilyDocument)
				return true;
			return false;
		}
	}
}
