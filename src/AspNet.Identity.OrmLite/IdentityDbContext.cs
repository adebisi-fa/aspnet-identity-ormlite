
using ServiceStack.OrmLite;
using System;
using System.Data;

namespace AspNet.Identity.OrmLite
{

    public class OrmLiteIdentityDbContext : OrmLiteIdentityDbContext<IdentityUser<string>, IdentityRole<string>>
    {
        public OrmLiteIdentityDbContext(IDbConnectionFactory connection) : base(connection)
        { }
    }

    public class OrmLiteIdentityDbContext<TUser, TRole> : OrmLiteIdentityDbContext<TUser, TRole, string, string>
        where TUser : IdentityUser<string>
        where TRole : IdentityRole<string>
    {
        public OrmLiteIdentityDbContext(IDbConnectionFactory connection) : base(connection)
        {
            using (var db = connection.OpenDbConnection())
            {
                db.CreateTableIfNotExists<IdentityUser>();
                db.CreateTableIfNotExists<IdentityRole>();
                db.CreateTableIfNotExists<IdentityUserClaim>();
                db.CreateTableIfNotExists<IdentityUserLogin>();
                db.CreateTableIfNotExists<IdentityUserRole>();
            }
        }
    }

    public class OrmLiteIdentityDbContext<TUser, TRole, TKey, TRoleKey> : IDisposable
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TRoleKey>
    {

        IDbConnectionFactory _factory;

        public OrmLiteIdentityDbContext(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public IDbConnection OpenDbConnection()
        {
            return _factory.OpenDbConnection();
        }

        public void Dispose()
        {
            // No resource to dispose for now!
        }
    }
}
