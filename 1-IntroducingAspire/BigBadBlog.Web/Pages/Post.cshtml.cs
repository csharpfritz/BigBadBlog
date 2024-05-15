using BigBadBlog.Web.Data;
using Markdig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Hosting;

namespace BigBadBlog.Web.Pages;

[OutputCache(PolicyName = "Post")]
public class PostModel : PageModel
{
	private readonly IPostRepository _postRepository;
	public readonly MarkdownPipeline MarkdownPipeline;

	public PostModel(IPostRepository postRepository, IWebHostEnvironment host)
	{
		_postRepository = postRepository;

		MarkdownPipeline = new MarkdownPipelineBuilder()
			.UseYamlFrontMatter()
			.Build();

	}

	public (PostMetadata Metadata, string Content) Post { get; private set; }

	public async Task<IActionResult> OnGetAsync(string slug)
	{

		Post = await _postRepository.GetPostAsync(slug);

		if (Post == default)
		{
			return NotFound();
		}

		return Page();

	}
}
