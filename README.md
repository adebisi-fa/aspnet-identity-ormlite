##  ServiceStack.OrmLite.v3 AspNet Identity Provider  

An AspNet Identity provider implementation for ServiceStack.OrmLite.v3 (last version was 3.9.71). This provider can be used with all RDBMS that has a OrmLite Dialect (about eight (8)).

## Installation

```  
Install-Package AspNet.Identity.OrmLite
```

## Usage

1.  Create an implementation of __IDbConnectionFactory__, using the OrmLite Dialect for the RDMS you are targeting, as follows:
  ```C# 
    var factory = new OrmLiteConnectionFactory(
        
        /* Rdms-dependent-connection-string */
        "...",  

        /* provider specific dialet here e.g. MySqlDialect.Provider, SQLiteDialect.Provider, etc */
        SqlServerDialect.Provider 
        
    );
  ```
  __NOTE__: 
  
    Nuget your RDMBS's OrmLite package.  There are currently eight (8) flavours of OrmLite on NuGet (see list below).   __HEADS-UP: You must INSTALL version 3.9.71 of your package to use this OrmLite provider).   

  - [Sql Server](http://nuget.org/List/Packages/ServiceStack.OrmLite.SqlServer) (Pre-packaged with this identity provider)
  - [MySql](http://nuget.org/List/Packages/ServiceStack.OrmLite.MySql)
  - [PostgreSQL](http://nuget.org/List/Packages/ServiceStack.OrmLite.PostgreSQL)
  - [Oracle](http://nuget.org/packages/ServiceStack.OrmLite.Oracle)
  - [Firebird](http://nuget.org/List/Packages/ServiceStack.OrmLite.Firebird) 
  - [Sqlite32](http://nuget.org/List/Packages/ServiceStack.OrmLite.Sqlite32) - 32bit Mixed mode .NET only assembly 
  - [Sqlite64](http://nuget.org/List/Packages/ServiceStack.OrmLite.Sqlite64) - 64bit Mixed mode .NET only assembly
  - [Sqlite.Mono](http://nuget.org/packages/ServiceStack.OrmLite.Sqlite.Mono) - 32bit unmanaged dll, compatible with .NET / Mono
  
  This identity provider come pre-packaged with MSSQL Server OrmLite, i.e. __ServiceStack.OrmLite.SqlServer__. Hence, you are not required to install it again when targetting SQL Server.

2.  Create an instance of __OrmLiteIdentityDbContext__ instance using the created __IDbConnectionFactory__.  
  ```C# 
      var context = new OrmLiteIdentityDbContext(factory); 
  ```
  
3.  Create Asp.Net Identity __UserManager__ and __RoleManager__ like so:  
  ```C#
      var userManager = new UserManager<IdentityUser>(new UserStore(context));
      // Optionally configure userManager instance
      // Optionally store in a IoC Container
      // ....
      
      var roleManager = new RoleManager<IdentityRole>(new RoleStore(context));
      // Optionally configure roleManager (e.g. RoleValidator)
      // Optionally store in a IoC Container
  ```
