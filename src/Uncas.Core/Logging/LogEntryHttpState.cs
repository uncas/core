namespace Uncas.Core.Logging
{
    using System;
    using System.Web;

    /// <summary>
    /// Represents the HTTP state of a log entry.
    /// </summary>
    public class LogEntryHttpState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntryHttpState"/> class.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        internal LogEntryHttpState(
            HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("httpContext");
            }

            HttpRequest request = httpContext.Request;
            if (request == null)
            {
                throw new ArgumentException(
                    "The HTTP context must contain a Request object with a value.",
                    "httpContext");
            }

            HttpResponse response = httpContext.Response;
            if (response == null)
            {
                throw new ArgumentException(
                    "The HTTP context must contain a Response object with a value.",
                    "httpContext");
            }

            Url = request.Url;

            if (request.Headers != null &&
                request.Headers.Count > 0)
            {
                // TODO: Set headers properly:
                Headers = request.Headers[0];
            }

            UserHostAddress = request.UserHostAddress;

            if (httpContext.User != null && httpContext.User.Identity != null)
            {
                UserName = httpContext.User.Identity.Name;
            }

            if (request.UrlReferrer != null)
            {
                Referrer = request.UrlReferrer.AbsoluteUri;
            }

            StatusCode = response.StatusCode;
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The url of the HTTP request.</value>
        public Uri Url { get; private set; }

        /// <summary>
        /// Gets the referrer.
        /// </summary>
        /// <value>The referrer of the HTTP request, if any.</value>
        public string Referrer { get; private set; }

        /// <summary>
        /// Gets the HTTP headers.
        /// </summary>
        /// <value>The HTTP headers of the HTTP request.</value>
        public string Headers { get; private set; }

        /// <summary>
        /// Gets the user host address.
        /// </summary>
        /// <value>The user host address.</value>
        public string UserHostAddress { get; private set; }

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>The status code of the HTTP request.</value>
        public int StatusCode { get; private set; }
    }
}