using System;
using System.Collections;
using System.Text;

namespace RevitAddin.Core.Win32
{
	/// <summary>
	/// contains collected control info
	/// </summary>
	public struct Control
	{
		public string Path;
		public string Text;
		public string InternalText;
		public string ClassName;
		public int Level;
		public int ParentId;
	}


	public class EnumControls
	{

		public ArrayList Controls = new ArrayList();

		/// <summary>
		/// enum all controls including child controls
		/// </summary>
		/// <param name="print_to_log"></param>
		public EnumControls()
		{
			Controls.Clear();
			Functions.EnumWindows(new Functions.EnumProc(this.enumControlsCallBack), 0);
		}


		/// <summary>
		/// enum all child controls of a given parent control/window
		/// </summary>
		/// <param name="hwnd"> the handle of the parent control/window </param>
		public EnumControls(IntPtr hwnd)
		{
			Functions.EnumChildWindows(hwnd, new Functions.EnumProc(this.enumControlsCallBack), 0);
		}

		/// <summary>
		/// invoke building child control tree for give parent window
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="lValue"></param>
		/// <returns></returns>
		bool enumControlsCallBack(IntPtr hwnd, int lValue)
		{
			Control w = new Control
			{
				Level = 0,
				ParentId = -1
			};

			StringBuilder s = new StringBuilder(255); ;

			Functions.GetClassName(hwnd, s, 255);
			w.ClassName = s.ToString();
			Functions.GetWindowText(hwnd, s, 255);
			w.Text = s.ToString();
			Functions.InternalGetWindowText(hwnd, s, 255);
			w.InternalText = s.ToString();
			w.Path = "[" + w.Text + "]";

			Controls.Add(w);

			Functions.EnumChildWindows(hwnd, new Functions.EnumProc(this.enumChildControlsCallBack), Controls.Count - 1);

			return true;
		}

		/// <summary>
		/// build child control tree recurcively
		/// </summary>
		/// <param name="hwnd"></param>
		/// <param name="parentId"></param>
		/// <returns></returns>
		bool enumChildControlsCallBack(IntPtr hwnd, int parentId)
		{
			Control w = new Control
			{
				Level = ((Control)Controls[parentId]).Level + 1,
				ParentId = parentId
			};

			StringBuilder s = new StringBuilder(255);

			Functions.GetClassName(hwnd, s, 255);
			w.ClassName = s.ToString();
			Functions.GetWindowText(hwnd, s, 255);
			w.Text = s.ToString();
			Functions.InternalGetWindowText(hwnd, s, 255);
			w.InternalText = s.ToString();
			w.Path = ((Control)Controls[parentId]).Path + "[" + w.Text + "]";

			Controls.Add(w);

			Functions.EnumChildWindows(hwnd, new Functions.EnumProc(this.enumChildControlsCallBack), Controls.Count - 1);

			return true;
		}
	}
}