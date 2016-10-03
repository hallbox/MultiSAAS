namespace MultiSAAS.Web.ViewModels
{
  using AutoMapper;
  using Heroic.AutoMapper;
  using Data.Entity;
  using Extensions;
  using Framework;
  using Framework.Extensions;

  public class UserViewModel : MappedViewModel, IHaveCustomMappings
  {
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public SelectList ExternalTenant { get; set; }
    public string ExternalUsername { get; set; }
    public bool Enabled { get; set; } = true;

    public void CreateMappings(IMapperConfiguration configuration)
    {
      configuration.CreateMap<User, UserViewModel>()
        .ForMember(m => m.ExternalTenant, opt => opt.MapFrom(e => new SelectList() { SelectedValue = e.ExternalTenantCode }))
        .ForMember(m => m.AuditHistory, opt => opt.MapFrom(e => new AuditModel() { AuditableEntity = e }))
        .Ignore(m => m.Password);
      configuration.CreateMap<UserViewModel, User>()
        .ForMember(e => e.Password, opt => opt.MapFrom(m => !string.IsNullOrEmpty(m.Password) ? m.Password.Encrypt() : null))
        .ForMember(e => e.ExternalTenantCode, opt => opt.MapFrom(m => m.ExternalTenant.SelectedValue))
        .Ignore(e => e.ExternalTenant);
    }
  }
}