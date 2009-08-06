using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppAir.Web
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class HtmlLinkToExtensions
    {
        public static string LinkTo(this HtmlHelper htmlHelper, string linkText, Route route)
        {
            return LinkTo(htmlHelper, linkText, route, null, null);
        }

        public static string LinkTo(this HtmlHelper htmlHelper, string linkText, Route route, RouteValueDictionary routeValues)
        {
            return LinkTo(htmlHelper, linkText, route, routeValues, null);
        }

        public static string LinkTo(this HtmlHelper htmlHelper, string linkText, Route route, object routeValues)
        {
            return LinkTo(htmlHelper, linkText, route, new RouteValueDictionary(routeValues), null);
        }

        public static string LinkTo(this HtmlHelper htmlHelper, string linkText, Route route, object routeValues, RouteValueDictionary attributes)
        {
            return LinkTo(htmlHelper, linkText, route, new RouteValueDictionary(routeValues), attributes);
        }

        public static string LinkTo(this HtmlHelper htmlHelper, string linkText, Route route, RouteValueDictionary routeValues, object attributes)
        {
            return LinkTo(htmlHelper, linkText, route, routeValues, new RouteValueDictionary(attributes));
        }

        public static string LinkTo(this HtmlHelper htmlHelper, string linkText, Route route, RouteValueDictionary routeValues, RouteValueDictionary attributes)
        {
            var url = GenerateUrl(htmlHelper, route, routeValues);
            var aTag = new TagBuilder("a");
            aTag.Attributes.Add("href", url);
            aTag.InnerHtml = linkText;
            if (attributes != null) aTag.MergeAttributes(attributes);
            return aTag.ToString(TagRenderMode.Normal);
        }

        private static string GenerateUrl(HtmlHelper htmlHelper, Route route, RouteValueDictionary routeValues)
        {
            //TODO: remove code dupe see UrlLinkToExtensions
            var requestCtx = htmlHelper.ViewContext.RequestContext;
            var httpCtx = requestCtx.HttpContext;

            if (routeValues != null)
            {
                foreach (var d in route.Defaults)
                {
                    if (!routeValues.ContainsKey(d.Key))
                        routeValues.Add(d.Key, d.Value);
                }
            }
            else
            {
                routeValues = route.Defaults;
            }

            VirtualPathData vpd = route.GetVirtualPath(requestCtx, routeValues);
            if (vpd == null)
                return null;

            return httpCtx.Request.ApplicationPath + vpd.VirtualPath;
        }
    }
}
