namespace MultiSAAS.Web.Framework.Controllers
{
  using System.Linq;
  using System.Security.Claims;
  using System.Web;
  using System.Web.Mvc;
  using System.Web.Routing;
  using Microsoft.AspNet.Identity;

  public class AuthorizeAttribute : System.Web.Mvc.AuthorizeAttribute
  {
    public string TenantCode { get; set; }

    protected override bool AuthorizeCore(HttpContextBase httpContext)
    {
      var ctx = httpContext;
      var routeData = ((MvcHandler) httpContext.Handler).RequestContext.RouteData;
      TenantCode = (string) routeData.Values["tenant"] ?? string.Empty;
      var user = (ClaimsPrincipal) ctx.User;
      if (user.Claims != null && !user.Claims.Any(c => c.Type == ClaimTypes.System && c.Value == TenantCode))
      {
        return false;
      }
      return base.AuthorizeCore(httpContext);
    }

    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
    {
      base.HandleUnauthorizedRequest(filterContext);
      // base must be called first so that we can override the login url below
      filterContext.HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
      filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
      {
        { "tenant", TenantCode },
        { "controller", "Authentication" },
        { "action", "Login" },
        { "ReturnUrl", filterContext.HttpContext.Request.RawUrl }
      });
    }
  }
}