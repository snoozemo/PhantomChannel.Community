using Volo.Abp.Settings;

namespace PhantomChannel.Community.Settings;

public class CommunitySettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(CommunitySettings.MySetting1));
    }
}
