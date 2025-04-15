using System;
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.Comments;

public class CommentDto : AuditedEntityDto<Guid>
{
    public required Guid PostId { get; set; }
    public required Guid OwnerId { get; set; }
    public required string Content { get; set; }
    public Guid? ParentId { get; set; }

    public Guid? ReplyToId { get; set; }

    public int? Emotion { get; set; }

    public int? LikeCount { get; set; }
    public int? DislikeCount { get; set; }

    public string? OwnerName { get; set; }
    public string? OwnerAvatar { get; set; }
    public bool? Liked { get; set; }


}