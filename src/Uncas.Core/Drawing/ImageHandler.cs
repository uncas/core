using System;
using System.Drawing;
using System.IO;

namespace Uncas.Core.Drawing
{
    /// <summary>
    /// Handles images.
    /// </summary>
    public class ImageHandler
    {
        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public Image GetImage(string fileName)
        {
            return Bitmap.FromFile(fileName);
        }

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public Image GetImage(byte[] buffer)
        {
            Image img = null;
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                img = Bitmap.FromStream(ms);
            }
            return img;
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <returns></returns>
        public byte[] GetBytes(Image img)
        {
            byte[] bytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bytes = ms.ToArray();
            }
            return bytes;
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="maxWidthAndHeight">Height of the max width and.</param>
        /// <returns></returns>
        public byte[] GetThumbnail(byte[] buffer, int maxWidthAndHeight)
        {
            Size dummy = new Size();
            return GetThumbnail(buffer, maxWidthAndHeight, out dummy);
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="maxWidthAndHeight">Height of the max width and.</param>
        /// <param name="originalSize">Size of the original.</param>
        /// <returns></returns>
        public byte[] GetThumbnail(
            byte[] buffer,
            int maxWidthAndHeight,
            out Size originalSize)
        {
            byte[] output = null;
            using (Image img = GetImage(buffer))
            {
                originalSize = img.Size;
                output = GetBytes(GetThumbnail(img, maxWidthAndHeight));
            }
            return output;
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <returns></returns>
        public byte[] GetThumbnail(byte[] buffer, int maxWidth, int maxHeight)
        {
            Size dummy = new Size();
            return GetThumbnail(buffer, maxWidth, maxHeight, out dummy);
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="originalSize">Size of the original.</param>
        /// <returns></returns>
        public byte[] GetThumbnail(
            byte[] buffer,
            int maxWidth,
            int maxHeight,
            out Size originalSize)
        {
            byte[] output = null;
            using (Image img = GetImage(buffer))
            {
                originalSize = img.Size;
                output = GetBytes(GetThumbnail(img, maxWidth, maxHeight));
            }
            return output;
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="maxWidthAndHeight">Height of the max width and.</param>
        /// <returns></returns>
        public Image GetThumbnail(Image img, int maxWidthAndHeight)
        {
            int width = img.Width;
            int height = img.Height;
            int thumbWidth = 0;
            int thumbHeight = 0;
            if (width > height)
            {
                thumbWidth = maxWidthAndHeight;
                thumbHeight = height * thumbWidth / width;
            }
            else
            {
                thumbHeight = maxWidthAndHeight;
                thumbWidth = width * thumbHeight / height;
            }
            return GetThumbnail(img, new Size(thumbWidth, thumbHeight));
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <returns></returns>
        public Image GetThumbnail(Image img, int maxWidth, int maxHeight)
        {
            int width = img.Width;
            int height = img.Height;
            int thumbWidth = 0;
            int thumbHeight = 0;
            if (width * maxHeight > height * maxWidth)
            {
                thumbWidth = maxWidth;
                thumbHeight = height * thumbWidth / width;
            }
            else
            {
                thumbHeight = maxHeight;
                thumbWidth = width * thumbHeight / height;
            }
            return GetThumbnail(img, new Size(thumbWidth, thumbHeight));
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="img">The img.</param>
        /// <param name="thumbSize">Size of the thumb.</param>
        /// <returns></returns>
        public Image GetThumbnail(Image img, Size thumbSize)
        {
            int thumbWidth = thumbSize.Width;
            int thumbHeight = thumbSize.Height;
            if (thumbWidth > img.Width || thumbHeight > img.Height)
            {
                thumbWidth = img.Width;
                thumbHeight = img.Height;
            }
            Image bmp = new Bitmap(thumbWidth, thumbHeight);
            using (Graphics g = Graphics.FromImage(bmp))
                g.DrawImage(img, 0, 0, thumbWidth, thumbHeight);
            return bmp;
        }
    }
}