﻿using Autodesk.Revit.UI;

namespace RevitAddin.Core.ExternalEvents
{
	/// <summary>
	/// Interface IRevitContextEvent
	/// </summary>
	public interface IRevitContextEvent
	{

		/// <summary>
		/// Executes this event.
		/// </summary>
		/// <param name="application">The Revit UI application.</param>
		void Execute(UIApplication application);
	}
}
