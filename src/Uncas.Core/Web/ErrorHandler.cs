namespace Uncas.Core.Web
{
    using System;
    using System.Web;

    /// <summary>
    /// Web error handler.
    /// </summary>
    public static class ErrorHandler
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns>The status code.</returns>
        /// <remarks>
        /// See also http://www.digitallycreated.net/Blog/57/getting-the-correct-http-status-codes-out-of-asp.net-custom-error-pages.
        /// </remarks>
        public static int GetStatusCode(Exception exception)
        {
            var httpException = exception as HttpException;
            if (httpException == null)
            {
                return 500;
            }

            return httpException.GetHttpCode();
        }
    }
}