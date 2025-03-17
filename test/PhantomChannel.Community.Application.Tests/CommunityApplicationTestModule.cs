using Volo.Abp.Modularity;

namespace PhantomChannel.Community;

[DependsOn(
    typeof(CommunityApplicationModule),
    typeof(CommunityDomainTestModule)
)]
public class CommunityApplicationTestModule : AbpModule
{

}
