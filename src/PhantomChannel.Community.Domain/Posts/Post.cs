using System;
using System.Numerics;
using Volo.Abp.Domain.Entities.Auditing;

namespace PhantomChannel.Community.Posts;

public class Post : AuditedAggregateRoot<Guid>
{

    public required string Title { get; set; }
    public required Guid OwnerId { get; set; }
    public required string Content { get; set; }
    public string[]? ImgUrls { get; set; } = [];

}