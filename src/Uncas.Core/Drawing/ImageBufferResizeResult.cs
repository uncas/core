namespace Uncas.Core.Drawing
{
    using System.Collections.Generic;
    using System.Drawing;

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