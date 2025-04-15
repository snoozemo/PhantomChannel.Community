using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace PhantomChannel.Community.Comments;

public class Comment : AuditedAggregateRoot<Guid>
{
    public required Guid PostId { get; set; }
    public required Guid OwnerId { get; set; }
    public required string Content { get; set; }
    public Guid? ParentId { get; set; }
    public Guid? ReplyToId { get; set; }
    public int? Emotion { get; set; }

}