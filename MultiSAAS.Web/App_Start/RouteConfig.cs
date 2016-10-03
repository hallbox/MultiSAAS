namespace MultiSAAS.Web
{
  using System.Web.Mvc;
  using System.Web.Routing;
  using Framework.Controllers;

  public class RouteConfig
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.MapRoute(
        "TenantLogin",
        "{tenant}/Login",
        new { controller = "Authentication", action = "Login" }
        );

      routes.MapRoute(
        "TenantLogout",
        "{tenant}/Logout",
        new { controller = "Authentication", action = "Logout" }
        );

      routes.MapRoute(
        "Login",
        "Login",
        new { controller = "Authentication", action = "Login" }
        );

      routes.MapRoute(
        "Logout",
        "Logout",
        new { controller = "Authentication", action = "Logout" }
        );

      routes.MapMvcAttributeRoutes();

      var validRoute = new ControllerActionRouteConstraint();

      routes.MapRoute(
        "TenantControllerAdd",
        "{tenant}/{controller}/Add",
        new { action = "Create" },
        new { valid = validRoute }
        );

      routes.MapRoute(
        "TenantControllerAction",
        "{tenant}/{controller}/{action}",
        new { action = "Index" },
        new { valid = validRoute }
        );

      routes.MapRoute(
        "TenantControllerIdAction",
        "{tenant}/{controller}/{id}/{action}",
        new { action = "Details" },
        new { valid = validRoute }
        );

      routes.MapRoute(
        "ControllerAdd",
        "{controller}/Add",
        new { action = "Create" },
        new { valid = validRoute }
        );

      routes.MapRoute(
        "ControllerAction",
        "{controller}/{action}",
        new { action = "Index" },
        new { valid = validRoute }
        );

      routes.MapRoute(
        "ControllerIdAction",
        "{controller}/{id}/{action}",
        new { action = "Details" },
        new { valid = validRoute }
        );
    }
  }
}