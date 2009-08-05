AppAir.Web.Routing
==================

Currently only contains StrongRoute which is an example of how to have strongly typed routes.
There is some Html and Url helper extensions that go with these but they will be added later as apart of AppAir.Web

Usage
-----
    
    
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