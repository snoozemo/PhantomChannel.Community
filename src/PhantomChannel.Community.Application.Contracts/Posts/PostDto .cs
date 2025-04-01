using System;
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.Posts;

public class PostDto : AuditedEntityDto<Guid>
{
    public required string Title { get; set; }
    public required Guid OwnerId { get; set; }
    public required string Content { get; set; }

}