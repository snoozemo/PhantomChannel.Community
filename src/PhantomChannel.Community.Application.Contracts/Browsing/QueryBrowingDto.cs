using System;
using System.ComponentModel.DataAnnotations;

namespace PhantomChannel.Community.Browsing;

public class QueryBrowingDto
{
    [Required]
    public required Guid UserId { get; set; }

    [Required]
    public required string Url { get; set; } = string.Empty;

}