using PhantomChannel.Community.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Caching.StackExchangeRedis;

namespace PhantomChannel.Community.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(CommunityEntityFrameworkCoreModule),
    typeof(CommunityApplicationContractsModule)
)]
[DependsOn(typeof(AbpCachingStackExchangeRedisModule))]
    public class CommunityDbMigratorModule : AbpModule
{
}
