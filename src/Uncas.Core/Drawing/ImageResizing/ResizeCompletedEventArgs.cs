using System;

namespace Uncas.Core.Drawing.ImageResizing
{
    /// <summary>
    /// Contains event arguments when resize completed.
    /// </summary>
    public class ResizeCompletedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="canceled">If set to <c>true</c> [canceled].</param>
        /// <param name="processedImages">The processed images.</param>
        public ResizeCompletedEventArgs(
            bool canceled,
            ProcessedImagesInfo processedImages)
        {
            Canceled = canceled;
            ProcessedImages = processedImages;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ResizeCompletedEventArgs"/> is canceled.
        /// </summary>
        /// <value><c>true</c> if canceled; otherwise, <c>false</c>.</value>
        public bool Canceled { get; set; }

        /// <summary>
        /// Gets or sets the processed images.
        /// </summary>
        /// <value>The processed images.</value>
        public ProcessedImagesInfo ProcessedImages { get; set; }
    }
}