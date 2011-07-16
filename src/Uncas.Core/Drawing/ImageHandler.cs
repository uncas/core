using System;
using System.Diagnostics.CodeAnalysis;
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
            Image image = null;
            using (MemoryStream ms = new MemoryStream(buffer))
            {
                image = Bitmap.FromStream(ms);
            }

            return image;
        }

        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        public byte[] GetBytes(Image image)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            byte[] bytes = null;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
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
        /// Gets the thumbnail image as a byte array.
        /// </summary>
        /// <param name="buffer">The bytes of the original image.</param>
        /// <param name="maxWidthAndHeight">Max width and height of the requested thumbnail.</param>
        /// <param name="originalSize">The size of the original image.</param>
        /// <returns>The bytes of the thumbnail image.</returns>
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
        /// Gets the thumbnail image as a byte array.
        /// </summary>
        /// <param name="buffer">The bytes of the original image.</param>
        /// <param name="maxWidth">The max width of the requested thumbnail.</param>
        /// <param name="maxHeight">The max height of the requested thumbnail.</param>
        /// <param name="originalSize">The size of the original image.</param>
        /// <returns>The bytes of the thumbnail image.</returns>
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
        /// <param name="image">The img.</param>
        /// <param name="maxWidthAndHeight">Height of the max width and.</param>
        /// <returns></returns>
        public Image GetThumbnail(Image image, int maxWidthAndHeight)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            int width = image.Width;
            int height = image.Height;
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

            return GetThumbnail(image, new Size(thumbWidth, thumbHeight));
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="image">The img.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <returns></returns>
        public Image GetThumbnail(Image image, int maxWidth, int maxHeight)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            int width = image.Width;
            int height = image.Height;
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

            return GetThumbnail(image, new Size(thumbWidth, thumbHeight));
        }

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="thumbSize">Size of the thumb.</param>
        /// <returns></returns>
        [SuppressMessage(
            "Microsoft.Reliability", 
            "CA2000:Dispose objects before losing scope",
            Justification = "The criteria for specific exceptions are absent.")]
        public Image GetThumbnail(Image image, Size thumbSize)
        {
            if (image == null)
            {
                throw new ArgumentNullException("image");
            }

            int thumbWidth = thumbSize.Width;
            int thumbHeight = thumbSize.Height;
            if (thumbWidth > image.Width || thumbHeight > image.Height)
            {
                thumbWidth = image.Width;
                thumbHeight = image.Height;
            }

            Image bmp = new Bitmap(thumbWidth, thumbHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(image, 0, 0, thumbWidth, thumbHeight);
            }

            return bmp;
        }
    }
}