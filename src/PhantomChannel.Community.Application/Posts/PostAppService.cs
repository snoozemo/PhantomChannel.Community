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

namespace PhantomChannel.Community.Posts;

[Authorize(CommunityPermissions.Forums.Default)]
public class PostAppService(IRepository<Post, Guid> repository) : ApplicationService, IPostAppService
{
    private readonly IRepository<Post, Guid> _repository = repository;

    public async Task<PostDto> GetAsync(Guid id)
    {
        var post = await _repository.GetAsync(id);
        return ObjectMapper.Map<Post, PostDto>(post);
    }

    public async Task<PagedResultDto<PostDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var queryable = await _repository.GetQueryableAsync();
        var query = queryable
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "CreationTime desc" : input.Sorting)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var posts = await AsyncExecuter.ToListAsync(query);
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        return new PagedResultDto<PostDto>(
            totalCount,
            ObjectMapper.Map<List<Post>, List<PostDto>>(posts)
        );
    }

    [Authorize(CommunityPermissions.Forums.Create)]
    public async Task<PostDto> CreateAsync(CreateUpdatePostDto input)
    {
        var post = ObjectMapper.Map<CreateUpdatePostDto, Post>(input);
        await _repository.InsertAsync(post);
        return ObjectMapper.Map<Post, PostDto>(post);
    }

    [Authorize(CommunityPermissions.Forums.Edit)]
    public async Task<PostDto> UpdateAsync(Guid id, CreateUpdatePostDto input, ICurrentUser currentUser)
    {
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

    [Authorize(CommunityPermissions.Forums.Delete)]
    public async Task DeleteAsync(Guid id, ICurrentUser currentUser)
    {
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
