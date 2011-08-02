namespace Uncas.Core.Web
{
    using System.Web;

    public static class SiteUrl
    {
        public static string WebSiteBaseUrl
        {
            get
            {
                return string.Format(
                    "{0}://{1}{2}"
                    , HttpContext.Current.Request.Url.Scheme
                    , HttpContext.Current.Request.Url.Authority
                    , HttpContext.Current.Request.ApplicationPath.TrimEnd('/'));
            }
        }

        public static string CombineVirtualPaths(
            string virtualPath1
            , string virtualPath2)
        {
            return string.Format(
                "{0}/{1}", 
                virtualPath1.Trim('~', '/')
                , virtualPath2.Trim('/'));
        }

        public static string GetAbsoluteUrl(string virtualPath)
        {
            return CombineVirtualPaths(WebSiteBaseUrl, UrlEscape(virtualPath));
        }

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