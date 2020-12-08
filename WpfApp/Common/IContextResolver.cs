using WpfApp.DataAccess;

namespace WpfApp.Common
{
    public interface IContextResolver
    {
        WfpAppDbContext ResolveContext();
    }
}