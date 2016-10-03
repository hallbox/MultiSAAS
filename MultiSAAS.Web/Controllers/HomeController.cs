namespace MultiSAAS.Web.Controllers
{
  using System.Web.Mvc;
  using Framework.Controllers;

  public class HomeController : MultiTenantController
  {
    public ActionResult Index()
    {
      return View();
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your application description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }
  }
}