namespace MultiSAAS.Web.Framework.Helpers
{
  using System;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Web;
  using System.Web.Mvc;
  using System.Web.Mvc.Html;
  using System.Web.Routing;

  public static class BootstrapHelpers
  {
    public const string LabelCssClass = "col-md-2 control-label";

    public static IHtmlString BootstrapLabelFor<TModel, TProp>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProp>> property)
    {
      return helper.LabelFor(property, new
      {
        @class = LabelCssClass
      });
    }

    public static IHtmlString BootstrapLabel(this HtmlHelper helper, string propertyName)
    {
      return helper.Label(propertyName, new
      {
        @class = LabelCssClass
      });
    }

    public static string IsActive(this HtmlHelper html, string controllers = "", string actions = "", string cssClass = "active")
    {
      ViewContext viewContext = html.ViewContext;
      bool isChildAction = viewContext.Controller.ControllerContext.IsChildAction;

      if (isChildAction)
        viewContext = html.ViewContext.ParentActionViewContext;

      RouteValueDictionary routeValues = viewContext.RouteData.Values;
      string currentAction = routeValues["action"].ToString();
      string currentController = routeValues["controller"].ToString();

      if (String.IsNullOrEmpty(actions))
        actions = currentAction;

      if (String.IsNullOrEmpty(controllers))
        controllers = currentController;

      string[] acceptedActions = actions.Trim().Split(',').Distinct().ToArray();
      string[] acceptedControllers = controllers.Trim().Split(',').Distinct().ToArray();

      return acceptedActions.Contains(currentAction) && acceptedControllers.Contains(currentController) ?
          cssClass : String.Empty;
    }
  }
}