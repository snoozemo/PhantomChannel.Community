using Microsoft.Extensions.Localization;
using PhantomChannel.Community.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace PhantomChannel.Community.Blazor.Client;

[Dependency(ReplaceServices = true)]
public class CommunityBrandingProvider(IStringLocalizer<CommunityResource> localizer) : DefaultBrandingProvider
{
    private IStringLocalizer<CommunityResource> _localizer = localizer;

    public override string AppName => _localizer["AppName"];
}
