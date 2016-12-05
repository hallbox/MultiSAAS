namespace MultiSAAS.Web
{
  using System;
  using System.Security.Claims;
  using System.Web;
  using System.Web.Helpers;
  using System.Web.Mvc;
  using System.Web.Optimization;
  using System.Web.Routing;
  using Framework;
  using ModelMetadataProvider = Framework.ModelMetadata.ModelMetadataProvider;
  using Data;
  using Data.Entities;
  using Extensions;

  public class MvcApplication : HttpApplication
  {
    protected void Application_Start()
    {
      var settings = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration("~").AppSettings.Settings;

      Default.Username = settings["DefaultUsername"].Value;
      Default.TenantCode = settings["DefaultTenantCode"].Value;

      if (settings["DefaultUserPassword"] != null && settings["DefaultUserPassword"].Value.Length > 0)
      {
        var _tenantRepo = new TenantData();
        _tenantRepo.Add(new Tenant
        {
          TenantCode = Default.TenantCode,
          TenantName = settings["DefaultTenantName"].Value,
          AllowLogin = true
        }, true);

        var _userRepo = new UserData();
        _userRepo.Add(new User
        {
          Username = Default.Username,
          Password = settings["DefaultUserPassword"].Value.Encrypt(),
          FirstName = settings["DefaultUserFirstName"].Value,
          LastName = settings["DefaultUserLastName"].Value,
          EmailAddress = settings["DefaultUserEmailAddress"].Value,
          ExternalTenantCode = null,
          ExternalUsername = null
        }, true);
      }

      FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
      RouteConfig.RegisterRoutes(RouteTable.Routes);
      BundleConfig.RegisterBundles(BundleTable.Bundles);

      ViewEngines.Engines.Clear();
      ViewEngines.Engines.Add(new MultiTenantViewEngine());

      AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;

      ModelMetadataProviders.Current = new ModelMetadataProvider();
    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {
    }
  }
}