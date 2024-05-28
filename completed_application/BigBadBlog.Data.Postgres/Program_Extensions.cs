using BigBadBlog.Data.Postgres;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBadBlog;

public static class Program_Extensions
{

	public static IHostApplicationBuilder? AddDatabaseServices(this IHostApplicationBuilder? host)
	{

		host.AddNpgsqlDbContext<ApplicationDbContext>(ServiceNames.DATABASE_POSTS.NAME);

		return host;

	}

	public static IHostApplicationBuilder? AddIdentityServices(this IHostApplicationBuilder? builder)
	{

		var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
		//builder.Services.AddDbContext<ApplicationDbContext>(options =>
		//		options.UseSqlite(connectionString));
		//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

		//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
		//		.AddEntityFrameworkStores<ApplicationDbContext>();

		return builder;

	}

}
