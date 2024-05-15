# 0 - Start

This is the entry point to follow along with the Big Bag Blog series.  Inside this folder is a simple blog engine written with ASP.NET Core Razor Pages.  It is configured to be able to read markdown files from a folder called Posts, read the metadata from the YAML header, and transform that markdown to HTML.

There's an `IPostRepository` interface that hides all of the interaction with the filesystem that's implemented in the `MarkdownPostRepository` class.  There's a little bit of in-memory caching of the posts to help reduce the amount of times the blog needs to read from disk.

The original author of this blog thought it would be easy to just write a new markdown file for each new entry and redeploy the website to a cloud provider like Azure.  It's easy to follow, but clearly will not be able to handle hosting many blog posts or a lot of traffic from the internet.

Let's get started with [Module 1](../docs/1-Introduction.md) to improve this blog engine!