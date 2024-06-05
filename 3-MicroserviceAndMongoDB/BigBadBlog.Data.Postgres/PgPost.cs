using BigBadBlog.Common.Data;
using System.ComponentModel.DataAnnotations;

namespace BigBadBlog.Data.Postgres;

public class PgPost
{

	[Key]
	public int Id { get; set; }

	[Required, MaxLength(100)]
	public string Title { get; set; }

	[Required, MaxLength(100)]
	public string Author { get; set; }

	[Required]
	public DateTime Date { get; set; }

	[Required]
	public string Content { get; set; }

	[Required, MaxLength(150)]
	public string Slug { get; set; }

	// explicit conversion to PostMetadata
	public static explicit operator PostMetadata(PgPost post)
	{
		return new PostMetadata("", post.Title, post.Author, post.Date);
	}

	// explicit conversation from PostMetadata
	public static explicit operator PgPost((PostMetadata, string) post)
	{
		return new PgPost
		{
			Title = post.Item1.Title,
			Author = post.Item1.Author,
			Date = post.Item1.Date,
			Content = post.Item2,
			Slug = post.Item1.Slug
		};
	}

}
