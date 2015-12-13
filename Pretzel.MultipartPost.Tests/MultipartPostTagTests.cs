using DotLiquid;
using NUnit.Framework;
using Pretzel.Logic.Templating.Context;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pretzel.MultipartPost.Tests
{
    [TestFixture]
    public class MultipartPostTagTests
    {
        [Test]
        public void TestGoodPatterns()
        {
            var m = new MultipartPostTag(null);

            Assert.DoesNotThrow(() => m.Initialize(null, "", null));
            Assert.DoesNotThrow(() => m.Initialize(null, "asc", null));
            Assert.DoesNotThrow(() => m.Initialize(null, "desc", null));
            Assert.DoesNotThrow(() => m.Initialize(null, "wasc", null));
            Assert.DoesNotThrow(() => m.Initialize(null, "wdesc", null));
        }

        [Test]
        public void TestRenderMultipart()
        {
            var pageZero = CreatePost("0");
            var pageOne = CreateMultipartPost("1");
            var pageTwo = CreateMultipartPost("2");

            pageOne.DirectoryPages = new List<Page> { pageOne, pageTwo };
            pageTwo.DirectoryPages = new List<Page> { pageOne, pageTwo };

            var siteContext = new SiteContext();
            siteContext.Posts.Add(pageOne);
            siteContext.Posts.Add(pageTwo);

            Assert.AreEqual(string.Empty, RenderTag(siteContext, "0"));
            Assert.AreEqual("<ul class=\"multipart-post-list\"><li><a class=\"current-post\" href=\"/posts/series/1\">Post series 1</a></li><li><a href=\"/posts/series/2\">Post series 2</a></li></ul>", RenderTag(siteContext, "1"));
            Assert.AreEqual("<ul class=\"multipart-post-list\"><li><a href=\"/posts/series/1\">Post series 1</a></li><li><a class=\"current-post\" href=\"/posts/series/2\">Post series 2</a></li></ul>", RenderTag(siteContext, "2"));
        }

        [Test]
        public void TestWrongPatterns()
        {
            var m = new MultipartPostTag(null);

            Assert.Throws<ArgumentException>(() => m.Initialize(null, "string", null));
            Assert.Throws<ArgumentException>(() => m.Initialize(null, "as", null));
            Assert.Throws<ArgumentException>(() => m.Initialize(null, "desc a", null));
        }

        [Test]
        public void TestIsMultipartPost()
        {
            var pageZero = CreatePost("0");
            var pageOne = CreateMultipartPost("1");
            var pageTwo = CreateMultipartPost("2");

            pageOne.DirectoryPages = new List<Page> { pageOne, pageTwo };
            pageTwo.DirectoryPages = new List<Page> { pageOne, pageTwo };

            var siteContext = new SiteContext();
            siteContext.Posts.Add(pageOne);
            siteContext.Posts.Add(pageTwo);

            var m = new IsMultipartPostBlock(siteContext);
            var liquidContext = new Context();

            // If there is a NullReferenceException, it means that the base.Render() was call
            // so that the post is a multi part post
            liquidContext["page"] = new Hash { new KeyValuePair<string, object>("id", "0") };
            Assert.DoesNotThrow(() => m.Render(liquidContext, null));

            liquidContext["page"] = new Hash { new KeyValuePair<string, object>("id", "1") };
            Assert.Throws<NullReferenceException>(() => m.Render(liquidContext, null));

            liquidContext["page"] = new Hash { new KeyValuePair<string, object>("id", "2") };
            Assert.Throws<NullReferenceException>(() => m.Render(liquidContext, null));
        }

        private static Page CreateMultipartPost(string id)
        {
            return new Page
            {
                Id = $"{id}",
                File = $"C:/_posts/series/{id}.md",
                Title = $"Post series {id}",
                Url = $"/posts/series/{id}"
            };
        }

        private static Page CreatePost(string id)
        {
            return new Page
            {
                Id = $"{id}",
                File = $"C:/_posts/{id}.md",
                Title = $"Post {id}",
                Url = $"/posts/{id}"
            };
        }

        private static string RenderTag(SiteContext siteContext, string id)
        {
            var result = string.Empty;
            var m = new MultipartPostTag(siteContext);
            m.Initialize(null, "", null);
            var liquidContext = new Context();
            liquidContext["page"] = new Hash { new KeyValuePair<string, object>("id", id) };

            using (var t = new StringWriter())
            {
                m.Render(liquidContext, t);
                result = t.ToString();
                var sb = t.GetStringBuilder();
                sb.Remove(0, sb.Length);
            }

            return result;
        }
    }
}