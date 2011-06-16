using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Uncas.Core.Interop
{
    /// <summary>
    /// Represents the foreground window among all windows.
    /// </summary>
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

        /// <summary>
        /// Gets the current foreground window.
        /// </summary>
        /// <value>The current foreground window.</value>
        public static ForegroundWindow Current
        {
            get
            {
                return new ForegroundWindow();
            }
        }

        /// <summary>
        /// Gets the handle.
        /// </summary>
        /// <value>The handle.</value>
        public IntPtr Handle
        {
            get { return _handle; }
        }

        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// Gets the process.
        /// </summary>
        /// <value>The process.</value>
        public Process Process
        {
            get { return _process; }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">Set to <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
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