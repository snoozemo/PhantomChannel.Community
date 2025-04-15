namespace PhantomChannel.Community.Permissions;

public static class CommunityPermissions
{
    public const string GroupName = "Community";


    public static class Forums
    {
        public const string Default = GroupName + ".Posts";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }
    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";
}
