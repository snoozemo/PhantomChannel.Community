using System;
using Volo.Abp.Domain.Entities.Auditing;

namespace PhantomChannel.Community.Evaluations;

public class Evaluation : AuditedAggregateRoot<Guid>
{
    public required int Target { get; set; }
    public required Guid TargetId { get; set; }
    public bool? Like { get; set; }
    public required Guid OwnerId { get; set; }
}