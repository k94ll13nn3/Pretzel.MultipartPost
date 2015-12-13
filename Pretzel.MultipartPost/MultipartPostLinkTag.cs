// Pretzel.MultipartPost plugin
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DotLiquid;
using Pretzel.Logic.Extensibility;
using Pretzel.Logic.Templating.Context;

namespace Pretzel.MultipartPost
{
    public class MultipartPostLinkTag : DotLiquid.Tag, ITag
    {
        private readonly SiteContext siteContext;

        private bool renderNextPost;

        public MultipartPostLinkTag(SiteContext siteContext)
        {
            this.siteContext = siteContext;
        }

        public new string Name => "MultipartPostLink";

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var trimedMarkup = markup.Trim();
            if (!(trimedMarkup == "prev" || trimedMarkup == "next"))
            {
                throw new ArgumentException("Expected syntax: {% multipart_post_link [prev|next] %}");
            }

            switch (trimedMarkup)
            {
                case "prev":
                    this.renderNextPost = false;
                    break;

                case "next":
                    this.renderNextPost = true;
                    break;
            }

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            var currentPost = this.siteContext.Posts.FirstOrDefault(p => p.Id == context["page.id"].ToString());

            // The block is rendered only if the post is from a series of post.
            if (currentPost != null && new FileInfo(currentPost.File).Directory.Name != "_posts" && currentPost.DirectoryPages.Count() > 1)
            {
                var posts = currentPost.DirectoryPages.OrderBy(p => p.Id).ToList();

                if (this.renderNextPost)
                {
                    var index = posts.IndexOf(currentPost);
                    if (index == posts.Count - 1)
                    {
                        throw new ArgumentException($"The {currentPost.Id} post has no next part.");
                    }

                    result.Write(posts[index + 1].Url);
                }
                else
                {
                    var index = posts.IndexOf(currentPost);
                    if (index == 0)
                    {
                        throw new ArgumentException($"The {currentPost.Id} post has no previous part.");
                    }

                    result.Write(posts[index - 1].Url);
                }
            }
        }
    }
}