namespace MultiSAAS.Web.Controllers
{
  using System.Linq;
  using System.Threading.Tasks;
  using System.Web.Mvc;
  using AutoMapper;
  using Data.Entities;
  using Framework.Controllers;
  using ViewModels;
  using Data;

  public class TenantsController : MultiTenantController
  {
    private TenantData _repo;

    public TenantsController(TenantData repo)
    {
      _repo = repo;
    }

    #region GET

    // GET: Tenants
    public ActionResult Index()
    {
      ViewBag.Fields = "TenantCode,TenantName,AllowLogin";
      return Grid(new TenantViewModel());
    }

    // GET: Tenants/5/Details
    public ActionResult Details(string id)
    {
      return Form(id);
    }

    // GET: Tenants/Create
    public ActionResult Create()
    {
      return Form();
    }

    // GET: Tenants/5/Edit
    public ActionResult Edit(string id)
    {
      return Form(id);
    }

    // GET: Tenants/5/Delete
    public ActionResult Delete(string id)
    {
      return Form(id);
    }

    #endregion

    #region POST

    // POST: Tenants/Create
    [HttpPost, ValidateAntiForgeryToken]
    public ActionResult Create(TenantViewModel vm)
    {
      if (ModelState.IsValid)
      {
        var entity = new Tenant();
        Mapper.Map(vm, entity);
        _repo.Add(entity);
        return RedirectToAction("Index");
      }
      return Create();
    }

    // POST: Tenants/5/Edit
    [HttpPost, ValidateAntiForgeryToken]
    public ActionResult Edit(TenantViewModel vm)
    {
      if (ModelState.IsValid)
      {
        var entity = _repo.Single(vm.TenantCode);
        Mapper.Map(vm, entity);
        _repo.Update(entity);
        return RedirectToAction("Index");
      }
      return Edit(vm.TenantCode);
    }

    // POST: Tenants/5/Delete
    [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
    public ActionResult DeleteConfirmed(string id)
    {
      _repo.Remove(id);
      return RedirectToAction("Index");
    }

    #endregion

    // Default action result for forms.
    private ActionResult Form(string id = null)
    {
      var model = string.IsNullOrEmpty(id)
        ? new TenantViewModel()
        : _repo.ProjectToList<TenantViewModel>(id).Single();
      return FormActionResult(model, id);
    }

    // JSON results for filtered grid
    public async Task<JsonResult> Json(string tenantCode, string tenantName, bool? allowLogin)
    {
      var results = await _repo.ProjectToListAsync<TenantViewModel>(tenantCode, tenantName, allowLogin);
      return Json(results, JsonRequestBehavior.AllowGet);
    }
  }
}