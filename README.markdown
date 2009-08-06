AppAir.Web
==================

Usage
-----

View

    <%=Html.LinkTo("Register", UserRoutes.Register)%>
    <a href="<%=Url.LinkTo(UserRoutes.Register)%>">Register</a>

Global.asax
  
    public class MvcApplication : System.Web.HttpApplication
    {
		public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			AppAir.Web.Routing.StrongRouteRegistrar.MapFrom(typeof(UserRoutes), routes);
        }
        
        protected void Application_Start()
        {
			RegisterRoutes(RouteTable.Routes);
		}
	}
	
	public static class UserRoutes
	{
		[RouteOrder(2)]
		public static readonly StrongRoute Register 
			= StrongRoute.Map<UserController>("register", c => c.Register());
		
		[RouteOrder(1)]
		public static readonly StrongRoute Logout 
			= StrongRoute.Map<UserController>("logout", c => c.Logout());
		
		public static readonly StrongRoute Login 
			= StrongRoute.Map<UserController>("login", c => c.Login());
	}

