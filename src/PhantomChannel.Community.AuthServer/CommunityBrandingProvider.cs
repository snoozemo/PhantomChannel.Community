using Microsoft.Extensions.Localization;
using PhantomChannel.Community.Localization;
using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace PhantomChannel.Community;

[Dependency(ReplaceServices = true)]
public class CommunityBrandingProvider(IStringLocalizer<CommunityResource> localizer) : DefaultBrandingProvider
{
    private readonly IStringLocalizer<CommunityResource> _localizer = localizer;

    public override string AppName => _localizer["AppName"];
}
