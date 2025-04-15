using Microsoft.Extensions.Localization;
using PhantomChannel.Community.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace PhantomChannel.Community;

[Dependency(ReplaceServices = true)]
public class CommunityBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<CommunityResource> _localizer;

    public CommunityBrandingProvider(IStringLocalizer<CommunityResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
