using System;
using System.ComponentModel.DataAnnotations;

namespace PhantomChannel.Community.Comments;

public class CreateUpdateCommentDto
{
    [Required]
    public required Guid PostId { get; set; }

    [Required]
    public required Guid OwnerId { get; set; }

    [Required]
    [StringLength(1024)]
    public required string Content { get; set; } = string.Empty;

    public Guid? ParentId { get; set; } = null;

    public Guid? ReplyToId { get; set; } = null;

    public int? Emotion { get; set; }

}