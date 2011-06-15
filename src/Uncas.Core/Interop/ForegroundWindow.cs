using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Uncas.Core.Interop
{
    /// <remarks>
    /// http://stackoverflow.com/questions/115868/how-do-i-get-the-title-of-the-current-active-window-using-c
    /// </remarks>
    public class ForegroundWindow
    {
        // Declare external functions.
        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(
            IntPtr hWnd,
            StringBuilder text,
            int count);

        public static string GetForegroundWindowTitle()
        {
            int chars = 256;
            StringBuilder buff = new StringBuilder(chars);

            // Obtain the handle of the active window.
            IntPtr handle = GetForegroundWindow();

            // Update the controls.
            if (GetWindowText(handle, buff, chars) > 0)
            {
                return buff.ToString();
            }

            return null;
        }

        public static Process GetProcessAtWindowHandle(IntPtr windowHandle)
        {
            return Process.GetProcesses().SingleOrDefault(
                p => p.MainWindowHandle == windowHandle);
        }

        public static Process GetForegroundWindowProcess()
        {
            IntPtr foregroundWindowHandle = GetForegroundWindow();
            return GetProcessAtWindowHandle(foregroundWindowHandle);
        }
    }
}
