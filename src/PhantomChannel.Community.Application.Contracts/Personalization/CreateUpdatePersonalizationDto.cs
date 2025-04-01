using System;
using System.ComponentModel.DataAnnotations;

namespace PhantomChannel.Community.Personalization;

public class CreateUpdatePersonalizationDto
{
    [Required]
    [Url]
    public required string Avatar { get; set; }

    [Required]
    [StringLength(248)]
    public required string Introduction { get; set; } = string.Empty;

}