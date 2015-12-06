using DotLiquid;
using Pretzel.Logic.Extensibility;
using System.Collections.Generic;
using Pretzel.Logic.Templating.Context;
using System.Linq;
using System.IO;

namespace Pretzel.MultipartPost
{
    internal class IsMutltipartPostBlock : Block, ITag
    {
        private readonly SiteContext siteContext;

        public IsMutltipartPostBlock(SiteContext siteContext)
        {
            this.siteContext = siteContext;
        }

        public new string Name => "IsMultipartPost";

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, System.IO.TextWriter result)
        {
            if (this.siteContext == null)
            {
                return;
            }

            var currentPost = this.siteContext.Posts.FirstOrDefault(p => p.Id == context["page.id"].ToString());

            // The block is rendered only if the post is from a series of post.
            if (currentPost != null && new FileInfo(currentPost.File).Directory.Name != "_posts" && currentPost.DirectoryPages.Count() > 1)
            {
                base.Render(context, result);
            }
        }
    }
}