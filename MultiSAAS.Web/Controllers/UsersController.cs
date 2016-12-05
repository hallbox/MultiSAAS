namespace MultiSAAS.Web.Controllers
{
  using System.Linq;
  using System.Net;
  using System.Threading.Tasks;
  using System.Web.Mvc;
  using AutoMapper;
  using Data.Entities;
  using Framework.Controllers;
  using Framework.Helpers;
  using ViewModels;
  using Data;

  public class UsersController : MultiTenantController
  {
    private UserData _repo;
    private TenantData _tenantRepo;

    public UsersController(UserData repo, TenantData tenantRepo)
    {
      _repo = repo;
      _tenantRepo = tenantRepo;
    }

    public ActionResult Index()
    {
      ViewBag.Fields = "Username,FirstName,LastName,EmailAddress,Enabled";
      return Grid(new UserViewModel());
    }

    // JSON results for filtered grid
    public async Task<JsonResult> Json(string username, string firstName, string lastName, string emailAddress, bool? enabled)
    {
      var results = await _repo.ProjectToListAsync<UserViewModel>(username, firstName, lastName, emailAddress, enabled);
      return Json(results, JsonRequestBehavior.AllowGet);
    }

    // Default action result for forms.
    private ActionResult Form(string id = null)
    {
      var model = string.IsNullOrEmpty(id)
        ? new UserViewModel() { ExternalTenant = new Framework.SelectList() }
        : _repo.ProjectToList<UserViewModel>(id).Single();
      if (model != null)
      {
        model.ExternalTenant = _tenantRepo.ProjectToList<TenantViewModel>().ToSelectList(t => t.TenantCode, t => t.TenantName, model.ExternalTenant.SelectedValue, "");
      }
      return FormActionResult(model, id);
    }

    // GET: Users/5/Details
    public ActionResult Details(string id)
    {
      return Form(id);
    }

    // GET: Users/Create
    public ActionResult Create()
    {
      return Form();
    }

    // POST: Users/Create
    [HttpPost, ValidateAntiForgeryToken]
    public ActionResult Create(
      [Bind(Include = "Username,Password,FirstName,LastName,EmailAddress,ExternalTenant,ExternalUsername,Enabled")]
      UserViewModel vm)
    {
      if (ModelState.IsValid)
      {
        var entity = new User();
        Mapper.Map(vm, entity);
        _repo.Add(entity);
        return RedirectToAction("Index");
      }
      return Create();
    }

    // GET: Users/5/Edit
    public ActionResult Edit(string id)
    {
      return Form(id);
    }

    // POST: Users/5/Edit
    [HttpPost, ValidateAntiForgeryToken]
    public ActionResult Edit(
      [Bind(Include = "Username,Password,FirstName,LastName,EmailAddress,ExternalTenant,ExternalUsername,Enabled")]
      UserViewModel vm)
    {
      if (ModelState.IsValid)
      {
        User entity = _repo.Single(vm.Username);
        Mapper.Map(vm, entity);
        _repo.Update(entity);
        return RedirectToAction("Index");
      }
      return Edit(vm.Username);
    }

    // GET: Users/5/Delete
    public ActionResult Delete(string id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      return Form(id);
    }

    // POST: Users/5/Delete
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(string id)
    {
      _repo.Remove(id);
      return RedirectToAction("Index");
    }
  }
}