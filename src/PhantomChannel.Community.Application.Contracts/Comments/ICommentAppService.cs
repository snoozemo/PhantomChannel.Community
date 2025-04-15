using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PhantomChannel.Community.Comments;

public interface ICommentAppService : ICrudAppService<
        CommentDto,
        Guid,
        PagedAndSortedResultRequestDto,
        CreateUpdateCommentDto>
{

}