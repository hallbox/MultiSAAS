namespace MultiSAAS.Data.Entities
{
  using System.ComponentModel.DataAnnotations;

  public class Tenant : AuditableEntity
  {
    [StringLength(Constants.StringLength.TenantCode), Required, Key]
    public string TenantCode { get; set; }

    [Required]
    public string TenantName { get; set; }

    public bool AllowLogin { get; set; }

    [StringLength(1000)]
    public string ConnectionString { get; set; }

    [MaxLength]
    public string LogoImageData { get; set; }
  }
}