namespace MultiSAAS.Web.Controllers
{
  using System.Data.Entity;
  using System.Linq;
  using System.Net;
  using System.Threading.Tasks;
  using System.Web.Mvc;
  using AutoMapper;
  using AutoMapper.QueryableExtensions;
  using Data.Entity;
  using Framework.Controllers;
  using Framework.Helpers;
  using ViewModels;

  public class UsersController : MultiTenantController
  {
    public ActionResult Index()
    {
      ViewBag.Fields = "Username,FirstName,LastName,EmailAddress,Enabled";
      return Grid(new UserViewModel());
    }

    // JSON results for filtered grid
    public async Task<JsonResult> Json(string username, string firstName, string lastName, string emailAddress, bool? enabled)
    {
      var results = await db.Users
        .Include(u => u.ExternalTenant)
        .Where(u =>
          (string.IsNullOrEmpty(username) || u.Username.StartsWith(username)) &&
          (string.IsNullOrEmpty(firstName) || u.FirstName.StartsWith(firstName)) &&
          (string.IsNullOrEmpty(lastName) || u.LastName.StartsWith(lastName)) &&
          (string.IsNullOrEmpty(emailAddress) || u.EmailAddress.StartsWith(emailAddress)) &&
          (enabled == null || u.Enabled == enabled)
        )
        .ProjectTo<UserViewModel>()
        .ToListAsync();
      return Json(results, JsonRequestBehavior.AllowGet);
    }

    // Default action result for forms.
    private ActionResult Form(string id = null)
    {
      var model = string.IsNullOrEmpty(id)
        ? new UserViewModel() { ExternalTenant = new Framework.SelectList() }
        : db.Users.Where(u => u.Username == id).ProjectTo<UserViewModel>().FirstOrDefault();
      if (model != null)
      {
        model.ExternalTenant = db.Tenants.ToSelectList(t => t.TenantCode, t => t.TenantName, model.ExternalTenant.SelectedValue, "");
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
        db.Users.Add(entity);
        db.SaveChanges();
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
        User entity = db.Users.Find(vm.Username);
        Mapper.Map(vm, entity);
        if (string.IsNullOrEmpty(vm.Password))
        {
          db.Entry(entity).Property("Password").IsModified = false;
        }
        db.Entry(entity).State = EntityState.Modified;
        db.SaveChanges();
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
      var entity = db.Users.Find(id);
      db.Users.Remove(entity);
      db.SaveChanges();
      return RedirectToAction("Index");
    }
  }
}