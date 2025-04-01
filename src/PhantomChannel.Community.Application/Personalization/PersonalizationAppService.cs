using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhantomChannel.Community.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace PhantomChannel.Community.Personalization;

[Authorize(CommunityPermissions.Forums.Default)]
public class PersonalizationAppService(IIdentityUserRepository userRepository) : ApplicationService, IPersonalizationAppService
{
    private readonly IIdentityUserRepository _userRepository = userRepository;

    [Authorize(CommunityPermissions.Forums.Edit)]
    [HttpPut]
    public async Task<PersonalizationDto> SetPersonalizationAsync(PersonalizationDto input, ICurrentUser currentUser)
    {

        var user = await _userRepository.GetAsync(currentUser.Id!.Value);
        user.SetProperty("Avatar", input.Avatar);
        user.SetProperty("Introduction", input.Introduction);
        await _userRepository.UpdateAsync(user);
        return input;
    }
}
