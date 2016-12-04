namespace MultiSAAS.Data
{
  using System;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.DataAnnotations.Schema;
  using System.Linq;

  public abstract class AuditableEntity
  {
    [StringLength(Constants.StringLength.Username), Required]
    public string CreatedBy { get; set; }

    [Required]
    public DateTime CreatedDT { get; set; }

    [StringLength(Constants.StringLength.Username), Required]
    public string LastChangedBy { get; set; }

    [Required]
    public DateTime? LastChangedDT { get; set; }

    [Timestamp]
    public byte[] Timestamp { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public ulong TimestampNum => BitConverter.ToUInt64(Timestamp.Reverse().ToArray(), 0);
  }
}