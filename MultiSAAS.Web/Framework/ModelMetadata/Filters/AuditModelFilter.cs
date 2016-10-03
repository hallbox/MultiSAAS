namespace MultiSAAS.Web.Framework.ModelMetadata.Filters
{
  using System;
  using System.Collections.Generic;

  public class AuditModelFilter : IModelMetadataFilter
  {
    public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
    {
      if (metadata.ModelType == typeof(AuditModel))
      {
        if (string.IsNullOrEmpty(metadata.DataTypeName))
        {
          metadata.DataTypeName = "AuditModel";
        }
        metadata.HideSurroundingHtml = true;
      }
    }
  }
}