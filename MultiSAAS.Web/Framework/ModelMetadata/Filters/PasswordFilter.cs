namespace MultiSAAS.Web.Framework.ModelMetadata.Filters
{
  using System;
  using System.Collections.Generic;

  public class PasswordFilter : IModelMetadataFilter
  {
    public void TransformMetadata(System.Web.Mvc.ModelMetadata metadata, IEnumerable<Attribute> attributes)
    {
      if (!string.IsNullOrEmpty(metadata.PropertyName) && metadata.PropertyName.ToLower().Contains("password") && string.IsNullOrEmpty(metadata.DataTypeName))
      {
        metadata.DataTypeName = "Password";
      }
    }
  }
}