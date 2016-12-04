namespace MultiSAAS.Web.Controllers
{
  using System.Linq;
  using System.Security.Claims;
  using System.Web;
  using System.Web.Mvc;
  using Microsoft.AspNet.Identity;
  using Microsoft.Owin.Security;
  using Extensions;
  using Framework.Controllers;
  using ViewModels;
  using Data;

  // using AttributeRouting.Web.Mvc;

  [AllowAnonymous]
  public class AuthenticationController : MultiTenantController
  {
    private IAuthenticationManager AuthManager => HttpContext.GetOwinContext().Authentication;

    [AcceptVerbs(HttpVerbs.Get)]
    public ActionResult Login(string returnUrl)
    {
      var model = new LoginViewModel
      {
        ReturnUrl = returnUrl
      };
      return View("Form", model);
    }

    [AcceptVerbs(HttpVerbs.Post)]
    public ActionResult Login(LoginViewModel model)
    {
      if (!ModelState.IsValid)
      {
        return View("Form", new LoginViewModel());
      }

      var repo = new UserData();

      var encryptedPassword = model.Password.Encrypt();
      var user = repo.Authenticate(model.Username, model.Password);
      
      if (user != null)
      {
        var identity = new ClaimsIdentity(new[]
        {
          new Claim(ClaimTypes.NameIdentifier, TenantCode + "." + user.Username),
          new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
          new Claim(ClaimTypes.Email, user.EmailAddress),
          new Claim(ClaimTypes.GivenName, user.FirstName),
          new Claim(ClaimTypes.Surname, user.LastName),
          new Claim(ClaimTypes.System, TenantCode)
        },
          DefaultAuthenticationTypes.ApplicationCookie);

        AuthManager.SignIn(new AuthenticationProperties
        {
          IsPersistent = model.RememberMe
        }, identity);

        return Redirect(GetRedirectUrl(model.ReturnUrl));
      }

      ModelState.AddModelError("", "Invalid login.");
      return View("Form", new LoginViewModel());
    }

    private string GetRedirectUrl(string returnUrl)
    {
      if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
      {
        return Url.Action("index", "home");
      }

      return returnUrl;
    }

    [AcceptVerbs(HttpVerbs.Get)]
    public ActionResult Logout()
    {
      AuthManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
      return RedirectToAction("index", "home");
    }
  }
}