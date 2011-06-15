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
        private IntPtr _handle;

        private ForegroundWindow()
        {
            _handle = GetForegroundWindow();
        }

        public static ForegroundWindow Current
        {
            get
            {
                return new ForegroundWindow();
            }
        }

        public IntPtr Handle
        {
            get
            {
                return _handle;
            }
        }

        public string Title
        {
            get
            {
                int chars = 256;
                StringBuilder buff = new StringBuilder(chars);

                // Update the controls.
                if (GetWindowText(_handle, buff, chars) > 0)
                {
                    return buff.ToString();
                }

                return null;
            }
        }

        public Process Process
        {
            get
            {
                return GetProcessAtWindowHandle(_handle);
            }
        }

        private static Process GetProcessAtWindowHandle(IntPtr windowHandle)
        {
            return Process.GetProcesses().FirstOrDefault(
                p => p.MainWindowHandle == windowHandle);
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(
            IntPtr hWnd,
            StringBuilder text,
            int count);
    }
}