namespace Uncas.Core.Interop
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Provides functions to capture the entire screen, 
    /// or a particular window, and save it to a file.
    /// </summary>
    /// <remarks>
    /// See http://www.developerfusion.com/code/4630/capture-a-screen-shot/.
    /// See http://stackoverflow.com/questions/1163761/c-capture-screenshot-of-active-window.
    /// </remarks>
    public class ScreenCapture : IScreenCapture
    {
        /// <summary>
        /// Captures the foreground window.
        /// </summary>
        /// <returns>
        /// The image containing the foreground window.
        /// </returns>
        public Image CaptureForegroundWindow()
        {
            IntPtr handle = Window.CurrentForeground.Handle;
            return CaptureWindow(handle);
        }

        /// <summary>
        /// Creates an Image object containing a screen shot 
        /// of the entire desktop.
        /// </summary>
        /// <returns>The image containing the screen shot.</returns>
        public Image CaptureScreen()
        {
            return CaptureWindow(SafeNativeMethods.User32.GetDesktopWindow());
        }

        /// <summary>
        /// Captures a screen shot of the entire desktop,
        /// and saves it to a file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="format">The format.</param>
        public void CaptureScreenToFile(string fileName, ImageFormat format)
        {
            Image img = CaptureScreen();
            img.Save(fileName, format);
        }

        /// <summary>
        /// Creates an Image object containing a screen shot 
        /// of a specific window.
        /// </summary>
        /// <param name="handle">The handle to the window
        /// (in windows forms, this is obtained by the Handle property).</param>
        /// <returns>The image containing the capture of the window.</returns>
        public Image CaptureWindow(IntPtr handle)
        {
            // get te hDC of the target window
            IntPtr hdcSrc = SafeNativeMethods.User32.GetWindowDC(handle);

            // get the size
            var windowRect =
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
            IntPtr bitmapPointer = SafeNativeMethods.GDI32.CreateCompatibleBitmap(
                hdcSrc,
                width,
                height);

            // select the bitmap object
            IntPtr oldBitmap = SafeNativeMethods.GDI32.SelectObject(hdcDest, bitmapPointer);

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
            SafeNativeMethods.GDI32.SelectObject(hdcDest, oldBitmap);

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
            Image img = Image.FromHbitmap(bitmapPointer);

            // free up the Bitmap object
            SafeNativeMethods.GDI32.DeleteObject(bitmapPointer);

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
        /// Holds safe native methods used by the containing class.
        /// </summary>
        private static class SafeNativeMethods
        {
            /// <summary>
            /// Helper class containing Gdi32 API functions.
            /// </summary>
            internal static class GDI32
            {
                // BitBlt dwRop parameter
                public const int SRCCOPY = 0x00CC0020;

                [DllImport("gdi32.dll")]
                [return: MarshalAs(UnmanagedType.Bool)]
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
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool DeleteDC(IntPtr hDC);

                [DllImport("gdi32.dll")]
                [return: MarshalAs(UnmanagedType.Bool)]
                public static extern bool DeleteObject(IntPtr hObject);

                [DllImport("gdi32.dll")]
                public static extern IntPtr SelectObject(
                    IntPtr hDC,
                    IntPtr hObject);
            }

            /// <summary>
            /// Helper class containing User32 API functions.
            /// </summary>
            internal static class User32
            {
                [DllImport("user32.dll")]
                public static extern IntPtr GetDesktopWindow();

                [DllImport("user32.dll")]
                public static extern IntPtr GetWindowDC(IntPtr hWnd);

                [DllImport("user32.dll")]
                public static extern int GetWindowRect(
                    IntPtr hWnd,
                    ref RECT rect);

                [DllImport("user32.dll")]
                public static extern int ReleaseDC(
                    IntPtr hWnd,
                    IntPtr hDC);

                /// <summary>
                /// Struct that represents a rectangle of a window.
                /// </summary>
                [StructLayout(LayoutKind.Sequential)]
                public struct RECT
                {
                    public readonly int left;
                    public readonly int top;
                    public readonly int right;
                    public readonly int bottom;
                }
            }
        }
    }
}