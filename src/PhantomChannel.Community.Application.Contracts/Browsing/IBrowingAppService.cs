using System;
using Volo.Abp.Application.Services;

namespace PhantomChannel.Community.Browsing;

public interface IBrowsingAppService : IDeleteAppService<Guid>,
      IReadOnlyAppService<BrowsingDto, Guid>
{

}