namespace MultiSAAS.Web.Framework.ModelMetadata.Filters
{
  using System;
  using System.Collections.Generic;
  using System.Text.RegularExpressions;
  using MultiSAAS.Extensions;

  public class LabelFilter : IModelMetadataFilter
  {
    public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
    {
      if (!string.IsNullOrEmpty(metadata.PropertyName) && string.IsNullOrEmpty(metadata.DisplayName))
      {
        // split ProperCase words
        metadata.DisplayName = metadata.PropertyName.SplitWords();

        // ends with DT, replace with Date
        metadata.DisplayName = metadata.DisplayName != null
          ? Regex.Replace(metadata.DisplayName, "( DT)$", " Date")
          : null;

        // handle language translations here someday!
      }
    }
  }
}