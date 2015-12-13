// Pretzel.MultipartPost plugin
using Pretzel.Logic.Extensibility;

namespace Pretzel.MultipartPost
{
    public class IsMultipartPostBlockFactory : TagFactoryBase
    {
        public IsMultipartPostBlockFactory()
        : base("IsMultipartPost")
        {
        }

        public override ITag CreateTag()
        {
            return new IsMultipartPostBlock(this.SiteContext);
        }
    }
}