using System;

namespace Uncas.Core.Drawing.ImageResizing
{
    /// <summary>
    /// Contains event arguments for the resize progress.
    /// </summary>
    public class ResizeProgressEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeProgressEventArgs"/> class.
        /// </summary>
        /// <param name="processedImagesInfo">Info about the processed images.</param>
        public ResizeProgressEventArgs(ProcessedImagesInfo processedImagesInfo)
        {
            ProcessedImages = processedImagesInfo;
        }

        /// <summary>
        /// Gets or sets the processed images.
        /// </summary>
        /// <value>The processed images.</value>
        public ProcessedImagesInfo ProcessedImages { get; set; }
    }
}