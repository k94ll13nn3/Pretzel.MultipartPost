using DotLiquid;
using NUnit.Framework;
using System;

namespace Pretzel.MultipartPost.Tests
{
    [TestFixture]
    // Not working because of the custom DotLiquid.dll
    public class MultipartPostTagTests
    {
        [Test]
        public void TestWrongPatterns()
        {
            var tagFactory = new MultipartPostTagFactory();
            Template.RegisterTagFactory(tagFactory);

            Assert.Throws<ArgumentException>(() => Template.Parse("{% multipart_post string %}"));
            Assert.Throws<ArgumentException>(() => Template.Parse("{% multipart_post as %}"));
            Assert.Throws<ArgumentException>(() => Template.Parse("{% multipart_post desc a %}"));
        }

        [Test]
        public void TestGoodPatterns()
        {
            var tagFactory = new MultipartPostTagFactory();
            Template.RegisterTagFactory(tagFactory);

            Assert.DoesNotThrow(() => Template.Parse("{% multipart_post  %}"));
            Assert.DoesNotThrow(() => Template.Parse("{% multipart_post asc %}"));
            Assert.DoesNotThrow(() => Template.Parse("{% multipart_post desc %}"));
        }
    }
}