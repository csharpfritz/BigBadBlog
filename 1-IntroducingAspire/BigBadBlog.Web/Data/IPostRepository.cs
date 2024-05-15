namespace BigBadBlog.Web.Data;

/// <summary>
/// A definition of a simple interaction with blog posts
/// </summary>
public interface IPostRepository
{

	Task<IEnumerable<(PostMetadata,string)>> GetPostsAsync(int count, int page);

	Task<(PostMetadata,string)> GetPostAsync(string slug);

}

public record PostMetadata(string Filename, string Title, string Author, DateTime Date)
{

	public string Slug => Uri.EscapeDataString(Title.ToLower());

}