using BigBadBlog.Common.Data;

namespace BigBadBlog.Web;

//public partial class PostApi
//{

//	public PostApi(HttpClient client) : this(client.BaseAddress.ToString(), client) { }

//}

public class PostMicroserviceRepository(PostApi api) : IPostRepository
{
	public async Task AddPostAsync(PostMetadata metadata, string content)
	{

		// Add the post using PostApi
		var newPost = new Post()
		{
			Author = metadata.Author,
			Content = content,
			Date = metadata.Date,
			Title = metadata.Title,
		};

		await api.CreatePostAsync(newPost);

	}

	public async Task<(PostMetadata, string)> GetPostAsync(string slug)
	{

		// get the post using PostApi
		var post = await api.GetPostBySlugAsync(Uri.EscapeDataString(slug.ToLowerInvariant()));

		// convert the Post to a PostMetadata
		var metadata = new PostMetadata("", post.Title, post.Author, post.Date.Date);

		return (metadata, post.Content);

	}

	public async Task<IEnumerable<(PostMetadata, string)>> GetPostsAsync(int count, int page)
	{

		// get the posts using PostApi
		var posts = (await api.GetAllPostsAsync())
			.OrderByDescending(page => page.Date)	
			.Skip((page-1)*count)
			.Take(count)
			.ToList();

		var outCollection = posts.Count == 0 ?
			Enumerable.Empty<(PostMetadata, string)>() :
			posts.Select(p => (new PostMetadata("", p.Title, p.Author, p.Date.Date), p.Content));

		return outCollection;

	}
}