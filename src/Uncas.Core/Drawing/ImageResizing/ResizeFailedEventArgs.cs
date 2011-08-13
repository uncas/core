namespace Uncas.Core.Drawing.ImageResizing
{
    using System;

    /// <summary>
    /// Contains event arguments when resize failed.
    /// </summary>
    public class ResizeFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeFailedEventArgs"/> class.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public ResizeFailedEventArgs(Exception ex)
        {
            Exception = ex;
        }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }
    }
}