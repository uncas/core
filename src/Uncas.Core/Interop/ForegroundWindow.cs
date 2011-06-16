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
    public class ForegroundWindow : IDisposable
    {
        private IntPtr _handle;
        private string _title;
        private Process _process;

        private ForegroundWindow()
        {
            _handle = SafeNativeMethods.GetForegroundWindow();
            _title = GetTitle();
            _process = GetProcessAtWindowHandle(_handle);
        }

        ~ForegroundWindow()
        {
            Dispose(false);
        }

        public static ForegroundWindow Current
        {
            get
            {
                return new ForegroundWindow();
            }
        }

        public IntPtr Handle { get { return _handle; } }
        public string Title { get { return _title; } }
        public Process Process { get { return _process; } }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Gets rid of managed resources:
                _title = null;
                if (_process != null)
                {
                    _process.Dispose();
                    _process = null;
                }
            }

            // Gets rid of unmanaged resources: none so far...
        }

        private static Process GetProcessAtWindowHandle(IntPtr windowHandle)
        {
            return Process.GetProcesses().FirstOrDefault(
                p => p.MainWindowHandle == windowHandle);
        }

        private string GetTitle()
        {
            int chars = 256;
            StringBuilder buff = new StringBuilder(chars);

            // Update the controls.
            if (SafeNativeMethods.GetWindowText(_handle, buff, chars) > 0)
            {
                return buff.ToString();
            }

            return null;
        }

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