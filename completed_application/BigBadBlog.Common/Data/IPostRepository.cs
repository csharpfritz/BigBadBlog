namespace BigBadBlog.Common.Data;

/// <summary>
/// A definition of a simple interaction with blog posts
/// </summary>
public interface IPostRepository
{

	Task<IEnumerable<(PostMetadata,string)>> GetPostsAsync(int count, int page);

	Task<(PostMetadata,string)> GetPostAsync(string slug);

}
