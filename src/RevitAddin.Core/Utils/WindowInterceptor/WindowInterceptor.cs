using RevitAddin.Core.Win32;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace RevitAddin.Core.Utils
{
	/// <summary>
	/// Intercept creation of window and get its HWND
	/// </summary>
	public class WindowInterceptor
	{
		private IntPtr _hookId = IntPtr.Zero;
		private Functions.HookProc _cbf;
		public delegate void ProcessWindow(IntPtr hwnd);
		private ProcessWindow _processWindow;
		private IntPtr _ownerWinoow = IntPtr.Zero;



		/// <summary>
		/// Start dialog box interception for the specified owner window
		/// </summary>
		/// <param name="ownerWnd">owner window, if it is IntPtr.Zero then any windows will be intercepted</param>
		/// <param name="processWnd">custom delegate to process intercepted window. It should be a fast code in order to have no message stack overflow.</param>
		public WindowInterceptor(IntPtr ownerWnd, ProcessWindow processWnd)
		{
			if (processWnd == null)
				throw new Exception("process_window cannot be null!");
			_processWindow = processWnd;
			_ownerWinoow = ownerWnd;

			_cbf = new Functions.HookProc(dlgBoxHookProc);
			_hookId = Functions.SetWindowsHookEx(HookType.WH_CALLWNDPROCRET, _cbf, IntPtr.Zero, Functions.GetCurrentThreadId());
		}

		~WindowInterceptor()
		{
			if (_hookId != IntPtr.Zero)
				Functions.UnhookWindowsHookEx(_hookId);
		}

		/// <summary>
		/// An error message that will be handled
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Stop intercepting. Should be called to calm unmanaged code correctly
		/// </summary>
		public void Stop()
		{
			if (_hookId != IntPtr.Zero)
				Functions.UnhookWindowsHookEx(_hookId);
			_hookId = IntPtr.Zero;
		}



		private IntPtr dlgBoxHookProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode < 0)
				return Functions.CallNextHookEx(_hookId, nCode, wParam, lParam);

			CWPRETSTRUCT msg = (CWPRETSTRUCT)Marshal.PtrToStructure(lParam, typeof(CWPRETSTRUCT));

			//filter out create window events only
			if (msg.message == (uint)Messages.WM_SHOWWINDOW)
			{
				int h = Functions.GetWindow(msg.hwnd, Functions.GW_OWNER);
				//check if owner is that is specified
				if (_ownerWinoow == IntPtr.Zero || _ownerWinoow == new IntPtr(h))
				{
					if (_processWindow != null)
					{
						var children = new EnumControls(msg.hwnd);
						if (children.Controls.ToArray().Any(c => ((Win32.Control)c).ClassName == "Edit"
							&& ((Win32.Control)c).Text.Contains(Message)))
						{
							_processWindow(msg.hwnd);
						}
					}
				}
			}

			return Functions.CallNextHookEx(_hookId, nCode, wParam, lParam);
		}
	}
}