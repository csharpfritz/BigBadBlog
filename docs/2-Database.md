# 2 - Working with Databases and Entity Framework

Our little blog works nice with its markdown files, but we want to enable more users to work with the blog posts and that shared level of access means we should introduce a database.  A database will allow multiple users to read and write data as well as provide faster access to the data than reading a markdown file from disk.

## Introducing database providers

.NET Aspire has a number of **components** available that allow you to connect bits of system architecture together.  These components can run in a standalone workstation for development and testing.  When you're ready you can deploy those components and have a great production experience.

Aspire components typically have 2 parts: 
 1. a hosting package that will run the component on your developer system
 2. a library package for consuming the hosted service with optional telemetry and healthcheck features available

Among the database components available at the time of writing are:

- Azure Storage
- Azure Cosmos DB
- MongoDB
- MySQL
- Oracle
- PostgreSQL
- Qdrant
- Redis
- SQL Server

Microsoft Learn has an exhausitve list of the [current Aspire components](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/components-overview?tabs=dotnet-cli#available-components) available.

### Add Postgres to the system

We can introduce a Postgres database to our application system by adding a project reference and a few lines into the `BigBadBlog.AppHost/Program.cs` file.

Add the `Aspire.Hosting.PostgreSQL` package to the `BigBadBlog.AppHost` project.  You can run this command in the AppHost project folder:

```shell
dotnet add package Aspire.Hosting.PostgreSQL
```

In the `Program.cs` file of that project, let's introduce the database:

```csharp
var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("bigbad-db");
...
```

Running the application now shows the **bigbad-db** database running as a postgres container:

![Dashboard with first appearance of the database](img/2-DashboardWithDb-1.png)

That's nice, we have a Postgres container with a server.  Servers need databases and I'd like to be able to administer that database so that I can peek in there to look at the data.

Finally, I need to pass that database into my web application so that we can connect to it with Entity Framework.

We can enhance our code in `AppHost/Program.cs` to describe our database like this:

```csharp
var db = builder.AddPostgres("bigbad-db")
	.WithDataVolume()
	.WithPgAdmin()
	.AddDatabase("bigbad-database");

var webApp = builder.AddProject<BigBadBlog_Web>("web")
	.WithReference(cache)
	.WithReference(db)
	.WithExternalHttpEndpoints();
```

Let's review the new lines added:

- `WithDataVolume()` configures a data volume on disk to persist content to
- `WithPgAdmin()` launches a second container running the pgAdmin application.  This allows us to connect to our database and run SQL statements among other database administrative tasks
- `AddDatabase("bigbad-database")` adds a database to the server named 'bigbad-database'
- `WithReference(db)` passes a connection string for the "bigbad-database" into the web application.  The database is passed, not the server, because the `AddDatabase()` method was last in the chain 

Let's run it and take a look at the dashboard now:

![Aspire Dashboard showing the bigbad-database and pgAdmin containers](img/2-DashboardWithDb-2.png)

We have a _SLIGHT_ problem with this configuration: the database will start and assign a random password to the default postgres user on every restart of the Aspire stack.  This is counter to the purpose of our decision to create a data volume that will persist content between restarts.  We can force a password on the database by passing in a parameter from the configuration of our AppHost project.  

Let's add a parameter and the appropriate configuration to `appSettings.json` to configure our postgres database with a password.

In `AppHost/Program.cs` we'll define a parameter and pass that parameter as the password argument for our database declaration:

```csharp
var postgresPassword = builder.AddParameter("pgPassword", secret: true);

var db = builder.AddPostgres("bigbad-db", password: postgresPassword);
```

Next, we need to define the `pgPassword` configuration value for the AppHost project.  We can add a `Parameters` section to the `appSettings.json` file with a `pgPassword` key-value pair to be used for this parameter:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Aspire.Hosting.Dcp": "Warning"
    }
  },
	"Parameters":{
		"pgPassword": "MyP@55w0rd!"
	}
}
```

The actual value of the password doesn't matter, but I like to be all fancy and 31337 mixing numbers, letters and symbols.

Now when we start the database, it will always configure it with the password `MyP@55w0rd!`

## Aside: System Constants

There's a LOT of strings that we're passing around in our `AppHost/Program.cs` file and eventually into our application files that define the names of connection strings and other services.  These 'magic strings' are easy to mix up and confuse when building the application system.

Let's centralize these names in a `Common` project that can be referenced by both the `AppHost` and `Web` projects to ensure consistent references to the names of services.

I'll add a new **Class Library** project to the solution called `BigBadBlog.Common` and update the class file that came in the template with this:

```csharp
namespace BigBadBlog;

public static class ServiceNames
{

	/// <summary>
	/// Constants for referencing the database containing blog posts
	/// </summary>
	public static class DATABASE_POSTS {

		public const string SERVERNAME = "bigbad-db";
		public const string NAME = "bigbad-database";

	}

	public const string OUTPUTCACHE = "outputcache";

}
```

We can add a reference to this project in the `AppHost` project by copying this XML element into the `BigBadBlog.AppHost.csproj` file:

```xml
  <ItemGroup>
    <ProjectReference Include="..\BigBadBlog.Common\BigBadBlog.Common.csproj" IsAspireProjectResource="false" />
    <ProjectReference Include="..\BigBadBlog.Web\BigBadBlog.Web.csproj" />
  </ItemGroup>
```

Notice the additional attribute `IsAspireProjectResource`.  This changes the Common project from being a resource that Aspire may configure to a standard class library reference.  We can update the database configuration in `Program.cs` to take advantage of these resources now:

```csharp
var db = builder.AddPostgres(ServiceNames.DATABASE_POSTS.SERVERNAME, password: postgresPassword)
	.WithDataVolume()
	.WithPgAdmin()
	.AddDatabase(ServiceNames.DATABASE_POSTS.NAME);
```

This will make it easier when we connect the website to the database and configure Entity Framework.

## Add Entity Framework and the data project

## Introducing Database Migration Service

## Migrating Data

