using Volo.Abp.Modularity;

namespace PhantomChannel.Community;

[DependsOn(
    typeof(CommunityDomainModule),
    typeof(CommunityTestBaseModule)
)]
public class CommunityDomainTestModule : AbpModule
{

}
