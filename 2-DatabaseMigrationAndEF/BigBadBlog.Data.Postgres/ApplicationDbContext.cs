using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BigBadBlog.Data.Postgres;

public class ApplicationDbContext : IdentityDbContext
{
	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
	{
	}

	public DbSet<PgPost> Posts { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{

		// Add an index to the PgPost slug column
		builder.Entity<PgPost>()
			.HasIndex(p => p.Slug)
			.IsUnique();

		// Add an index to the PgPost date column
		builder.Entity<PgPost>()
			.HasIndex(p => p.Date);

		base.OnModelCreating(builder);
	}

}
