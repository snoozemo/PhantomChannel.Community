using System;
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.LikeAndDislikes;

public class LikeAndDislikeDto : AuditedEntityDto<Guid>
{
    public required Guid PostId { get; set; }
    public required Guid OwnerId { get; set; }
    public required TargetType Target { get; set; }
    public required LikeOrDislikeType LikeOrDislike { get; set; }

}