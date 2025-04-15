using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhantomChannel.Community.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Data;
using Volo.Abp.Identity;

namespace PhantomChannel.Community.Self;

[Route("api/forums/self")]
[Authorize(CommunityPermissions.Forums.Default)]
public class SelfAppService(IIdentityUserRepository userRepository) : ApplicationService, ISelfAppService
{
    private readonly IIdentityUserRepository _userRepository = userRepository;

    [Authorize(CommunityPermissions.Forums.Edit)]
    [HttpPut("set")]
    public async Task<SelfDto> SetSelfAsync(CreateUpdateSelfDto input)
    {

        var user = await _userRepository.GetAsync(CurrentUser.Id!.Value);
        user.SetProperty("Avatar", input.Avatar);
        user.SetProperty("Introduction", input.Introduction);
        await _userRepository.UpdateAsync(user);

        return new SelfDto
        {
            Avatar = user.GetProperty<string>("Avatar"),
            Introduction = user.GetProperty<string>("Introduction"),
            UserName = user.UserName,
            Surname = user.Surname,
            Email = user.Email
        }; ;
    }

    [Authorize(CommunityPermissions.Forums.Default)]
    [HttpGet("get")]
    public async Task<SelfDto> GetSelfAsync()
    {

        var user = await _userRepository.GetAsync(CurrentUser.Id!.Value);

        user.GetProperty<string>("Avatar");
        user.GetProperty<string>("Introduction");

        return new SelfDto
        {
            Avatar = user.GetProperty<string>("Avatar"),
            Introduction = user.GetProperty<string>("Introduction"),
            UserName = user.UserName,
            Surname = user.Surname,
            Email = user.Email
        };

    }
}
