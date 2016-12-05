namespace MultiSAAS.Data
{
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Core.Objects;
  using System.Data.Entity.Infrastructure;
  using System.Linq;
  using System.Linq.Expressions;
  using System.Reflection;

  public static class DbExtensions
  {
    public static void ReadAllDateTimeValuesAsUtc(this DbContext context)
    {
      ((IObjectContextAdapter)context).ObjectContext.ObjectMaterialized += ReadAllDateTimeValuesAsUtc;
    }

    private static void ReadAllDateTimeValuesAsUtc(object sender, ObjectMaterializedEventArgs e)
    {
      // Extract all DateTime properties of the object type
      var properties = e.Entity.GetType().GetProperties()
        .Where(property => property.PropertyType == typeof(DateTime) ||
                           property.PropertyType == typeof(DateTime?)).ToList();
      // Set all DaetTimeKinds to Utc
      properties.ForEach(property => SpecifyUtcKind(property, e.Entity));
    }

    private static void SpecifyUtcKind(PropertyInfo property, object value)
    {
      // Get the datetime value
      var datetime = property.GetValue(value, null);

      // Set DateTimeKind to Utc
      if (property.PropertyType == typeof(DateTime))
      {
        datetime = DateTime.SpecifyKind((DateTime)datetime, DateTimeKind.Utc);
      }
      else if (property.PropertyType == typeof(DateTime?))
      {
        var nullable = (DateTime?)datetime;
        if (!nullable.HasValue)
        {
          return;
        }
        datetime = (DateTime?)DateTime.SpecifyKind(nullable.Value, DateTimeKind.Utc);
      }
      else
      {
        return;
      }

      // And set the Utc DateTime value
      property.SetValue(value, datetime, null);
    }

    public static T GetAttributeFrom<T>(this object instance, string propertyName) where T : Attribute
    {
      var attrType = typeof(T);
      var property = instance.GetType().GetProperty(propertyName);
      return (T)property.GetCustomAttributes(attrType, false).First();
    }
  }

  public static class DbSetExtensions
  {
    public static T AddIfNotExists<T>(this DbSet<T> dbSet, T entity, Expression<Func<T, bool>> predicate = null)
      where T : class, new()
    {
      var exists = predicate != null ? dbSet.Any(predicate) : dbSet.Any();
      return !exists ? dbSet.Add(entity) : null;
    }
  }
}