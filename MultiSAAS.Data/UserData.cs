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
    private readonly DbContext _context;

    public UserData() : this(new DbContext())
    {
    }

    public UserData(DbContext context)
    {
      _context = context;
    }

    public async Task<List<ProjectTo>> ProjectToListAsync<ProjectTo>(string username, string firstName = null, string lastName = null, string emailAddress = null, bool? enabled = null)
    {
      return await Queryable(username, firstName, lastName, emailAddress, enabled).ProjectTo<ProjectTo>().ToListAsync();
    }

    public List<ProjectTo> ProjectToList<ProjectTo>(string username, string firstName = null, string lastName = null, string emailAddress = null, bool? enabled = null)
    {
      using (var context = new DbContext())
      {
        return Queryable(username, firstName, lastName, emailAddress, enabled).ProjectTo<ProjectTo>().ToList();
      }
    }

    private IQueryable Queryable(string username, string firstName = null, string lastName = null, string emailAddress = null, bool? enabled = null)
    {
      var list = _context.Users
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
        return _context.Users.AsNoTracking().SingleOrDefault(u => u.Username == id);
      }
    }

    public Entities.User Authenticate(string id, string password)
    {
      var encryptedPassword = password.Encrypt();
      return _context.Users.FirstOrDefault(u => u.Username == id && u.Password == encryptedPassword);
    }

    public void Add(Entities.User entity, bool onlyIfNotExists = false)
    {
      if (entity != null)
      {
        if (onlyIfNotExists)
        {
          _context.Users.AddIfNotExists(entity);
        }
        else
        {
          _context.Users.Add(entity);
        }
        _context.SaveChanges();
      }
    }

    public void Update(Entities.User entity)
    {
      if (entity != null)
      {
          if (string.IsNullOrEmpty(entity.Password))
          {
            _context.Users.Attach(entity);
            _context.Entry(entity).Property("Password").IsModified = false;
          }
          _context.Entry(entity).State = EntityState.Modified;
          _context.SaveChanges();
      }
    }

    public void Remove(string id)
    {
      if (!string.IsNullOrEmpty(id))
      {
          _context.Users.Remove(_context.Users.Find(id));
          _context.SaveChanges();
      }
    }

  }
}
