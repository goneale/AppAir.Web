using System;
using System.Web;

namespace AppAir.Web.Routing
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RouteOrderAttribute : Attribute
    {
        public RouteOrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; set; }
    }
}