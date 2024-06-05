namespace BigBadBlog.Common.Data;

public record PostMetadata(string Filename, string Title, string Author, DateTime Date)
{

	public string Slug => Uri.EscapeDataString(Title.ToLower());

}