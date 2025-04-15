using PhantomChannel.Community.Localization;
using Volo.Abp.AspNetCore.Components;

namespace PhantomChannel.Community.Blazor.Client;

public abstract class CommunityComponentBase : AbpComponentBase
{
    protected CommunityComponentBase()
    {
        LocalizationResource = typeof(CommunityResource);
    }
}
