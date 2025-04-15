using PhantomChannel.Community.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace PhantomChannel.Community.Permissions;

public class CommunityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CommunityPermissions.GroupName);

        //论坛相关
        var forumsPermission = myGroup.AddPermission(CommunityPermissions.Forums.Default, L("Permission:Forums"));
        forumsPermission.AddChild(CommunityPermissions.Forums.Create, L("Permission:Forums.Create"));
        forumsPermission.AddChild(CommunityPermissions.Forums.Edit, L("Permission:Forums.Edit"));
        forumsPermission.AddChild(CommunityPermissions.Forums.Delete, L("Permission:Forums.Delete"));

        //Define your own permissions here. Example:
        //myGroup.AddPermission(CommunityPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CommunityResource>(name);
    }
}
