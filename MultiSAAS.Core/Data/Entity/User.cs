namespace MultiSAAS.Data.Entity
{
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;

  public class User : AuditableEntity
  {
    public User()
    {
      Enabled = true;
    }

    [StringLength(Constants.StringLength.Username), Required, Key]
    public string Username { get; set; }

    [StringLength(32)]
    public string Password { get; set; }

    [StringLength(25), Required]
    public string FirstName { get; set; }

    [StringLength(25), Required]
    public string LastName { get; set; }

    [StringLength(50), DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; }

    [StringLength(Constants.StringLength.TenantCode)]
    public string ExternalTenantCode { get; set; }

    [ForeignKey("ExternalTenantCode")]
    public virtual Tenant ExternalTenant { get; set; }

    [StringLength(Constants.StringLength.Username)]
    public string ExternalUsername { get; set; }

    public bool Enabled { get; set; }
  }
}