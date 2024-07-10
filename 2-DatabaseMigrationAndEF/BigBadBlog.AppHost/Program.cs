using BigBadBlog;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

#region Posts Database

var postgresPassword = builder.AddParameter("postgresspassword", secret: true);
var dbServer = builder.AddPostgres(ServiceNames.DATABASE_POSTS.SERVERNAME, password: postgresPassword)
	.AsAzurePostgresFlexibleServer()	
	.WithDataVolume();
	

var db = dbServer
.AddDatabase(ServiceNames.DATABASE_POSTS.NAME);


var migrationService = builder.AddProject<BigBadBlog_Service_DatabaseMigration>(ServiceNames.MIGRATION)
	.WithReference(db);

#endregion

#region Redis Cache

var cache = builder.AddRedis(ServiceNames.OUTPUTCACHE, 65028)
	.AsAzureRedis()
	.WithRedisCommander();

#endregion

#region Web Application

var webApp = builder.AddProject<BigBadBlog_Web>("web")
	.WithReference(cache)
	.WithReference(db)
	.WithExternalHttpEndpoints();

#endregion

builder.Build().Run();
