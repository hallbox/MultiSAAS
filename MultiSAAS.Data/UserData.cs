using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using MultiSAAS.Extensions;

namespace MultiSAAS.Data
{
  public class UserData
  {
    public async Task<List<ProjectTo>> ProjectToListAsync<ProjectTo>(string username, string firstName = null, string lastName = null, string emailAddress = null, bool? enabled = null)
    {
      using (var context = new TenantContext())
      {
        return await QueryableFromContext(context, username, firstName, lastName, emailAddress, enabled).ProjectTo<ProjectTo>().ToListAsync();
      }
    }

    public List<ProjectTo> ProjectToList<ProjectTo>(string username, string firstName = null, string lastName = null, string emailAddress = null, bool? enabled = null)
    {
      using (var context = new TenantContext())
      {
        return QueryableFromContext(context, username, firstName, lastName, emailAddress, enabled).ProjectTo<ProjectTo>().ToList();
      }
    }

    private IQueryable QueryableFromContext(TenantContext context, string username, string firstName = null, string lastName = null, string emailAddress = null, bool? enabled = null)
    {
      var list = context.Users
      .Include(u => u.ExternalTenant)
      .Where(u =>
        (string.IsNullOrEmpty(username) || u.Username.Equals(username)) &&
        (string.IsNullOrEmpty(firstName) || u.FirstName.StartsWith(firstName)) &&
        (string.IsNullOrEmpty(lastName) || u.LastName.StartsWith(lastName)) &&
        (string.IsNullOrEmpty(emailAddress) || u.EmailAddress.StartsWith(emailAddress)) &&
        (enabled == null || u.Enabled == enabled)
      )
      .AsNoTracking();
      return list;
    }

    public Entities.User Single(string id)
    {
      if (string.IsNullOrEmpty(id))
      {
        return null;
      }
      else
      {
        using (var context = new TenantContext())
        {
          return context.Users.AsNoTracking().SingleOrDefault(u => u.Username == id);
        }
      }
    }

    public Entities.User Authenticate(string id, string password)
    {
      using (var context = new TenantContext())
      {
        return context.Users.First(u => u.Username == id && u.Password == password.Encrypt());
      }
    }

    public void Add(Entities.User entity)
    {
      if (entity != null)
      {
        using (var context = new TenantContext())
        {
          context.Users.Add(entity);
          context.SaveChanges();
        }
      }
    }

    public void Update(Entities.User entity)
    {
      if (entity != null)
      {
        using (var context = new TenantContext())
        {
          if (string.IsNullOrEmpty(entity.Password))
          {
            context.Users.Attach(entity);
            context.Entry(entity).Property("Password").IsModified = false;
          }
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
          context.Users.Remove(context.Users.Find(id));
          context.SaveChanges();
        }
      }
    }

  }
}
