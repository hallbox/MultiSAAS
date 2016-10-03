namespace MultiSAAS.Web.Framework
{
  using System.Collections.Generic;
  using System.Web.Mvc;

  public class SelectList
  {
    public List<SelectListItem> Items { get; set; }
    public string SelectedValue { get; set; }
  }
}