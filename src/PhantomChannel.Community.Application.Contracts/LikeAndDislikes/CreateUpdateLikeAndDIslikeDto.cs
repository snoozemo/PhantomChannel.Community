using System;
using System.ComponentModel.DataAnnotations;

namespace PhantomChannel.Community.LikeAndDislikes;

public class CreateUpdateLikeAndDislikeDto
{
    [Required]
    public required Guid PostId { get; set; }
    [Required]
    public required Guid OwnerId { get; set; }

    [Required]
    public required TargetType Target { get; set; }
    [Required]
    public required LikeOrDislikeType LikeOrDislike { get; set; }
}