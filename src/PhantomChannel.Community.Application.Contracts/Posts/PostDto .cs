using System;
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.Posts;

public class PostDto : AuditedEntityDto<Guid>
{
    public string? Title { get; set; }
    public Guid OwnerId { get; set; }
    public string? Content { get; set; }
    public string[]? ImgUrls { get; set; } = [];

    public int? CommentsCount { get; set; }

    public int? LikeCount { get; set; }
    public int? ViewCount { get; set; }

    public string? OwnerName { get; set; }
    public string? OwnerAvatar { get; set; }
    public bool? Liked { get; set; }


}