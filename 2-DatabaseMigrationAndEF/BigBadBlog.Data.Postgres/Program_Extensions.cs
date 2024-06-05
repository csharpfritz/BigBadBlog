using BigBadBlog.Common.Data;
using BigBadBlog.Data.Postgres;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBadBlog;

public static class Program_Extensions
{

	public static IHostApplicationBuilder? AddPostgresDatabaseServices(this IHostApplicationBuilder? host)
	{

		host.AddNpgsqlDbContext<ApplicationDbContext>(ServiceNames.DATABASE_POSTS.NAME);
			
		host.Services.AddScoped<IPostRepository, PgPostRepository>();

		return host;

	}

	public static IHostApplicationBuilder? AddIdentityServices(this IHostApplicationBuilder? builder)
	{


		builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>();

		return builder;

	}

}
