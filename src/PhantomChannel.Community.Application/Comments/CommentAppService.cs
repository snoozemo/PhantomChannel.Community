using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PhantomChannel.Community.Permissions;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using System.Linq.Dynamic.Core;
using Volo.Abp.Users;

namespace PhantomChannel.Community.Comments;

[Authorize(CommunityPermissions.Forums.Default)]
public class CommentAppService(IRepository<Comment, Guid> repository) : ApplicationService, ICommentAppService
{
    private readonly IRepository<Comment, Guid> _repository = repository;

    public async Task<CommentDto> GetAsync(Guid id)
    {
        var comment = await _repository.GetAsync(id);
        return ObjectMapper.Map<Comment, CommentDto>(comment);
    }

    public async Task<PagedResultDto<CommentDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var queryable = await _repository.GetQueryableAsync();
        var query = queryable
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "CreationTime desc" : input.Sorting)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var comments = await AsyncExecuter.ToListAsync(query);
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        return new PagedResultDto<CommentDto>(
            totalCount,
            ObjectMapper.Map<List<Comment>, List<CommentDto>>(comments)
        );
    }

    [Authorize(CommunityPermissions.Forums.Create)]
    public async Task<CommentDto> CreateAsync(CreateUpdateCommentDto input)
    {
        var comment = ObjectMapper.Map<CreateUpdateCommentDto, Comment>(input);
        await _repository.InsertAsync(comment);
        return ObjectMapper.Map<Comment, CommentDto>(comment);
    }

    [Authorize(CommunityPermissions.Forums.Edit)]
    public async Task<CommentDto> UpdateAsync(Guid id, CreateUpdateCommentDto input, ICurrentUser currentUser)
    {


        var comment = await _repository.GetAsync(id);
        var isAdmin = currentUser.IsInRole("admin");
        var isOwner = comment.OwnerId == currentUser.Id!.Value;

        if (!isAdmin && !isOwner)
        {
            throw new Exception("没有权限");
        }

        ObjectMapper.Map(input, comment);
        await _repository.UpdateAsync(comment);
        return ObjectMapper.Map<Comment, CommentDto>(comment);
    }

    [Authorize(CommunityPermissions.Forums.Delete)]
    public async Task DeleteAsync(Guid id, ICurrentUser currentUser)
    {
        var comment = await _repository.GetAsync(id);
        var isAdmin = currentUser.IsInRole("admin");
        var isOwner = comment.OwnerId == currentUser.Id!.Value;

        if (!isAdmin && !isOwner)
        {
            throw new Exception("没有权限");
        }

        await _repository.DeleteAsync(id);
    }
}
