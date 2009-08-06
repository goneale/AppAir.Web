using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppAir.Web
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class UrlLinkToExtensions
    {
        public static string LinkTo(this UrlHelper urlHelper, Route route)
        {
            return LinkTo(urlHelper, route, null);
        }

        public static string LinkTo(this UrlHelper urlHelper, Route route, object routeValues)
        {
            return LinkTo(urlHelper, route, new RouteValueDictionary(routeValues));
        }

        public static string LinkTo(this UrlHelper urlHelper, Route route, RouteValueDictionary routeValues)
        {
            //TODO: remove code dupe see HtmlLinkToExtensions
            var requestCtx = urlHelper.RequestContext;
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