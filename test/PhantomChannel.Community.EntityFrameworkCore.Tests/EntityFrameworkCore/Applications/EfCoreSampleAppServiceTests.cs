using PhantomChannel.Community.Samples;
using Xunit;

namespace PhantomChannel.Community.EntityFrameworkCore.Applications;

[Collection(CommunityTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<CommunityEntityFrameworkCoreTestModule>
{

}
