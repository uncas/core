namespace Uncas.Core.Web
{
    using System;
    using System.Web;
    using System.Globalization;
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Site URL logic.
    /// </summary>
    public static class SiteUrl
    {
        /// <summary>
        /// Gets the website base URL.
        /// </summary>
        public static Uri BaseUrl
        {
            get
            {
                var urlString =
                    string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}://{1}{2}"
                    , HttpContext.Current.Request.Url.Scheme
                    , HttpContext.Current.Request.Url.Authority
                    , HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
                return new Uri(urlString);
            }
        }

        /// <summary>
        /// Combines the virtual paths.
        /// </summary>
        /// <param name="virtualPath1">The 1st virtual path.</param>
        /// <param name="virtualPath2">The 2nd virtual path.</param>
        /// <returns></returns>
        public static string CombineVirtualPaths(
            string virtualPath1
            , string virtualPath2)
        {
            if (string.IsNullOrEmpty(virtualPath1))
            {
                throw new ArgumentNullException("virtualPath1");
            }

            string effective2 = virtualPath2 != null ? virtualPath2 : string.Empty;

            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}/{1}",
                virtualPath1.Trim('~', '/')
                , effective2.Trim('/'));
        }

        /// <summary>
        /// Gets the absolute URL.
        /// </summary>
        /// <param name="virtualPath">The virtual path.</param>
        /// <returns></returns>
        public static Uri GetAbsoluteUrl(string virtualPath)
        {
            string urlString =
                CombineVirtualPaths(
                BaseUrl.AbsoluteUri,
                UrlEscape(virtualPath));
            return new Uri(urlString);
        }

        /// <summary>
        /// URLs the escape.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static string UrlEscape(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            else
                return text
                    .Replace(" ", "%20")
                    .Replace("æ", "%C3%A6")
                    .Replace("ø", "%C3%B8")
                    .Replace("å", "%C3%A5")
                    .Replace("Æ", "%C3%86")
                    .Replace("Ø", "%C3%98")
                    .Replace("Å", "%C3%85")
                    ;
        }
    }
}