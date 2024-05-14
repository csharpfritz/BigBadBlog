using Markdig;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;

namespace BigBadBlog.Web.Data;

/// <summary>
/// A simple repository for blog posts that are saved as markdown files in the Posts folder
/// </summary>
public class MarkdownPostRepository : IPostRepository
{

	private static readonly Dictionary<PostMetadata, string> _posts = new();
	private readonly MarkdownPipeline _MarkdownPipeline;

	public MarkdownPostRepository()
	{

		_MarkdownPipeline = new MarkdownPipelineBuilder()
			.UseYamlFrontMatter()
			.Build();

		if (!_posts.Any())
		{
			var files = Directory.GetFiles("Posts", "*.md");
			foreach (var file in files)
			{
				var newPost = ExtractMetadataFromFile(file);
				_posts.Add(newPost.Item1, newPost.Item2);
			}
		}

	}

	public Task<(PostMetadata, string)> GetPostAsync(string slug)
	{

		var toCheck = Uri.EscapeDataString(slug.ToLower());
		var thePost = _posts.FirstOrDefault(p => p.Key.Slug == toCheck);

		return Task.FromResult((thePost.Key, thePost.Value));

	}

	public Task<IEnumerable<(PostMetadata, string)>> GetPostsAsync(int count, int page)
	{

		count = count < 1 ? 10 : count;
		page = page < 1 ? 1 : page;

		return _posts.Any() ?
			Task.FromResult(_posts
				.OrderByDescending(p => p.Key.Date)
				.Skip((page - 1) * count)
				.Take(count)
				.Select(p => (p.Key, p.Value))
				.AsEnumerable())
			: Task.FromResult(Enumerable.Empty<(PostMetadata, string)>());
	}

	private (PostMetadata, string) ExtractMetadataFromFile(string fileName)
	{

		// Read all content from the file
		var content = File.ReadAllText(fileName);

		var yamlBlock = Markdown.Parse(content, _MarkdownPipeline)
			.Descendants<YamlFrontMatterBlock>()
			.FirstOrDefault();

		var yamlDictionary = new Dictionary<string, string>();
		foreach (var line in yamlBlock.Lines)
		{
			if (line.ToString().Contains(": "))
			{
				var values = line.ToString().Split(": ", StringSplitOptions.TrimEntries);
				yamlDictionary.Add(values[0].ToLower(), values[1]);
			}
		}

		return (new PostMetadata(
			fileName,
			yamlDictionary["title"],
			yamlDictionary["author"],
			DateTime.Parse(yamlDictionary["date"])
		), content);

	}

}
