using System;
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.Books;

public class BookDto : AuditedEntityDto<Guid>
{
    public required string Name { get; set; }

    public BookType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }
}