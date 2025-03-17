using System;
using System.Collections.Generic;
using System.Text;
using PhantomChannel.Community.Localization;
using Volo.Abp.Application.Services;

namespace PhantomChannel.Community;

/* Inherit your application services from this class.
 */
public abstract class CommunityAppService : ApplicationService
{
    protected CommunityAppService()
    {
        LocalizationResource = typeof(CommunityResource);
    }
}
