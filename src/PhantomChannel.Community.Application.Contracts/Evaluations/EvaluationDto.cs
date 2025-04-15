using System;
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.Evaluations;

public class EvaluationDto : AuditedEntityDto<Guid>
{
    public required int Target { get; set; }
    public required Guid TargetId { get; set; }
    public bool? Like { get; set; }
    public required Guid OwnerId { get; set; }
}