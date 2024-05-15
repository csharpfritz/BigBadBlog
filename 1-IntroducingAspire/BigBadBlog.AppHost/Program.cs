using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis("outputcache", 65028)
	.WithRedisCommander();

var webApp = builder.AddProject<BigBadBlog_Web>("web")
	.WithReference(cache)
	.WithExternalHttpEndpoints();

builder.Build().Run();
