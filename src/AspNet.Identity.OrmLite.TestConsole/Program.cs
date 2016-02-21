using Microsoft.AspNet.Identity;
using ServiceStack.Common;
using ServiceStack.OrmLite;
using System;

namespace AspNet.Identity.OrmLite.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var factory = new OrmLiteConnectionFactory(
                @"server=.\sqlexpress2005; User Id=sa; Password=m$ft@local; Database=SampleIdentityDb", 
                SqlServerDialect.Provider
            );

            var mysqlFactory = new OrmLiteConnectionFactory(
                  @"server=localhost; User Id=mysql-general; Password=DbPass14; Database=ef6-context",
                MySqlDialect.Provider
            );

            var ctxt = new OrmLiteIdentityDbContext(mysqlFactory);

            var roleManager = new RoleManager<IdentityRole>(new RoleStore(ctxt));

            roleManager.Create(new IdentityRole("SuperAdministrators"));
            roleManager.Create(new IdentityRole("Administrators"));
            roleManager.Create(new IdentityRole("Users"));

            Console.WriteLine("Three roles created");

            Console.WriteLine();
            Console.WriteLine("Listing the roles all, again!");
            roleManager.Roles.ExecAll(r => Console.WriteLine(r.Name));
        }
    }
}
