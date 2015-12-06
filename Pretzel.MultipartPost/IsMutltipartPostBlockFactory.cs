using Pretzel.Logic.Extensibility;

namespace Pretzel.MultipartPost
{
    public class IsMutltipartPostBlockFactory : TagFactoryBase
    {
        public IsMutltipartPostBlockFactory()
        : base("IsMultipartPost")
        {
        }

        public override ITag CreateTag()
        {
            return new IsMutltipartPostBlock(this.SiteContext);
        }
    }
}