using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AppAir.Web.Routing
{
    [AspNetHostingPermission(System.Security.Permissions.SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class StrongRoute : Route
    {
        public StrongRoute()
            : base(null, new MvcRouteHandler())
        {
            Constraints = new RouteValueDictionary();
            Defaults = new RouteValueDictionary();
        }

        public string Key { get; set; }

        public static StrongRoute Map<TController>(string url, Expression<Func<TController, ActionResult>> action)
            where TController : Controller
        {
            return Map(url, action, false);
        }

        public static StrongRoute Map<TController>(string url, Expression<Func<TController, ActionResult>> action, bool useDefaults)
            where TController : Controller
        {
            if (action == null)
                throw new ArgumentException("Action must be a method call");

            var body = action.Body as MethodCallExpression;

            if (body == null)
                throw new ArgumentException("Expression must be a method call");

            if (body.Object != action.Parameters[0])
                throw new ArgumentException("Method call must target lambda argument");

            var route = new StrongRoute { Constraints = null };
            var actionName = body.Method.Name;
            var attributes = body.Method.GetCustomAttributes(typeof(ActionNameAttribute), false);
            var controllerName = typeof(TController).Name;

            if (attributes.Length > 0)
                actionName = ((ActionNameAttribute)attributes[0]).Name;

            if (controllerName.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                controllerName = controllerName.Remove(controllerName.Length - 10, 10);

            route.Defaults.Add("controller", controllerName);
            route.Defaults.Add("action", actionName);
            route.Url = url;
            route.Key = controllerName + ":" + actionName;

            if (!useDefaults) return route;

            for (int i = 0; i < body.Arguments.Count; i++)
            {
                var name = body.Method.GetParameters()[i].Name;
                var value = ((ConstantExpression)body.Arguments[i]).Value;
                if (value != null)
                    route.Defaults.Add(name, value);
            }

            return route;
        }

        public void Register(RouteCollection routes)
        {
            routes.Add(this.Key, this);
        }
    }
}
