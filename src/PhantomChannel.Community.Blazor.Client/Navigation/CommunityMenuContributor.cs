using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PhantomChannel.Community.Localization;
using PhantomChannel.Community.Permissions;
using PhantomChannel.Community.MultiTenancy;
using Volo.Abp.Account.Localization;
using Volo.Abp.UI.Navigation;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.SettingManagement.Blazor.Menus;
using Volo.Abp.Users;
using Volo.Abp.TenantManagement.Blazor.Navigation;
using Volo.Abp.Identity.Blazor;

namespace PhantomChannel.Community.Blazor.Client.Navigation;

public class CommunityMenuContributor : IMenuContributor
{
    private readonly IConfiguration _configuration;

    public CommunityMenuContributor(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task ConfigureMenuAsync(MenuConfigurationContext context)
    {
        if (context.Menu.Name == StandardMenus.Main)
        {
            await ConfigureMainMenuAsync(context);
        }
        else if (context.Menu.Name == StandardMenus.User)
        {
            await ConfigureUserMenuAsync(context);
        }
    }

    private static async Task ConfigureMainMenuAsync(MenuConfigurationContext context)
    {
        var l = context.GetLocalizer<CommunityResource>();
        
        //Administration
        var administration = context.Menu.GetAdministration();
        administration.Order = 6;

        context.Menu.AddItem(new ApplicationMenuItem(
            CommunityMenus.Home,
            l["Menu:Home"],
            "/",
            icon: "fas fa-home",
            order: 1
        ));
        if (MultiTenancyConsts.IsEnabled)
        {
            administration.SetSubItemOrder(TenantManagementMenuNames.GroupName, 1);
        }
        else
        {
            administration.TryRemoveMenuItem(TenantManagementMenuNames.GroupName);
        }

        administration.SetSubItemOrder(IdentityMenuNames.GroupName, 2);
        administration.SetSubItemOrder(SettingManagementMenus.GroupName, 3);
    }

    private async Task ConfigureUserMenuAsync(MenuConfigurationContext context)
    {
        var accountStringLocalizer = context.GetLocalizer<AccountResource>();
        var authServerUrl = _configuration["AuthServer:Authority"] ?? "";

        context.Menu.AddItem(new ApplicationMenuItem(
            "Account.Manage",
            accountStringLocalizer["MyAccount"],
            $"{authServerUrl.EnsureEndsWith('/')}Account/Manage",
            icon: "fa fa-cog",
            order: 1000,
            target: "_blank").RequireAuthenticated());

        await Task.CompletedTask;
    }
}
