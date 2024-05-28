using BigBadBlog;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgresPassword = builder.AddParameter("postgresspassword", secret: true);
var db = builder.AddPostgres(ServiceNames.DATABASE_POSTS.SERVERNAME, password: postgresPassword)
	.WithDataVolume()
	.AddDatabase(ServiceNames.DATABASE_POSTS.NAME);

var cache = builder.AddRedis(ServiceNames.OUTPUTCACHE, 65028)
	.WithRedisCommander();

var webApp = builder.AddProject<BigBadBlog_Web>("web")
	.WithReference(cache)
	.WithExternalHttpEndpoints();

builder.Build().Run();
