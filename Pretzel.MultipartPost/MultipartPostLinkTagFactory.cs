// Pretzel.MultipartPost plugin
using Pretzel.Logic.Extensibility;

namespace Pretzel.MultipartPost
{
    public class MultipartPostLinkTagFactory : TagFactoryBase
    {
        public MultipartPostLinkTagFactory()
        : base("MultipartPostLink")
        {
        }

        public override ITag CreateTag()
        {
            return new MultipartPostLinkTag(this.SiteContext);
        }
    }
}