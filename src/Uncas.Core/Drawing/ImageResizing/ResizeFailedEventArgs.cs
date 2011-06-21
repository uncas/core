using System;

namespace Uncas.Core.Drawing.ImageResizing
{
    /// <summary>
    /// Contains event arguments when resize failed.
    /// </summary>
    public class ResizeFailedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResizeFailedEventArgs"/> class.
        /// </summary>
        /// <param name="ex">The ex.</param>
        public ResizeFailedEventArgs(Exception ex)
        {
            this.Exception = ex;
        }

        /// <summary>
        /// Gets or sets the exception.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }
    }
}