using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BigBadBlog.Web.Pages;

public class IndexModel : PageModel
{
	private readonly ILogger<IndexModel> _logger;
	public readonly MarkdownPipeline MarkdownPipeline;
	private readonly IPostRepository _postRepository;
	public IEnumerable<(PostMetadata Metadata, string Content)> Posts;

	public IndexModel(IPostRepository postRepository, ILogger<IndexModel> logger)
	{
		_postRepository = postRepository;
		_logger = logger;

		MarkdownPipeline = new MarkdownPipelineBuilder()
			.UseYamlFrontMatter()
			.Build();

	}

	public async Task<IActionResult> OnGetAsync()
	{

		Posts = await _postRepository.GetPostsAsync(10, 1);

		return Page();

	}
}
