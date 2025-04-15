using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace PhantomChannel.Community.Notices;

public class Notice : AuditedAggregateRoot<Guid>
{
    public Guid OwnerId { get; set; }
    public string NoticeTitle { get; set; } = default!;
    public string NoticeContent { get; set; } = default!;
    public required bool Checked { get; set; } = false;
}