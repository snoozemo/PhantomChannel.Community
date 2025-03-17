using PhantomChannel.Community.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace PhantomChannel.Community.Permissions;

public class CommunityPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(CommunityPermissions.GroupName);
        var booksPermission = myGroup.AddPermission(CommunityPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(CommunityPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(CommunityPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(CommunityPermissions.Books.Delete, L("Permission:Books.Delete"));
        //Define your own permissions here. Example:
        //myGroup.AddPermission(CommunityPermissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<CommunityResource>(name);
    }
}
