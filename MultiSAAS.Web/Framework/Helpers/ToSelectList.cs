namespace MultiSAAS.Web.Framework.Helpers
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Web.Mvc;
  using SelectList = SelectList;

  public static partial class HtmlHelpers
  {
    public static SelectList ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, object> value,
      Func<T, object> text, IEnumerable<object> selectedValues)
    {
      return enumerable.ToSelectList(value, text, selectedValues, null);
    }

    public static SelectList ToSelectList<T>(
      this IEnumerable<T> enumerable, 
      Func<T, object> value,
      Func<T, object> text, 
      string selectedValue = null, 
      string blankValue = null
    )
    {
      return enumerable.ToSelectList(value, text, selectedValue == null ? null : new List<object> { selectedValue }, blankValue);
    }

    public static SelectList ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, object> value,
      Func<T, object> text, IEnumerable<object> selectedValues, string blankValue)
    {
      var sel = selectedValues?.Where(x => x != null).ToList().ConvertAll(x => x.ToString()) ?? new List<string>();

      var items = enumerable.Select(f => new SelectListItem
      {
        Value = value(f).ToString(),
        Text = text(f).ToString(),
        Selected = sel.Contains(value(f).ToString())
      }).ToList();
      if (blankValue != null)
      {
        items.Insert(0, new SelectListItem
        {
          Value = blankValue,
          Text = "",
          Selected = sel.Contains(blankValue)
        });
      }
      return new SelectList() { Items = items, SelectedValue = sel.Count > 0 ? sel[0] : "" };
    }

    /*
    public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, object> value,
      bool selectAll = false)
    {
      return enumerable.ToSelectList(value, value, selectAll);
    }

    public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, object> value,
      object selectedValue)
    {
      return enumerable.ToSelectList(value, value, new List<object> {selectedValue});
    }

    public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, object> value,
      IEnumerable<object> selectedValues)
    {
      return enumerable.ToSelectList(value, value, selectedValues);
    }

    public static IEnumerable<SelectListItem> ToSelectList<T>(this IEnumerable<T> enumerable, Func<T, object> value,
      Func<T, object> text, bool selectAll = false)
    {
      IEnumerable<T> selectList = new List<T>();
      if (selectAll)
      {
        selectList = enumerable.ToList();
      }
      return enumerable.ToSelectList(value, value, selectList);
    }
    */
  }
}