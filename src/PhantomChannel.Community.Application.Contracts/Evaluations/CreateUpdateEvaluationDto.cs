using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.Evaluations
{
    public class CreateUpdateEvaluationDto : EntityDto<Guid>
    {
        [Required]
        public required EvaluationEnum Target { get; set; }
        [Required]
        public required Guid TargetId { get; set; }
        public bool? Like { get; set; }
        public Guid? OwnerId { get; set; }
    }
}