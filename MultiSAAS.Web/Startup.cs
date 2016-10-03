namespace MultiSAAS.Web
{
  using Microsoft.AspNet.Identity;
  using Microsoft.Owin.Security.Cookies;
  using Owin;

  // [assembly: OwinStartup(typeof(MultiSAAS.Web.Startup))]

  // http://benfoster.io/blog/aspnet-identity-stripped-bare-mvc-part-1
  // http://weblog.west-wind.com/posts/2015/Apr/29/Adding-minimal-OWIN-Identity-Authentication-to-an-Existing-ASPNET-MVC-Application

  public class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      ConfigureAuthentication(app);
    }

    // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
    public void ConfigureAuthentication(IAppBuilder app)
    {
      // Enable the application to use a cookie to store information for the signed in user
      app.UseCookieAuthentication(new CookieAuthenticationOptions
      {
        AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie
        ////LoginPath = new PathString("/Login")
      });
    }
  }
}