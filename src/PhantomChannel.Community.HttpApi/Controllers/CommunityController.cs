using PhantomChannel.Community.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace PhantomChannel.Community.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class CommunityController : AbpControllerBase
{
    protected CommunityController()
    {
        LocalizationResource = typeof(CommunityResource);
    }
}
