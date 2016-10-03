namespace MultiSAAS.Migrations
{
  using System.Data.Entity.Migrations;
  using System.Data.Entity.Validation;
  using System.Text;
  using MultiSAAS.Data;
  using MultiSAAS.Data.Entity;

  internal sealed class Configuration : DbMigrationsConfiguration<TenantContext>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = false;
    }

    protected override void Seed(TenantContext context)
    {
      if (System.Diagnostics.Debugger.IsAttached == false)
      {
        System.Diagnostics.Debugger.Launch();
      }

      context.SaveChanges();
    }

    // http://www.blaiseliu.com/got-entityvalidationerrors-debug-into-entity-framework-code-first/
    private static void SaveChanges(TenantContext context)
    {
      try
      {
        context.SaveChanges();
      }
      catch (DbEntityValidationException ex)
      {
        var sb = new StringBuilder();
        foreach (var failure in ex.EntityValidationErrors)
        {
          sb.AppendFormat("{0} failed validation\n", failure.Entry.Entity.GetType());
          foreach (var error in failure.ValidationErrors)
          {
            sb.AppendFormat("- {0} : {1}", error.PropertyName, error.ErrorMessage);
            sb.AppendLine();
          }
        }
        throw new DbEntityValidationException(
          "Entity Validation Failed - errors follow:\n" +
          sb, ex);
      }
    }
  }
}