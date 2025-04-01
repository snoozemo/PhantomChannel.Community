using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PhantomChannel.Community.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PhantomChannel.Community.LikeAndDislikes;

[Authorize(CommunityPermissions.Forums.Default)]
public class LikeAndDislikeAppService(IRepository<LikeAndDislike, Guid> repository) : ApplicationService, ILikeAndDislikeAppService
{
    private readonly IRepository<LikeAndDislike, Guid> _repository = repository;



    [Authorize(CommunityPermissions.Forums.Create)]
    public async Task<LikeAndDislikeDto> CreateAsync(CreateUpdateLikeAndDislikeDto input)
    {
        var likeAndDislike = ObjectMapper.Map<CreateUpdateLikeAndDislikeDto, LikeAndDislike>(input);
        await _repository.InsertAsync(likeAndDislike);
        return ObjectMapper.Map<LikeAndDislike, LikeAndDislikeDto>(likeAndDislike);
    }


    [Authorize(CommunityPermissions.Forums.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }


}
