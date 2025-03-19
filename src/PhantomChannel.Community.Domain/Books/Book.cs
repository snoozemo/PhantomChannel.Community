using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace PhantomChannel.Community.Books;

public class Book : AuditedAggregateRoot<Guid>
{
    public required string Name { get; set; }

    public BookType Type { get; set; }

    public DateTime PublishDate { get; set; }

    public float Price { get; set; }

    public string? AuthorName2 { get; set; }
}