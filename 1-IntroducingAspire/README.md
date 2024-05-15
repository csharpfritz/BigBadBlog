# 1 - Introducing Aspire

Well... that was a LOT of changes and [updates we've implemented](../docs/1-Introduction.md) at this point.  

- The blog engine now runs in a .NET Aspire system that's started from within the `BigBadBlog.AppHost` project.
- We've added a Redis database to the system
- The Redis database is connected to the web project and used as an OutputCache so the blog doesn't need to re-render blog posts all the time
- We added a RedisCommander instance to allow us to look inside the Redis database

