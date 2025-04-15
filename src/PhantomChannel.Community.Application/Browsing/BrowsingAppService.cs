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

namespace PhantomChannel.Community.Browsing;

[Authorize(CommunityPermissions.Forums.Default)]
public class BrowsingAppService(IRepository<AuditLog, Guid> auditLogRepository) : ApplicationService, IBrowsingAppService
{
    private readonly IRepository<AuditLog, Guid> _auditLogRepository = auditLogRepository;


    public async Task<PagedResultDto<BrowsingDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        //TODO
        var currentUser = CurrentUser;
        var queryable = await _auditLogRepository.GetQueryableAsync();
        var query = queryable
            .Where(p => p.UserId == currentUser.Id!.Value)
            .OrderBy(input.Sorting.IsNullOrWhiteSpace() ? "ExecutionTime" : input.Sorting)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount);

        var browsings = await AsyncExecuter.ToListAsync(query);
        var totalCount = await AsyncExecuter.CountAsync(queryable);

        return new PagedResultDto<BrowsingDto>(
            totalCount,
            ObjectMapper.Map<List<AuditLog>, List<BrowsingDto>>(browsings)
        );
    }


    [Authorize(CommunityPermissions.Forums.Delete)]
    public async Task DeleteAsync(Guid id)
    {
        await _auditLogRepository.DeleteAsync(id);
    }
}
