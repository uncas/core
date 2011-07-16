namespace Uncas.Core.Drawing
{
    using System;
    using System.Drawing;

    interface IImageHandler
    {
        byte[] GetBytes(Image image);
        Image GetImage(byte[] buffer);
        Image GetImage(string fileName);
        byte[] GetThumbnail(byte[] buffer, int maxWidth, int maxHeight);
        byte[] GetThumbnail(byte[] buffer, int maxWidth, int maxHeight, out Size originalSize);
        byte[] GetThumbnail(byte[] buffer, int maxWidthAndHeight);
        byte[] GetThumbnail(byte[] buffer, int maxWidthAndHeight, out Size originalSize);
        Image GetThumbnail(Image image, Size thumbSize);
        Image GetThumbnail(Image image, int maxWidth, int maxHeight);
        Image GetThumbnail(Image image, int maxWidthAndHeight);
    }
}
