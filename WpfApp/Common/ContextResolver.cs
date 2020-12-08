using WpfApp.DataAccess;
using WpfApp.Helpers;
using Unity;

namespace WpfApp.Common
{
    public class ContextResolver : IContextResolver
    {
        public WfpAppDbContext ResolveContext()
        {
            return ContainerHelper.Container.Resolve<WfpAppDbContext>();
        }
    }
}
