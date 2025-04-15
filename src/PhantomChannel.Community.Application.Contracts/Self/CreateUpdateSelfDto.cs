using System;
using System.ComponentModel.DataAnnotations;

namespace PhantomChannel.Community.Self;

public class CreateUpdateSelfDto
{
    [Url]
    public string Avatar { get; set; } = string.Empty;

    [StringLength(248)]
    public string Introduction { get; set; } = string.Empty;

}