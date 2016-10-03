namespace MultiSAAS.Web.Framework
{
  using System.Diagnostics;
  using System.Web.Mvc;
  using Controllers;

  public class MultiTenantViewEngine : RazorViewEngine
  {
    public MultiTenantViewEngine()
    {
      ViewLocationFormats = GetFormats();
      MasterLocationFormats = ViewLocationFormats;
      PartialViewLocationFormats = ViewLocationFormats;
    }

    private static string[] GetFormats(string prefix = "")
    {
      // "~/Views/%1/{1}/{0}.cshtml",
      // "~/Views/{1}/{0}.cshtml",
      // "~/Views/%1/Shared/{0}.cshtml",
      // "~/Views/Shared/{0}.cshtml"

      var pre = "~" + prefix + "/Views/";
      var post = "/{0}.cshtml";
      return new[]
      {
        pre + @"_Tenants/%1/{1}" + post,
        pre + @"{1}" + post,
        pre + @"_Tenants/%1/Shared" + post,
        pre + @"Shared" + post
      };
    }

    public override ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
    {
      if (string.IsNullOrEmpty(masterName))
      {
        masterName = "_Layout";
      }
      return base.FindView(controllerContext, viewName, masterName, useCache);
    }

    protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
    {
      var controller = controllerContext.Controller as MultiTenantController;
      Debug.Assert(controller != null, "CreateView controller != null");
      return base.CreateView(controllerContext, viewPath.Replace("%1", controller.TenantCode), masterPath.Replace("%1", controller.TenantCode));
    }

    protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
    {
      var controller = controllerContext.Controller as MultiTenantController;
      Debug.Assert(controller != null, "FileExists controller != null");
      return base.FileExists(controllerContext, virtualPath.Replace("%1", controller.TenantCode));
    }
  }
}