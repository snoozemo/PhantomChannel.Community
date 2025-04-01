using System;
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.Personalization;

public class PersonalizationDto : AuditedEntityDto<Guid>
{
    public required string Avatar { get; set; }
    public required string Introduction { get; set; }

}