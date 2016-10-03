namespace MultiSAAS.Web.Controllers
{
  using System.Data.Entity;
  using System.Linq;
  using System.Threading.Tasks;
  using System.Web.Mvc;
  using AutoMapper;
  using AutoMapper.QueryableExtensions;
  using Data.Entity;
  using Framework.Controllers;
  using ViewModels;

  public class TenantsController : MultiTenantController
  {
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
        db.Tenants.Add(entity);
        db.SaveChanges();
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
        var entity = db.Tenants.Find(vm.TenantCode);
        Mapper.Map(vm, entity);
        db.Entry(entity).State = EntityState.Modified;
        db.SaveChanges();
        return RedirectToAction("Index");
      }
      return Edit(vm.TenantCode);
    }

    // POST: Tenants/5/Delete
    [HttpPost, ValidateAntiForgeryToken, ActionName("Delete")]
    public ActionResult DeleteConfirmed(string id)
    {
      var entity = db.Tenants.Find(id);
      db.Tenants.Remove(entity);
      db.SaveChanges();
      return RedirectToAction("Index");
    }

    #endregion

    // Default action result for forms.
    private ActionResult Form(string id = null)
    {
      var model = string.IsNullOrEmpty(id)
        ? new TenantViewModel()
        : db.Tenants.Where(u => u.TenantCode == id).ProjectTo<TenantViewModel>().First();
      return FormActionResult(model, id);
    }

    // JSON results for filtered grid
    public async Task<JsonResult> Json(string tenantCode, string tenantName, bool? allowLogin)
    {
      var results = await db.Tenants
        .Where(u =>
          (string.IsNullOrEmpty(tenantCode) || u.TenantCode.StartsWith(tenantCode)) &&
          (string.IsNullOrEmpty(tenantName) || u.TenantName.StartsWith(tenantName)) &&
          (allowLogin == null || u.AllowLogin == allowLogin)
        )
        .ProjectTo<TenantViewModel>()
        .ToListAsync();
      return Json(results, JsonRequestBehavior.AllowGet);
    }
  }
}