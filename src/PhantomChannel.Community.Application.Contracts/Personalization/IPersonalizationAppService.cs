using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace PhantomChannel.Community.Personalization;

public interface IPersonalizationAppService : IApplicationService

{
    // 设置 user 的 字段
    Task<PersonalizationDto> SetPersonalizationAsync(PersonalizationDto input, ICurrentUser currentUser);

}