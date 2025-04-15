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
using Volo.Abp.AuditLogging;
using Volo.Abp.Identity;
using Volo.Abp.Data;
using PhantomChannel.Community.Comments;
using PhantomChannel.Community.Evaluations;
using Microsoft.AspNetCore.Mvc;

namespace PhantomChannel.Community.Posts;

[Route("api/forums/posts")]
[Authorize(CommunityPermissions.Forums.Default)]
public class PostsAppService(IRepository<Post, Guid> repository, IRepository<AuditLog, Guid> auditLogRepository, IIdentityUserRepository userRepository, IRepository<Comment, Guid> commentRepository, IRepository<Evaluation, Guid> evaluateRepository) : ApplicationService, IPostAppService
{
    private readonly IRepository<Post, Guid> _repository = repository;
    private readonly IRepository<AuditLog, Guid> _auditLogRepository = auditLogRepository;
    private readonly IRepository<Evaluation, Guid> _evaluateRepository = evaluateRepository;

    private readonly IIdentityUserRepository _userRepository = userRepository;
    private readonly IRepository<Comment, Guid> _commentRepository = commentRepository;

    [HttpGet("{id}")]
    [Authorize(CommunityPermissions.Forums.Default)]
    public async Task<PostDto> GetAsync(Guid id)
    {
        var post = await _repository.GetAsync(id);
        var user = await _userRepository.GetAsync(post.OwnerId);
        var currentUser = CurrentUser;
        var result = ObjectMapper.Map<Post, PostDto>(post);
        var viewCount = await _auditLogRepository.CountAsync(a => a.ClientId == "Forums" && a.Url == "/api/forums/posts/" + id);
        var likeCount = await _evaluateRepository.CountAsync(e => e.Target == EvaluationConst.Post && e.TargetId == id && e.Like == true);
        var currentUserEvaluation = await _evaluateRepository.FirstOrDefaultAsync(e => e.OwnerId == currentUser.Id!.Value && e.Target == EvaluationConst.Post && e.TargetId == id);

        result.OwnerAvatar = user.GetProperty<string>("Avatar");
        result.OwnerName = user.UserName;
        result.ViewCount = viewCount;
        result.LikeCount = likeCount;
        if (currentUserEvaluation != null)
        {
            result.Liked = currentUserEvaluation.Like;
        }
        return result;
    }
    [Authorize(CommunityPermissions.Forums.Default)]
    [HttpGet("list")]
    public async Task<PagedResultDto<PostDto>> GetListAsync(PostPagedResultRequestDto input)
    {
        var queryable = await _repository.GetQueryableAsync();

        if (!string.IsNullOrWhiteSpace(input.Filter))
        {
            queryable = queryable.Where(p => p.Title.Contains(input.Filter) || p.Content.Contains(input.Filter));
        }

        var query = queryable
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "CreationTime desc" : input.Sorting)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        //获取帖子
        var posts = await AsyncExecuter.ToListAsync(query);
        var result = ObjectMapper.Map<List<Post>, List<PostDto>>(posts);
        var postIds = result.Select(p => p.Id).ToList();

        //获取用户
        var userIds = result.Select(p => p.OwnerId).Distinct().ToList();
        var users = await _userRepository.GetListByIdsAsync(userIds);
        var userDict = users.ToDictionary(u => u.Id);

        // 获取点赞
        var evaluations = await _evaluateRepository.GetListAsync(p => p.Target == EvaluationConst.Post && postIds.Contains(p.TargetId) && p.Like == true);
        var evaluationGroups = evaluations.GroupBy(e => e.TargetId).ToDictionary(g => g.Key, g => g.Count());

        // 阅读量
        var viewCounts = await _auditLogRepository.GetListAsync(a => a.ClientId == "Forums" &&
            postIds.Select(p => p.ToString()).ToArray().Contains(a.Url.Replace("/api/forums/posts/", "")));
        var viewCountDict = viewCounts
            .GroupBy(v => v.Url.Replace("/api/forums/posts/", ""))
            .ToDictionary(g => g.Key, g => g.Count());

        // 获取评论量
        var comments = await _commentRepository.GetListAsync(c => postIds.Contains(c.PostId));
        var commentCounts = comments
            .GroupBy(c => c.PostId)
            .ToDictionary(g => g.Key, g => g.Count());

        foreach (var item in result)
        {

            item.OwnerAvatar = userDict.TryGetValue(item.OwnerId, out var user) ? user.GetProperty<string>("Avatar") : null;
            item.OwnerName = user?.UserName ?? null;
            item.ViewCount = viewCountDict.TryGetValue(item.Id.ToString(), out var viewCount) ? viewCount : 0;
            item.CommentsCount = commentCounts.TryGetValue(item.Id, out var count) ? count : 0;
            item.LikeCount = evaluationGroups.TryGetValue(item.Id, out var likeCount) ? likeCount : 0;

        }

        var totalCount = await AsyncExecuter.CountAsync(queryable);
        return new PagedResultDto<PostDto>(totalCount, result);

    }
    [HttpGet("my-list")]
    [Authorize(CommunityPermissions.Forums.Default)]
    public async Task<PagedResultDto<PostDto>> GetMyListAsync(PagedAndSortedResultRequestDto input)
    {

        var currentUser = CurrentUser;
        var queryable = await _repository.GetQueryableAsync();
        var query = queryable
            .Where(p => p.OwnerId == currentUser.Id!.Value)
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "CreationTime desc" : input.Sorting)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var posts = await AsyncExecuter.ToListAsync(query);
        var result = ObjectMapper.Map<List<Post>, List<PostDto>>(posts);

        var totalCount = await _repository.CountAsync(p => p.OwnerId == currentUser.Id!.Value);
        return new PagedResultDto<PostDto>(totalCount, result);
    }

    [HttpPost("create")]
    [Authorize(CommunityPermissions.Forums.Create)]
    public async Task<PostDto> CreateAsync(CreateUpdatePostDto input)
    {
        var post = ObjectMapper.Map<CreateUpdatePostDto, Post>(input);
        post.OwnerId = CurrentUser.Id!.Value;
        await _repository.InsertAsync(post);
        return ObjectMapper.Map<Post, PostDto>(post);
    }

    [HttpPut("update")]
    [Authorize(CommunityPermissions.Forums.Edit)]
    public async Task<PostDto> UpdateAsync(Guid id, CreateUpdatePostDto input)
    {
        var currentUser = CurrentUser;
        var post = await _repository.GetAsync(id);
        var isAdmin = currentUser.IsInRole("admin");
        var isOwner = post.OwnerId == currentUser.Id!.Value;

        if (!isAdmin && !isOwner)
        {
            throw new Exception("没有权限");
        }
        ObjectMapper.Map(input, post);
        await _repository.UpdateAsync(post);
        return ObjectMapper.Map<Post, PostDto>(post);
    }

    [HttpDelete("delete")]
    [Authorize(CommunityPermissions.Forums.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        var currentUser = CurrentUser;
        var post = await _repository.GetAsync(id);
        var isAdmin = currentUser.IsInRole("admin");
        var isOwner = post.OwnerId == currentUser.Id!.Value;

        if (!isAdmin && !isOwner)
        {
            throw new Exception("没有权限");
        }

        await _repository.DeleteAsync(id);
    }

}
