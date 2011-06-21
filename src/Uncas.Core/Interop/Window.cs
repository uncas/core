using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Uncas.Core.Interop
{
    /// <summary>
    /// Wraps a window in Microsoft Windows.
    /// </summary>
    /// <remarks>
    /// See http://stackoverflow.com/questions/115868/how-do-i-get-the-title-of-the-current-active-window-using-c.
    /// </remarks>
    public class Window
    {
        private readonly IntPtr _handle;
        private readonly string _title;
        private readonly string _processName;

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        /// <param name="handle">The window handle.</param>
        public Window(IntPtr handle)
        {
            _handle = handle;
            _title = GetTitle(handle);
            _processName = GetProcessName(handle);
        }

        /// <summary>
        /// Gets the current foreground window.
        /// </summary>
        /// <value>The current foreground window.</value>
        public static Window CurrentForeground
        {
            get
            {
                return new Window(SafeNativeMethods.GetForegroundWindow());
            }
        }

        /// <summary>
        /// Gets the handle of the window.
        /// </summary>
        /// <value>The handle of the window.</value>
        public IntPtr Handle
        {
            get { return _handle; }
        }

        /// <summary>
        /// Gets the title of the window.
        /// </summary>
        /// <value>The title of the window.</value>
        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Gets the name of the process.
        /// </summary>
        /// <value>
        /// The name of the process.
        /// </value>
        public string ProcessName
        {
            get
            {
                return _processName;
            }
        }

        private static string GetProcessName(IntPtr handle)
        {
            using (Process process =
                GetProcessAtWindowHandle(handle))
            {
                return process != null ?
                    process.ProcessName :
                    null;
            }
        }

        private static Process GetProcessAtWindowHandle(
            IntPtr windowHandle)
        {
            return Process.GetProcesses().FirstOrDefault(
                p => p.MainWindowHandle == windowHandle);
        }

        private static string GetTitle(
            IntPtr windowHandle)
        {
            int chars = 256;
            StringBuilder buff = new StringBuilder(chars);
            int getWindowTextResult =
                SafeNativeMethods.GetWindowText(
                windowHandle,
                buff,
                chars);
            if (getWindowTextResult > 0)
            {
                return buff.ToString();
            }

            return null;
        }

        /// <summary>
        /// Holds safe native methods used by the containing class.
        /// </summary>
        private static class SafeNativeMethods
        {
            [DllImport("user32.dll")]
            internal static extern IntPtr GetForegroundWindow();

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern int GetWindowText(
                IntPtr hWnd,
                StringBuilder text,
                int count);
        }
    }
}