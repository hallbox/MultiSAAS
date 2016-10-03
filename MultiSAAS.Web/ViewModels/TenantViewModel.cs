namespace MultiSAAS.Web.ViewModels
{
  using AutoMapper;
  using Heroic.AutoMapper;
  using Data.Entity;
  using Framework;

  public class TenantViewModel : MappedViewModel, IHaveCustomMappings
  {
    public string TenantCode { get; set; }
    public string TenantName { get; set; }
    public bool AllowLogin { get; set; }
    public string ConnectionString { get; set; }

    public void CreateMappings(IMapperConfiguration configuration)
    {
      configuration.CreateMap<Tenant, TenantViewModel>()
        .ForMember(m => m.AuditHistory, opt => opt.MapFrom(e => new AuditModel() { AuditableEntity = e }));
      configuration.CreateMap<TenantViewModel, Tenant>();
    }
  }
}