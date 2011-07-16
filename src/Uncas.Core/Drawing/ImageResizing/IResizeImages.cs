using System;
using System.Collections;

namespace Uncas.Core.Drawing.ImageResizing
{
    /// <summary>
    /// Resizes images.
    /// </summary>
    public interface IResizeImages
    {
        /// <summary>
        /// Occurs when resize completed.
        /// </summary>
        event EventHandler<ResizeCompletedEventArgs> ResizeCompleted;

        /// <summary>
        /// Occurs when resize failed.
        /// </summary>
        event EventHandler<ResizeFailedEventArgs> ResizeFailed;

        /// <summary>
        /// Occurs when resize progress changed.
        /// </summary>
        event EventHandler<ResizeProgressEventArgs> ResizeProgressChanged;

        /// <summary>
        /// Cancels the resize work.
        /// </summary>
        void CancelResizeWork();

        /// <summary>
        /// Does the resize work async.
        /// </summary>
        /// <param name="baseOutputFolder">The base output folder.</param>
        /// <param name="maxImageSize">Maximum size of images.</param>
        /// <param name="chooseFiles">If set to <c>true</c> [choose files].</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="chooseFolder">If set to <c>true</c> [choose folder].</param>
        /// <param name="baseInputFolder">The base input folder.</param>
        /// <param name="includeSubfolders">If set to <c>true</c> [include sub folders].</param>
        void DoResizeWorkAsync(
            string baseOutputFolder, 
            int maxImageSize, 
            bool chooseFiles, 
            IEnumerable filePaths, 
            bool chooseFolder, 
            string baseInputFolder, 
            bool includeSubfolders);
    }
}
