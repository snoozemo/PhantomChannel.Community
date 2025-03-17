using Xunit;

namespace PhantomChannel.Community.EntityFrameworkCore;

[CollectionDefinition(CommunityTestConsts.CollectionDefinitionName)]
public class CommunityEntityFrameworkCoreCollection : ICollectionFixture<CommunityEntityFrameworkCoreFixture>
{

}
