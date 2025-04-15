using Volo.Abp.Modularity;

namespace PhantomChannel.Community;

/* Inherit from this class for your domain layer tests. */
public abstract class CommunityDomainTestBase<TStartupModule> : CommunityTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
