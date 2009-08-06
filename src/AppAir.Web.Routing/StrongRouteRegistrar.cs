using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppAir.Web.Routing
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public static class StrongRouteRegistrar
    {
        public static void MapFrom<TContainer>(RouteCollection routes)
            where TContainer : Controller
        {
            MapFrom(typeof(TContainer), routes);
        }

        public static void MapFrom(Type t, RouteCollection routes)
        {
            var discoveredRoutes = new SortedList<int, StrongRoute>();

            var fields = from type in t.GetMembers()
                         where
                             type.MemberType.Equals(MemberTypes.Field) &&
                             typeof(StrongRoute).IsAssignableFrom(((FieldInfo)type).FieldType)
                         select type;

            foreach (var field in fields)
            {
                var routeOrder = (RouteOrderAttribute)field.GetCustomAttributes(typeof (RouteOrderAttribute), false).FirstOrDefault();
                var sr = ((FieldInfo)field).GetValue(null) as StrongRoute;
                if (sr == null) continue;
                sr.Key = t.Name + ":" + field.Name + "_" + sr.Key;

                var order = int.MaxValue;
                if (routeOrder != null)
                {
                    order = routeOrder.Order;
                }
                else
                {
                    while (discoveredRoutes.ContainsKey(order))
                        order--;
                }


                discoveredRoutes.Add(order, sr);
            }

            foreach(var route in discoveredRoutes)
                route.Value.Register(routes);
        }
    }
}