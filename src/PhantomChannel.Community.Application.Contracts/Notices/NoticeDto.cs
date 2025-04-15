using System;
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.Notices;

public class NoticeDto : AuditedEntityDto<Guid>
{
    public Guid OwnerId { get; set; }
    public string NoticeTitle { get; set; } = default!;
    public string NoticeContent { get; set; } = default!;
    public required bool Checked { get; set; } = false;
}