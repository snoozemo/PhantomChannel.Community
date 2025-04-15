using PhantomChannel.Community.Samples;
using Xunit;

namespace PhantomChannel.Community.EntityFrameworkCore.Domains;

[Collection(CommunityTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<CommunityEntityFrameworkCoreTestModule>
{

}
