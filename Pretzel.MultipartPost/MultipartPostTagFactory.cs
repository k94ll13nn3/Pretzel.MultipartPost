using Pretzel.Logic.Extensibility;

namespace Pretzel.MultipartPost
{
    public class MultipartPostTagFactory : TagFactoryBase
    {
        public MultipartPostTagFactory()
        : base("MultipartPost")
        {
        }

        public override ITag CreateTag()
        {
            return new MultipartPostTag(this.SiteContext);
        }
    }
}