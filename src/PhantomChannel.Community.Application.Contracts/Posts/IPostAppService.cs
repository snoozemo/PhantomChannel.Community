using System;
using Volo.Abp.Application.Services;

namespace PhantomChannel.Community.Posts;

public interface IPostAppService : ICrudAppService<
        PostDto,
        Guid,
        PostPagedResultRequestDto,
        CreateUpdatePostDto>
{

}