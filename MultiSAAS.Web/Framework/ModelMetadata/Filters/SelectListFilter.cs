namespace MultiSAAS.Web.Framework.ModelMetadata.Filters
{
  using System;
  using System.Collections.Generic;

  public class SelectListFilter : IModelMetadataFilter
  {
    public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
    {
      if (metadata.ModelType == typeof(SelectList) && string.IsNullOrEmpty(metadata.DataTypeName))
      {
        metadata.DataTypeName = "SelectList";
      }
    }
  }
}