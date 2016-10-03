namespace MultiSAAS.Web.Framework.ModelMetadata
{
  using System;
  using System.Collections.Generic;
  using System.Linq;
  using System.Reflection;
  using System.Web.Mvc;

  public class ModelMetadataProvider : DataAnnotationsModelMetadataProvider
  {
    private readonly IModelMetadataFilter[] _metadataFilters;

    public ModelMetadataProvider()
    {
      var types = Assembly.GetExecutingAssembly().GetExportedTypes();

      _metadataFilters = (from t in types
        from i in t.GetInterfaces()
        where typeof(IModelMetadataFilter).IsAssignableFrom(t)
              && !t.IsAbstract
              && !t.IsInterface
        select (IModelMetadataFilter) Activator.CreateInstance(t)
        ).ToArray();
    }

    public ModelMetadataProvider(IModelMetadataFilter[] metadataFilters)
    {
      _metadataFilters = metadataFilters;
    }

    protected override System.Web.Mvc.ModelMetadata CreateMetadata(
      IEnumerable<Attribute> attributes,
      Type containerType,
      Func<object> modelAccessor,
      Type modelType,
      string propertyName
    )
    {
      var metadata = base.CreateMetadata(
        attributes,
        containerType,
        modelAccessor,
        modelType,
        propertyName
      );
      Array.ForEach(_metadataFilters, m => m.TransformMetadata(metadata, attributes));

      return metadata;
    }
  }
}