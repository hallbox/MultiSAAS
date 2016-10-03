namespace MultiSAAS.Web.Framework.ModelMetadata.Filters
{
  using System;
  using System.Collections.Generic;

  public class ReadOnlyFilter : IModelMetadataFilter
  {
    public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
    {
      if (metadata.IsReadOnly && string.IsNullOrEmpty(metadata.DataTypeName))
      {
        metadata.DataTypeName = "ReadOnly";
      }
    }
  }
}