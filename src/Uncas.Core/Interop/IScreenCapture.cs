using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Uncas.Core.Interop
{
    /// <summary>
    /// Provides functions to capture the entire screen, 
    /// or a particular window, and save it to a file.
    /// </summary>
    public interface IScreenCapture
    {
        /// <summary>
        /// Captures the foreground window.
        /// </summary>
        /// <returns>The image containing the foreground window.</returns>
        Image CaptureForegroundWindow();

        /// <summary>
        /// Creates an Image object containing a screen shot 
        /// of the entire desktop.
        /// </summary>
        /// <returns>The image containing the screen shot.</returns>
        Image CaptureScreen();

        /// <summary>
        /// Captures a screen shot of the entire desktop,
        /// and saves it to a file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        void CaptureScreenToFile(string fileName, ImageFormat format);

        /// <summary>
        /// Creates an Image object containing a screen shot 
        /// of a specific window.
        /// </summary>
        /// <param name="handle">The handle to the window
        /// (in windows forms, this is obtained by the Handle property).</param>
        /// <returns>The image containing the capture of the window.</returns>
        Image CaptureWindow(IntPtr handle);

        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        void CaptureWindowToFile(IntPtr handle, string fileName, ImageFormat format);
    }
}
