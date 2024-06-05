using BigBadBlog.Common.Data;
using Microsoft.EntityFrameworkCore;

namespace BigBadBlog.Data.Postgres;

internal class PgPostRepository : IPostRepository
{
	private readonly ApplicationDbContext _context;

	public PgPostRepository(ApplicationDbContext context)
	{
		_context = context;
	}

	public async Task<(PostMetadata, string)> GetPostAsync(string slug)
	{
		var post = await _context.Posts
			.Where(p => p.Slug == slug)
			.Select(p => (PostMetadata)p)
			.FirstOrDefaultAsync();

		if (post == default)
		{
			return default;
		}

		var content = await _context.Posts
			.Where(p => p.Slug == slug)
			.Select(p => p.Content)
			.FirstOrDefaultAsync();

		return (post, content);
	}

	public async Task<IEnumerable<(PostMetadata,string)>> GetPostsAsync(int count, int page)
	{
		// fetch the posts from the database and convert them to PostMetadata
		var posts = await _context.Posts
			.OrderByDescending(p => p.Date)
			.Skip(count * (page - 1))
			.Take(count)
			.ToListAsync();

		return posts.Select(p => ((PostMetadata)p, p.Content));

	}

	public async Task AddPostAsync(PostMetadata metadata, string content)
	{
		var post = (PgPost)(metadata, content);

		post.Date = new DateTime(DateTime.Now.Ticks, DateTimeKind.Utc);
		_context.Posts.Add(post);

		await _context.SaveChangesAsync();
	}

	public async Task UpdatePostAsync(PostMetadata metadata, string content)
	{
		var post = (PgPost)(metadata, content);

		_context.Posts.Update(post);

		await _context.SaveChangesAsync();
	}

	public async Task DeletePostAsync(string slug)
	{
		var post = await _context.Posts
			.Where(p => p.Slug == slug)
			.FirstOrDefaultAsync();

		if (post != default)
		{
			_context.Posts.Remove(post);
			await _context.SaveChangesAsync();
		}
	}
}