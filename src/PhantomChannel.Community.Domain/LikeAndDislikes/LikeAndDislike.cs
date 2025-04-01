using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace PhantomChannel.Community.LikeAndDislikes;

public class LikeAndDislike : AuditedAggregateRoot<Guid>
{
    public required Guid PostId { get; set; }
    public required Guid OwnerId { get; set; }
    public required TargetType Target { get; set; }
    public required LikeOrDislikeType LikeOrDislike { get; set; }
}