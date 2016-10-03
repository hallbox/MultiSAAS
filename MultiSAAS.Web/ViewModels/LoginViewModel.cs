namespace MultiSAAS.Web.ViewModels
{
  using System.ComponentModel.DataAnnotations;
  using System.Web.Mvc;

  public class LoginViewModel
  {
    [Required]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }

    [HiddenInput]
    public string ReturnUrl { get; set; }
  }
}