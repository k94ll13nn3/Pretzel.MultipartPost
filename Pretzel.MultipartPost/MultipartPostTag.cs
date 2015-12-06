using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotLiquid;
using Pretzel.Logic.Extensibility;
using Pretzel.Logic.Templating.Context;

namespace Pretzel.MultipartPost
{
    // TODO: Permettre l'inclusion ou non du post courant (en gras ?)
    // TODO: Add a base CSS class ?
    public class MultipartPostTag : DotLiquid.Tag, ITag
    {
        private readonly SiteContext siteContext;
        private bool reverseOrder;
        private bool includeCurrent = true;

        public MultipartPostTag(SiteContext siteContext)
        {
            this.siteContext = siteContext;
        }

        public new string Name => "MultipartPost";

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var trimedMarkup = markup.Trim();
            if (!(string.IsNullOrWhiteSpace(markup) || trimedMarkup == "asc" || trimedMarkup == "desc" || trimedMarkup == "wasc" || trimedMarkup == "wdesc"))
            {
                throw new ArgumentException("Expected syntax: {% multipart_post [asc|desc|wasc|wdesc] %}");
            }

            if (trimedMarkup == "desc")
            {
                this.reverseOrder = true;
            }
            else if (trimedMarkup == "wasc")
            {
                this.includeCurrent = false;
            }
            else if (trimedMarkup == "wdesc")
            {
                this.reverseOrder = true;
                this.includeCurrent = false;
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            if (this.siteContext == null)
            {
                return;
            }

            var currentPost = this.siteContext.Posts.FirstOrDefault(p => p.Id == context["page.id"].ToString());

            if (currentPost != null && new FileInfo(currentPost.File).Directory.Name != "_posts" && currentPost.DirectoryPages.Count() > 1)
            {
                var posts = this.reverseOrder ? currentPost.DirectoryPages.OrderByDescending(p => p.Id) : currentPost.DirectoryPages.OrderBy(p => p.Id);

                result.Write("<ul>");

                foreach (var page in posts)
                {
                    if (page.Id != currentPost.Id || this.includeCurrent)
                    {
                        result.Write($"<li><a href=\"{page.Url}\">{page.Title}</a></li>");
                    }
                }

                result.Write("</ul>");
            }
        }
    }
}