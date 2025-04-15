using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace PhantomChannel.Community.Notices
{
    public class CreateUpdateNoticeDto : EntityDto<Guid>
    {
        public Guid OwnerId { get; set; }
        [StringLength(64)]
        public string NoticeTitle { get; set; } = default!;
        [StringLength(512)]
        public string NoticeContent { get; set; } = default!;
        public required bool Checked { get; set; } = false;
    }
}