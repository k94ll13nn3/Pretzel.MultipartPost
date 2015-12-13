using DotLiquid;
using NUnit.Framework;
using Pretzel.Logic.Templating.Context;
using System;
using System.Collections.Generic;
using System.IO;

namespace Pretzel.MultipartPost.Tests
{
    [TestFixture]
    public class MultipartPostTests
    {
        [Test]
        public void TestMultiPartPostLinkGoodPatterns()
        {
            var tagFactory = new MultipartPostLinkTagFactory();
            Template.RegisterTagFactory(tagFactory);

            Assert.DoesNotThrow(() => Template.Parse("{% multipart_post_link prev %}"));
            Assert.DoesNotThrow(() => Template.Parse("{% multipart_post_link next %}"));

            Assert.Throws<ArgumentException>(() => Template.Parse("{% multipart_post_link  %}"));
            Assert.Throws<ArgumentException>(() => Template.Parse("{% multipart_post_link string %}"));
            Assert.Throws<ArgumentException>(() => Template.Parse("{% multipart_post_link pre %}"));
            Assert.Throws<ArgumentException>(() => Template.Parse("{% multipart_post_link nexte %}"));
            Assert.Throws<ArgumentException>(() => Template.Parse("{% multipart_post_link next lala %}"));
        }

        [Test]
        public void TestMultiPartPostPatterns()
        {
            var tagFactory = new MultipartPostTagFactory();
            Template.RegisterTagFactory(tagFactory);

            Assert.DoesNotThrow(() => Template.Parse("{% multipart_post %}"));
            Assert.DoesNotThrow(() => Template.Parse("{% multipart_post asc %}"));
            Assert.DoesNotThrow(() => Template.Parse("{% multipart_post desc %}"));
            Assert.DoesNotThrow(() => Template.Parse("{% multipart_post wasc %}"));
            Assert.DoesNotThrow(() => Template.Parse("{% multipart_post wdesc %}"));

            Assert.Throws<ArgumentException>(() => Template.Parse("{% multipart_post string %}"));
            Assert.Throws<ArgumentException>(() => Template.Parse("{% multipart_post as %}"));
            Assert.Throws<ArgumentException>(() => Template.Parse("{% multipart_post desc a %}"));
        }

        [Test]
        public void TestRenderIsMultipartPost()
        {
            var siteContext = CreateContext();
            var tag = new IsMultipartPostBlock(siteContext);

            // If there is a NullReferenceException, it means that the base.Render() was call
            // so that the post is a multi part post
            Assert.DoesNotThrow(() => RenderTag(tag, "0"));
            Assert.Throws<NullReferenceException>(() => RenderTag(tag, "1"));
            Assert.Throws<NullReferenceException>(() => RenderTag(tag, "2"));
        }

        [Test]
        public void TestRenderMultipartPost()
        {
            const string postOneHtml = "<li><a href=\"/posts/series/1\">Post series 1</a></li>";
            const string postTwoHtml = "<li><a href=\"/posts/series/2\">Post series 2</a></li>";
            const string postThreeHtml = "<li><a href=\"/posts/series/3\">Post series 3</a></li>";
            const string postOneCurrentHtml = "<li><a class=\"current-post\" href=\"/posts/series/1\">Post series 1</a></li>";
            const string postTwoCurrentHtml = "<li><a class=\"current-post\" href=\"/posts/series/2\">Post series 2</a></li>";
            var siteContext = CreateContext();
            var tag = new MultipartPostTag(siteContext);

            Assert.AreEqual(string.Empty, RenderTag(tag, "0", true, ""));
            Assert.AreEqual($"<ul class=\"multipart-post-list\">{postOneCurrentHtml}{postTwoHtml}{postThreeHtml}</ul>", RenderTag(tag, "1", true, ""));
            Assert.AreEqual($"<ul class=\"multipart-post-list\">{postOneHtml}{postTwoCurrentHtml}{postThreeHtml}</ul>", RenderTag(tag, "2", true, ""));

            Assert.AreEqual(string.Empty, RenderTag(tag, "0", true, "asc"));
            Assert.AreEqual($"<ul class=\"multipart-post-list\">{postOneCurrentHtml}{postTwoHtml}{postThreeHtml}</ul>", RenderTag(tag, "1", true, "asc"));
            Assert.AreEqual($"<ul class=\"multipart-post-list\">{postOneHtml}{postTwoCurrentHtml}{postThreeHtml}</ul>", RenderTag(tag, "2", true, "asc"));

            Assert.AreEqual(string.Empty, RenderTag(tag, "0", true, "desc"));
            Assert.AreEqual($"<ul class=\"multipart-post-list\">{postThreeHtml}{postTwoHtml}{postOneCurrentHtml}</ul>", RenderTag(tag, "1", true, "desc"));
            Assert.AreEqual($"<ul class=\"multipart-post-list\">{postThreeHtml}{postTwoCurrentHtml}{postOneHtml}</ul>", RenderTag(tag, "2", true, "desc"));

            Assert.AreEqual(string.Empty, RenderTag(tag, "0", true, "wasc"));
            Assert.AreEqual($"<ul class=\"multipart-post-list\">{postTwoHtml}{postThreeHtml}</ul>", RenderTag(tag, "1", true, "wasc"));
            Assert.AreEqual($"<ul class=\"multipart-post-list\">{postOneHtml}{postThreeHtml}</ul>", RenderTag(tag, "2", true, "wasc"));

            Assert.AreEqual(string.Empty, RenderTag(tag, "0", true, "wdesc"));
            Assert.AreEqual($"<ul class=\"multipart-post-list\">{postThreeHtml}{postTwoHtml}</ul>", RenderTag(tag, "1", true, "wdesc"));
            Assert.AreEqual($"<ul class=\"multipart-post-list\">{postThreeHtml}{postOneHtml}</ul>", RenderTag(tag, "2", true, "wdesc"));
        }

