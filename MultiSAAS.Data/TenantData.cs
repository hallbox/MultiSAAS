using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace MultiSAAS.Data
{
  public class TenantData
  {
    public async Task<List<ProjectTo>> ProjectToListAsync<ProjectTo>(string tenantCode = null, string tenantName = null, bool? allowLogin = null)
    {
      using (var context = new TenantContext())
      {
        return await QueryableFromContext(context, tenantCode, tenantName, allowLogin).ProjectTo<ProjectTo>().ToListAsync();
      }
    }

    public List<ProjectTo> ProjectToList<ProjectTo>(string tenantCode = null, string tenantName = null, bool? allowLogin = null)
    {
      using (var context = new TenantContext())
      {
        return QueryableFromContext(context, tenantCode, tenantName, allowLogin).ProjectTo<ProjectTo>().ToList();
      }
    }

    public IQueryable QueryableFromContext(TenantContext context, string tenantCode = null, string tenantName = null, bool? allowLogin = null)
    {
        var list = context.Tenants
        .Where(u =>
          (string.IsNullOrEmpty(tenantCode) || u.TenantCode.Equals(tenantCode)) &&
          (string.IsNullOrEmpty(tenantName) || u.TenantName.StartsWith(tenantName)) &&
          (allowLogin == null || u.AllowLogin == allowLogin)
        )
        .AsNoTracking();
        return list;
    }

    public Entities.Tenant Single(string id)
    {
      if (string.IsNullOrEmpty(id))
      {
        return null;
      }
      else
      {
        using (var context = new TenantContext())
        {
          return context.Tenants.AsNoTracking().SingleOrDefault(u => u.TenantCode == id);
        }
      }
    }

    public void Add(Entities.Tenant entity)
    {
      if (entity != null)
      {
        using (var context = new TenantContext())
        {
          context.Tenants.Add(entity);
          context.SaveChanges();
        }
      }
    }

    public void Update(Entities.Tenant entity)
    {
      if (entity != null)
      {
        using (var context = new TenantContext())
        {
          context.Entry(entity).State = EntityState.Modified;
          context.SaveChanges();
        }
      }
    }

    public void Remove(string id)
    {
      if (!string.IsNullOrEmpty(id))
      {
        using (var context = new TenantContext())
        {
          context.Tenants.Remove(context.Tenants.Find(id));
          context.SaveChanges();
        }
      }
    }

  }
}
