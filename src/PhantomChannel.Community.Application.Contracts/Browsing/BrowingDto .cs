using System;
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.Browsing;

public class BrowsingDto : AuditedEntityDto<Guid>
{

    public required string Url { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }


}