namespace Uncas.Core.Drawing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;

    /// <summary>
    /// Image handler interface.
    /// </summary>
    public interface IImageHandler
    {
        /// <summary>
        /// Gets the bytes.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        byte[] GetBytes(Image image);

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        Image GetImage(byte[] buffer);

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        Image GetImage(string fileName);

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <returns></returns>
        byte[] GetThumbnail(byte[] buffer, int maxWidth, int maxHeight);

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <returns></returns>
        ImageBufferResizeResult GetThumbnailResult(
            byte[] buffer,
            int maxWidth,
            int maxHeight);

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <param name="originalSize">Size of the original.</param>
        /// <returns></returns>
        [Obsolete("Use GetThumbnailResult instead.")]
        byte[] GetThumbnail(
            byte[] buffer,
            int maxWidth,
            int maxHeight,
            out Size originalSize);

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="maxWidthAndHeight">Height of the max width and.</param>
        /// <returns></returns>
        byte[] GetThumbnail(byte[] buffer, int maxWidthAndHeight);

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="buffer">The bytes of the original image.</param>
        /// <param name="maxWidthAndHeight">Height of the max width and.</param>
        /// <returns>
        /// The thumbnail result.
        /// </returns>
        ImageBufferResizeResult GetThumbnailResult(
            byte[] buffer,
            int maxWidthAndHeight);

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="maxWidthAndHeight">Height of the max width and.</param>
        /// <param name="originalSize">Size of the original.</param>
        /// <returns></returns>
        [Obsolete("Use GetThumbnailResult instead.")]
        byte[] GetThumbnail(
            byte[] buffer,
            int maxWidthAndHeight,
            out Size originalSize);

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="thumbSize">Size of the thumb.</param>
        /// <returns></returns>
        Image GetThumbnail(Image image, Size thumbSize);

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="maxWidth">Width of the max.</param>
        /// <param name="maxHeight">Height of the max.</param>
        /// <returns></returns>
        Image GetThumbnail(Image image, int maxWidth, int maxHeight);

        /// <summary>
        /// Gets the thumbnail.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <param name="maxWidthAndHeight">Height of the max width and.</param>
        /// <returns></returns>
        Image GetThumbnail(Image image, int maxWidthAndHeight);
    }

    /// <summary>
    /// Represents the buffer (byte array) result of resizing an image.
    /// </summary>
    public class ImageBufferResizeResult
    {
        private readonly byte[] _buffer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageBufferResizeResult"/> class.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <param name="originalSize">Size of the original image.</param>
        public ImageBufferResizeResult(
            byte[] buffer,
            Size originalSize)
        {
            _buffer = buffer;
            OriginalSize = originalSize;
        }

        /// <summary>
        /// Gets the buffer.
        /// </summary>
        /// <value>
        /// The buffer.
        /// </value>
        public IEnumerable<byte> Buffer
        {
            get
            {
                return _buffer;
            }
        }

        /// <summary>
        /// Gets the size of the original image.
        /// </summary>
        /// <value>
        /// The size of the original image.
        /// </value>
        public Size OriginalSize { get; private set; }

        /// <summary>
        /// Gets the buffer as an array.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBufferAsArray()
        {
            return _buffer;
        }
    }
}
