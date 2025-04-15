using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using System;
using Microsoft.AspNetCore.Authorization;
using PhantomChannel.Community.Permissions;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;

namespace PhantomChannel.Community.Evaluations;

[Route("api/forums/evaluations")]
[Authorize(CommunityPermissions.Forums.Default)]
public class EvaluationsAppService(IRepository<Evaluation, Guid> repository) : ApplicationService, IApplicationService
{

    private readonly IRepository<Evaluation, Guid> _repository = repository;

    [Authorize(CommunityPermissions.Forums.Create)]
    [HttpPost("post/{id}")]
    public async Task<bool> LikePostAsync(Guid id)
    {
        var post = await _repository.FirstOrDefaultAsync(e => e.Target == EvaluationConst.Post && e.TargetId == id && e.OwnerId == CurrentUser.Id!.Value);
        if (post != null)
        {
            await _repository.DeleteAsync(post.Id);
        }
        else
        {
            await _repository.InsertAsync(new Evaluation()
            {
                OwnerId = CurrentUser.Id!.Value,
                Target = EvaluationConst.Post,
                TargetId = id,
                Like = true
            });
        }
        return true;
    }
    [Authorize(CommunityPermissions.Forums.Create)]
    [HttpPost("comment/{id}")]
    public async Task<bool> LikeCommentAsync(Guid id)
    {
        var comment = await _repository.FirstOrDefaultAsync(e => e.Target == EvaluationConst.Comment && e.TargetId == id && e.OwnerId == CurrentUser.Id!.Value);
        if (comment != null)
        {
            await _repository.DeleteAsync(comment.Id);
        }
        else
        {
            await _repository.InsertAsync(new Evaluation()
            {
                OwnerId = CurrentUser.Id!.Value,
                Target = EvaluationConst.Post,
                TargetId = id,
                Like = true
            });
        }
        return true;
    }
}
