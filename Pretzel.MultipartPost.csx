public class MultipartPostTag : DotLiquid.Tag, ITag
{
	private readonly SiteContext siteContext;
	private bool reverseOrder = false;

	public MultipartPostTag(SiteContext siteContext)
	{
		this.siteContext = siteContext;
	}

	public new string Name 
	{
		get 
		{
			return "MultipartPost";
		}
	}

	public override void Initialize(string tagName, string markup, List<string> tokens)
	{
		if (!(string.IsNullOrWhiteSpace(markup) || markup.Trim() == "asc" || markup.Trim() == "desc"))
		{
			throw new ArgumentException("Expected syntax: {% multipart_post [asc|desc] %}");
		}

		if (markup.Trim() == "desc")
		{
			this.reverseOrder = true;
		}

		base.Initialize(tagName, markup, tokens);
	}

	public override void Render(Context context, TextWriter result)
	{
		if (siteContext == null)
		{
			return;
		}

		var currentPost = siteContext.Posts.FirstOrDefault(p => p.Id == context["page.id"].ToString());
		var fileInfo = new FileInfo(currentPost.File);

		if (currentPost != null && fileInfo.Directory.Name != "_posts" && currentPost.DirectoryPages.Count() > 1)
		{
			var posts = this.reverseOrder ? currentPost.DirectoryPages.OrderByDescending(p => p.Id) : currentPost.DirectoryPages.OrderBy(p => p.Id);

			result.Write("<ul>");
			foreach (var page in posts)
			{
				result.Write(string.Format("<li><a href=\"{0}\">{1}</a></li>", page.Url, page.Title));
			}
			result.Write("</ul>");
		}
	}
}

public class MultipartPostTagFactory : TagFactoryBase
{
	public MultipartPostTagFactory()
		: base("MultipartPost")
	{ }

	public override ITag CreateTag()
	{
		return new MultipartPostTag(SiteContext);
	}
}