        [Test]
        public void TestRenderMultipartPostLink()
        {
            const string postOneRendered = "/posts/series/1";
            const string postTwoRendered = "/posts/series/2";
            const string postThreeRendered = "/posts/series/3";
            var siteContext = CreateContext();
            var tag = new MultipartPostLinkTag(siteContext);

            Assert.AreEqual(string.Empty, RenderTag(tag, "0", true, "prev"));

            Assert.Throws<ArgumentException>(() => RenderTag(tag, "1", true, "prev"));
            Assert.AreEqual($"{postTwoRendered}", RenderTag(tag, "1", true, "next"));

            Assert.AreEqual($"{postOneRendered}", RenderTag(tag, "2", true, "prev"));
            Assert.AreEqual($"{postThreeRendered}", RenderTag(tag, "2", true, "next"));

            Assert.AreEqual($"{postTwoRendered}", RenderTag(tag, "3", true, "prev"));
            Assert.Throws<ArgumentException>(() => RenderTag(tag, "3", true, "next"));
        }

        [Test]
        public void TestRenderMultipartPostUniquePostInSeries()
        {
            var siteContext = CreateContextUniquePostInSeries();
            var tag = new MultipartPostTag(siteContext);

            Assert.AreEqual(string.Empty, RenderTag(tag, "1", true, ""));
            Assert.AreEqual(string.Empty, RenderTag(tag, "1", true, "asc"));
            Assert.AreEqual(string.Empty, RenderTag(tag, "1", true, "desc"));
            Assert.AreEqual(string.Empty, RenderTag(tag, "1", true, "wasc"));
            Assert.AreEqual(string.Empty, RenderTag(tag, "1", true, "wdesc"));
        }

        private static SiteContext CreateContext()
        {
            var pageZero = CreatePost("0");
            var pageOne = CreateMultipartPost("1");
            var pageTwo = CreateMultipartPost("2");
            var pageThree = CreateMultipartPost("3");

            pageOne.DirectoryPages = new List<Page> { pageOne, pageTwo, pageThree };
            pageTwo.DirectoryPages = new List<Page> { pageOne, pageTwo, pageThree };
            pageThree.DirectoryPages = new List<Page> { pageOne, pageTwo, pageThree };

            var siteContext = new SiteContext();
            siteContext.Posts.Add(pageZero);
            siteContext.Posts.Add(pageOne);
            siteContext.Posts.Add(pageTwo);
            siteContext.Posts.Add(pageThree);

            return siteContext;
        }

        private static SiteContext CreateContextUniquePostInSeries()
        {
            var pageOne = CreateMultipartPost("1");

            pageOne.DirectoryPages = new List<Page> { pageOne };

            var siteContext = new SiteContext();
            siteContext.Posts.Add(pageOne);

            return siteContext;
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

        private static string RenderTag(DotLiquid.Tag tag, string id)
        {
            return RenderTag(tag, id, false, "");
        }

        private static string RenderTag(DotLiquid.Tag tag, string id, bool initialize, string markup)
        {
            var result = string.Empty;

            if (initialize)
            {
                tag.Initialize(null, markup, null);
            }

            var liquidContext = new Context();
            liquidContext["page"] = new Hash { new KeyValuePair<string, object>("id", id) };

            using (var t = new StringWriter())
            {
                tag.Render(liquidContext, t);
                result = t.ToString();
                var sb = t.GetStringBuilder();
                sb.Remove(0, sb.Length);
            }

            return result;
        }
    }
}