namespace MultiSAAS.Data
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.Data.Entity;
  using System.Data.Entity.ModelConfiguration.Conventions;
  using System.Linq;
  using Entity;

  [DbConfigurationType(typeof(DbConfiguration))]
  public class TenantContext : System.Data.Entity.DbContext
  {
    public const string DefaultTenantCode = Constants.Default.TenantCode;
    public const string DefaultUsername = Constants.Default.Username;

    public TenantContext() : this("name=" + DefaultTenantCode)
    {
    }

    public TenantContext(string name) : base(name)
    {
      Username = DefaultUsername;
      this.ReadAllDateTimeValuesAsUtc();
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Tenant> Tenants { get; set; }

    public string Username { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      // singularize table names
      modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

      // default strings maxlength
      modelBuilder
        .Properties()
        .Where(x =>
          x.PropertyType.FullName.Equals("System.String")
          &&
          !x.GetCustomAttributes(false).OfType<StringLengthAttribute>().Any()
          &&
          !x.GetCustomAttributes(false).OfType<MaxLengthAttribute>().Any())
        .Configure(c => c.HasMaxLength(Constants.StringLength.Default));

      base.OnModelCreating(modelBuilder);

      // see: complex types for extended audit trail
      // https://www.exceptionnotfound.net/entity-change-tracking-using-dbcontext-in-entity-framework-6/
    }

    public override int SaveChanges()
    {
      var now = DateTime.UtcNow;
      foreach (var e in ChangeTracker.Entries<AuditableEntity>())
      {
        if (e.State == EntityState.Added)
        {
          e.Entity.CreatedBy = Username;
          e.Entity.CreatedDT = now;
        }
        e.Entity.LastChangedBy = Username;
        e.Entity.LastChangedDT = now;
      }
      return base.SaveChanges();
    }
  }
}