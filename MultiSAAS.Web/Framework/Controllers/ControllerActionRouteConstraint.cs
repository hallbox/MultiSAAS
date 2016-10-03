namespace MultiSAAS.Web.Framework.Controllers
{
  using System.Linq;
  using System.Reflection;
  using System.Web;
  using System.Web.Routing;

  public class ControllerActionRouteConstraint : IRouteConstraint
  {
    public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values,
      RouteDirection routeDirection)
    {
      // make sure at least one controller/action combination matches
      return Assembly.GetExecutingAssembly().GetTypes()
        .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
        .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
        .Where(
          m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any()
            &&
            m.DeclaringType?.Name.ToLower() == values["controller"].ToString().ToLower() + "controller"
            &&
            m.Name.ToLower() == values["action"].ToString().ToLower()
        )
        .ToList().Count > 0;
    }
  }
}