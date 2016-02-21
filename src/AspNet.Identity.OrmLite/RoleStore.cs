
using Microsoft.AspNet.Identity;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspNet.Identity.OrmLite
{

    public class RoleStore : IQueryableRoleStore<IdentityRole, string>, IRoleStore<IdentityRole, string>, IDisposable
    {
        private OrmLiteIdentityDbContext _context;

        public RoleStore(OrmLiteIdentityDbContext context)
        {
            if (context == null)
                throw new ArgumentNullException("context is null");

            _context = context;
        }

        public virtual IQueryable<IdentityRole> Roles
        {
            get
            {
                using (var db = _context.OpenDbConnection())
                    return db.Select<IdentityRole>().AsQueryable();
            }
        }

        public virtual Task CreateAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            using (var db = _context.OpenDbConnection())
            {
                db.Insert(role);
                return Task.FromResult(0);
            }
        }

        public virtual Task DeleteAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }

            using (var db = _context.OpenDbConnection())
            {
                db.Delete<IdentityRole>(r => r.Id == role.Id);
                return Task.FromResult(0);
            }
        }

        public virtual Task<IdentityRole> FindByIdAsync(string roleId)
        {
            using (var db = _context.OpenDbConnection())
                return Task.FromResult(db.FirstOrDefault<IdentityRole>(r => r.Id == roleId));
        }

        public virtual Task<IdentityRole> FindByNameAsync(string roleName)
        {
            using (var db = _context.OpenDbConnection())
                return Task.FromResult(db.FirstOrDefault<IdentityRole>(r => r.Name == roleName));
        }

        public virtual Task UpdateAsync(IdentityRole role)
        {
            if (role == null)
            {
                throw new ArgumentNullException("role");
            }
            using (var db = _context.OpenDbConnection())
            {
                db.Update(role);
                return Task.FromResult(0);
            }
        }

        public virtual void Dispose()
        {
            // No resource to dispose for now!
        }
    }
}
