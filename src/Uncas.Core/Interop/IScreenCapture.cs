using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Uncas.Core.Interop
{
    public interface IScreenCapture
    {
        Image CaptureForegroundWindow();
        Image CaptureScreen();
        void CaptureScreenToFile(string fileName, ImageFormat format);
        Image CaptureWindow(IntPtr handle);
        void CaptureWindowToFile(IntPtr handle, string fileName, ImageFormat format);
    }
}
