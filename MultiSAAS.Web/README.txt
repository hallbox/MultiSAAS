1. Layout the solution. 

		http://stackoverflow.com/a/1370464

		>> [Customer.]App (solution)
			>> [Customer.]App.Core (class library)
				+ Config
				> Domain
					> Model
					> Persistence
					> Queries
					> Services
				> Persistence
				> Services
			>> [Customer.]App.Web (asp.net mvc)
				+ Images
				+ Scripts
				+ Content
				+ Controllers
				+ Views
				+ Models

		LEGEND
		>  folder with namespace
		>> folder with project
 		+  folder 

2. Create App.Core class library project.  Delete Class1.cs.

3. Create App.Web ASP.NET MVC project using the No Authentication option.

4. Set As Startup for Web project.

5. Add Reference in Web to Core.

6. Manage NuGet Packages For Solution:

		Core project:

		EntityFramework

		Web project:

		Microsoft.Owin.Host.SystemWeb
		Microsoft.Owin.Security.Cookies
		Microsoft.AspNet.Identity.Owin

7. Web/Startup.cs based on:

	http://benfoster.io/blog/aspnet-identity-stripped-bare-mvc-part-1

	http://weblog.west-wind.com/posts/2015/Apr/29/Adding-minimal-OWIN-Identity-Authentication-to-an-Existing-ASPNET-MVC-Application

	http://blog.markjohnson.io/exorcising-entity-framework-from-asp-net-identity/

	http://www.asp.net/identity/overview/extensibility/overview-of-custom-storage-providers-for-aspnet-identity

8. App.config / Web.config (add to both for enable-migrations)

  <connectionStrings>
    <add name="rt" providerName="System.Data.SqlClient" connectionString="Server=MHALL;Database=RTD2;User Id=rtd2;Password=rtr0cks9" />
  </connectionStrings>


9. enable-migrations -ProjectName "RTD2.Core" -Force

