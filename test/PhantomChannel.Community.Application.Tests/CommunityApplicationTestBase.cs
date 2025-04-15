using Volo.Abp.Modularity;

namespace PhantomChannel.Community;

public abstract class CommunityApplicationTestBase<TStartupModule> : CommunityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
