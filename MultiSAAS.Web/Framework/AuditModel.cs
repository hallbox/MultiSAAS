namespace MultiSAAS.Web.Framework
{
  using System;
  using Data;

  public class AuditModel
  {
    public DateTime? CreatedDT { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? LastChangedDT { get; set; }
    public string LastChangedBy { get; set; }
    public string Separator { get; set; } = " by ";
    public string Created => CreatedDT + (!string.IsNullOrEmpty(CreatedBy) ? Separator : null) + CreatedBy;
    public string Updated => LastChangedDT + (!string.IsNullOrEmpty(LastChangedBy) ? Separator : null) + LastChangedBy;
    public AuditableEntity AuditableEntity
    {
      set
      {
        CreatedDT = value.CreatedDT;
        CreatedBy = value.CreatedBy;
        LastChangedDT = value.LastChangedDT;
        LastChangedBy = value.LastChangedBy;
      }
    }
  }
}