# 3 - Microservices and MongoDB

Up to this point, we've built and connected services in the same solution and the same repository for our blog.  This convenience and typical approach for .NET developers with mono-repos leads to this question and criticism of .NET Aspire:

![Reddit Question about using separate repositories](img/3-Reddit.png)

You're right... that is a typical pattern for microservice development, placing services in separate folders in separate solutions with distinct source control repositories.  Let's take a step away from our current demo, and build an API in a separate folder and a new solution using MongoDB to store posts.  In fact, the completed sample code for this API isn't even in this repository.  I moved it to https://github.com/csharpfritz/BigBadBlog.PostService

## Starting the new service

Let's assume I'm working on another team in conjunction with Arthur the Author to manage the content for the new blog engine.  I'm building the PostService with [MongoDB](https://mongodb.com), my favorite NoSQL storage solution, in another repository and I'll expose the contents of the service using OpenAPI.


Project reference:  https://learn.microsoft.com/en-us/dotnet/api/aspire.hosting.projectresourcebuilderextensions.addproject?view=dotnet-aspire-8.0.1#aspire-hosting-projectresourcebuilderextensions-addproject(aspire-hosting-idistributedapplicationbuilder-system-string-system-string)