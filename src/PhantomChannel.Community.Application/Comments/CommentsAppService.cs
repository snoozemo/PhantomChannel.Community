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
using Volo.Abp.Identity;
using Volo.Abp.Data;
using Microsoft.AspNetCore.Mvc;
using PhantomChannel.Community.Evaluations;

namespace PhantomChannel.Community.Comments;

[Authorize(CommunityPermissions.Forums.Default)]
[Route("api/forums/comments")]
public class CommentsAppService(IRepository<Comment, Guid> repository, IRepository<Evaluation, Guid> evaluateRepository, IIdentityUserRepository userRepository) : ApplicationService, ICommentAppService
{
    private readonly IRepository<Comment, Guid> _repository = repository;
    private readonly IRepository<Evaluation, Guid> _evaluateRepository = evaluateRepository;
    private readonly IIdentityUserRepository _userRepository = userRepository;

    [HttpGet("{id}")]
    [Authorize(CommunityPermissions.Forums.Default)]
    public async Task<CommentDto> GetAsync(Guid id)
    {

        var comment = await _repository.GetAsync(id);
        var user = await _userRepository.GetAsync(comment.OwnerId);
        var result = ObjectMapper.Map<Comment, CommentDto>(comment);

        result.OwnerAvatar = user.GetProperty<string>("Avatar");
        result.OwnerName = user.UserName;
        return result;
    }
    [HttpGet("list")]
    [Authorize(CommunityPermissions.Forums.Default)]
    public async Task<PagedResultDto<CommentDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {

        var queryable = await _repository.GetQueryableAsync();
        var query = queryable
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "CreationTime desc" : input.Sorting)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //获取评论
        var comments = await AsyncExecuter.ToListAsync(query);
        var result = ObjectMapper.Map<List<Comment>, List<CommentDto>>(comments);

        // 获取用户信息
        var userIds = result.Select(p => p.OwnerId).Distinct().ToList();
        var users = await _userRepository.GetListByIdsAsync(userIds);
        var userDict = users.ToDictionary(u => u.Id);

        // 批量获取点赞状态
        var commentIds = result.Select(p => p.Id).ToList();
        var evaluations = await _evaluateRepository.GetListAsync(p => p.Target == EvaluationConst.Comment && commentIds.Contains(p.TargetId));
        var evaluationGroups = evaluations.GroupBy(e => e.TargetId).ToDictionary(g => g.Key, g => g.ToList());

        foreach (var item in result)
        {
            if (userDict.TryGetValue(item.OwnerId, out var user))
            {
                item.OwnerAvatar = user.GetProperty<string>("Avatar");
                item.OwnerName = user.UserName;
            }

            if (evaluationGroups.TryGetValue(item.Id, out var group))
            {
                item.LikeCount = group.Count;

                var currentEvaluation = group.Find(e => e.OwnerId == CurrentUser.Id!.Value);

                if (currentEvaluation != null)
                {
                    item.Liked = currentEvaluation.Like;
                }
            }
        }

        var totalCount = await AsyncExecuter.CountAsync(queryable);
        return new PagedResultDto<CommentDto>(totalCount, result);
    }

    [HttpGet("my-list")]
    [Authorize(CommunityPermissions.Forums.Default)]
    public async Task<PagedResultDto<CommentDto>> GetMyListAsync(PagedAndSortedResultRequestDto input)
    {

        var currentUser = CurrentUser;
        var queryable = await _repository.GetQueryableAsync();
        var query = queryable
            .Where(p => p.OwnerId == currentUser.Id!.Value)
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "CreationTime desc" : input.Sorting)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var comments = await AsyncExecuter.ToListAsync(query);
        var result = ObjectMapper.Map<List<Comment>, List<CommentDto>>(comments);

        var totalCount = await _repository.CountAsync(p => p.OwnerId == currentUser.Id!.Value);
        return new PagedResultDto<CommentDto>(totalCount, result);
    }

    [HttpPost("create")]
    [Authorize(CommunityPermissions.Forums.Create)]
    public async Task<CommentDto> CreateAsync(CreateUpdateCommentDto input)
    {
        var comment = ObjectMapper.Map<CreateUpdateCommentDto, Comment>(input);
        comment.OwnerId = CurrentUser.Id!.Value;
        await _repository.InsertAsync(comment);
        return ObjectMapper.Map<Comment, CommentDto>(comment);
    }

    [HttpPut("update")]
    [Authorize(CommunityPermissions.Forums.Edit)]
    public async Task<CommentDto> UpdateAsync(Guid id, CreateUpdateCommentDto input)
    {

        var currentUser = CurrentUser;
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

    [HttpDelete("delete")]
    [Authorize(CommunityPermissions.Forums.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var currentUser = CurrentUser;
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
