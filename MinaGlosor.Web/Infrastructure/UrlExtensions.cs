using System;
using System.Web.Mvc;

namespace MinaGlosor.Web.Infrastructure
{
    public static class UrlExtensions
    {
        public static string ContentCacheBreak(this UrlHelper url, string contentPath)
        {
            if (contentPath == null) throw new ArgumentNullException("contentPath");
            var path = url.Content(contentPath);
            path += "?" + Application.GetAppVersion();
            return path;
        }
    }
}