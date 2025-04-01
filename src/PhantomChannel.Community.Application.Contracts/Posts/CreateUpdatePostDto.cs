using System;
using System.ComponentModel.DataAnnotations;

namespace PhantomChannel.Community.Posts;

public class CreateUpdatePostDto
{
    [Required]
    [StringLength(64)]
    public required string Title { get; set; } = string.Empty;

    [Required]
    public required Guid OwnerId { get; set; }

    [Required]
    [StringLength(5120)]
    public required string Content { get; set; } = string.Empty;
}