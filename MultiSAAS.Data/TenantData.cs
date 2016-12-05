using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;

namespace MultiSAAS.Data
{
  public class TenantData
  {
    private DbContext _context;

    public TenantData() : this(new DbContext())
    {
    }

    public TenantData(DbContext context)
    {
      _context = context;
    }

    public async Task<List<ProjectTo>> ProjectToListAsync<ProjectTo>(string tenantCode = null, string tenantName = null, bool? allowLogin = null)
    {
      return await Queryable(tenantCode, tenantName, allowLogin).ProjectTo<ProjectTo>().ToListAsync();
    }

    public List<ProjectTo> ProjectToList<ProjectTo>(string tenantCode = null, string tenantName = null, bool? allowLogin = null)
    {
      return Queryable(tenantCode, tenantName, allowLogin).ProjectTo<ProjectTo>().ToList();
    }

    public IQueryable Queryable(string tenantCode = null, string tenantName = null, bool? allowLogin = null)
    {
      var list = _context.Tenants
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
        return _context.Tenants.Find(id);
      }
    }

    public void Add(Entities.Tenant entity, bool onlyIfNotExists = false)
    {
      if (entity != null)
      {
        if (onlyIfNotExists)
        {
          _context.Tenants.AddIfNotExists(entity);
        }
        else
        {
          _context.Tenants.Add(entity);
        }
        _context.SaveChanges();
      }
    }

    public void Update(Entities.Tenant entity)
    {
      if (entity != null)
      {
        _context.Entry(entity).State = EntityState.Modified;
        _context.SaveChanges();
      }
    }

    public void Remove(string id)
    {
      if (!string.IsNullOrEmpty(id))
      {
        _context.Tenants.Remove(_context.Tenants.Find(id));
        _context.SaveChanges();
      }
    }

  }
}
