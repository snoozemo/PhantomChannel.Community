using System;
using System.ComponentModel.DataAnnotations;

namespace PhantomChannel.Community.Posts;

public class CreateUpdatePostDto
{
    [StringLength(64)]
    public string Title { get; set; } = string.Empty;

    public Guid OwnerId { get; set; }

    [StringLength(5120)]
    public string Content { get; set; } = string.Empty;

    public string[]? ImgUrls { get; set; } = [];

}