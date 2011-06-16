using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Uncas.Core.Interop
{
    /// <summary>
    /// Provides functions to capture the entire screen, 
    /// or a particular window, and save it to a file.
    /// </summary>
    /// <remarks>
    /// http://www.developerfusion.com/code/4630/capture-a-screen-shot/
    /// http://stackoverflow.com/questions/1163761/c-capture-screenshot-of-active-window
    /// </remarks>
    public class ScreenCapture : IScreenCapture
    {
        /// <summary>
        /// Creates an Image object containing a screen shot 
        /// of the entire desktop.
        /// </summary>
        /// <returns></returns>
        public Image CaptureScreen()
        {
            return CaptureWindow(SafeNativeMethods.User32.GetDesktopWindow());
        }

        public Image CaptureForegroundWindow()
        {
            IntPtr handle;
            using (var foregroundWindow = ForegroundWindow.Current)
                handle = foregroundWindow.Handle;
            return CaptureWindow(handle);
        }

        /// <summary>
        /// Creates an Image object containing a screen shot 
        /// of a specific window
        /// </summary>
        /// <param name="handle">The handle to the window. 
        /// (In windows forms, this is obtained by the Handle property.)</param>
        /// <returns></returns>
        public Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = SafeNativeMethods.User32.GetWindowDC(handle);

            // get the size
            SafeNativeMethods.User32.RECT windowRect =
                new SafeNativeMethods.User32.RECT();
            int windowRectResult =
                SafeNativeMethods.User32.GetWindowRect(handle, ref windowRect);
            if (windowRectResult != 1)
            {
                throw new InteropException(
                    "Error getting window rectangle",
                    windowRectResult);
            }

            int width = windowRect.right - windowRect.left;
            int height = windowRect.bottom - windowRect.top;

            // create a device context we can copy to
            IntPtr hdcDest = SafeNativeMethods.GDI32.CreateCompatibleDC(hdcSrc);

            // create a bitmap we can copy it to,
            // using GetDeviceCaps to get the width/height
            IntPtr hBitmap = SafeNativeMethods.GDI32.CreateCompatibleBitmap(
                hdcSrc,
                width,
                height);

            // select the bitmap object
            IntPtr hOld = SafeNativeMethods.GDI32.SelectObject(hdcDest, hBitmap);

            // bitblt over
            SafeNativeMethods.GDI32.BitBlt(
                 hdcDest,
                 0,
                 0,
                 width,
                 height,
                 hdcSrc,
                 0,
                 0,
                 SafeNativeMethods.GDI32.SRCCOPY);

            // restore selection
            SafeNativeMethods.GDI32.SelectObject(hdcDest, hOld);

            // clean up 
            SafeNativeMethods.GDI32.DeleteDC(hdcDest);
            int releaseDCResult =
                SafeNativeMethods.User32.ReleaseDC(handle, hdcSrc);
            if (releaseDCResult != 1)
            {
                throw new InteropException(
                    "Error releasing DC",
                    releaseDCResult);
            }

            // get a .NET image object for it
            Image img = Image.FromHbitmap(hBitmap);

            // free up the Bitmap object
            SafeNativeMethods.GDI32.DeleteObject(hBitmap);

            return img;
        }

        /// <summary>
        /// Captures a screen shot of a specific window, and saves it to a file.
        /// </summary>
        /// <param name="handle">The handle.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        public void CaptureWindowToFile(
            IntPtr handle,
            string fileName,
            ImageFormat format)
        {
            Image img = CaptureWindow(handle);
            img.Save(fileName, format);
        }

        /// <summary>
        /// Captures a screen shot of the entire desktop,
        /// and saves it to a file
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        public void CaptureScreenToFile(string fileName, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(fileName, format);
        }

        private static class SafeNativeMethods
        {
            /// <summary>
            /// Helper class containing Gdi32 API functions
            /// </summary>
            internal static class GDI32
            {
                // BitBlt dwRop parameter
                public const int SRCCOPY = 0x00CC0020;

                [DllImport("gdi32.dll")]
                public static extern bool BitBlt(
                    IntPtr hObject,
                    int nXDest,
                    int nYDest,
                    int nWidth,
                    int nHeight,
                    IntPtr hObjectSource,
                    int nXSrc,
                    int nYSrc,
                    int dwRop);

                [DllImport("gdi32.dll")]
                public static extern IntPtr CreateCompatibleBitmap(
                    IntPtr hDC,
                    int nWidth,
                    int nHeight);

                [DllImport("gdi32.dll")]
                public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

                [DllImport("gdi32.dll")]
                public static extern bool DeleteDC(IntPtr hDC);

                [DllImport("gdi32.dll")]
                public static extern bool DeleteObject(IntPtr hObject);

                [DllImport("gdi32.dll")]
                public static extern IntPtr SelectObject(
                    IntPtr hDC,
                    IntPtr hObject);
            }

            /// <summary>
            /// Helper class containing User32 API functions
            /// </summary>
            internal static class User32
            {
                [StructLayout(LayoutKind.Sequential)]
                public struct RECT
                {
                    public int left;
                    public int top;
                    public int right;
                    public int bottom;
                }

                [DllImport("user32.dll")]
                public static extern IntPtr GetDesktopWindow();

                [DllImport("user32.dll")]
                public static extern IntPtr GetWindowDC(IntPtr hWnd);

                [DllImport("user32.dll")]
                public static extern int ReleaseDC(
                    IntPtr hWnd,
                    IntPtr hDC);

                [DllImport("user32.dll")]
                public static extern int GetWindowRect(
                    IntPtr hWnd,
                    ref RECT rect);
            }
        }
    }
}