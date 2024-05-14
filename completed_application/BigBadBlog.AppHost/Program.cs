using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var web = builder.AddProject<BigBadBlog_Web>("web");

builder.Build().Run();
