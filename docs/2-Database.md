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







## Aside: System Constants

## Add Entity Framework and the data project

## Introducing Database Migration Service

## Migrating Data

