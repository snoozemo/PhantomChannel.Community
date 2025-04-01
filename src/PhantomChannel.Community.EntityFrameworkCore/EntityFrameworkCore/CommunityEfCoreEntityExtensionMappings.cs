using Volo.Abp.Threading;

namespace PhantomChannel.Community.EntityFrameworkCore;

public static class CommunityEfCoreEntityExtensionMappings
{
    private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

    public static void Configure()
    {
        CommunityGlobalFeatureConfigurator.Configure();
        CommunityModuleExtensionConfigurator.Configure();

        OneTimeRunner.Run(() =>
        {


            // ObjectExtensionManager.Instance
            //     .MapEfCoreProperty<IdentityUser, string>(
            //         nameof(UserExpansion.Avatar),
            //         (entityBuilder, propertyBuilder) =>
            //         {
            //             propertyBuilder.HasMaxLength(UserExpansionsConsts.MaxAvatarLength);
            //         })
            //     .MapEfCoreProperty<IdentityUser, string>(
            //         nameof(UserExpansion.Introduction),
            //         (entityBuilder, propertyBuilder) =>
            //         {
            //             propertyBuilder.HasMaxLength(UserExpansionsConsts.MaxIntroductionLength);
            //         });
            /* You can configure extra properties for the
             * entities defined in the modules used by your application.
             *
             * This class can be used to map these extra properties to table fields in the database.
             *
             * USE THIS CLASS ONLY TO CONFIGURE EF CORE RELATED MAPPING.
             * USE CommunityModuleExtensionConfigurator CLASS (in the Domain.Shared project)
             * FOR A HIGH LEVEL API TO DEFINE EXTRA PROPERTIES TO ENTITIES OF THE USED MODULES
             *
             * Example: Map a property to a table field:

                 ObjectExtensionManager.Instance
                     .MapEfCoreProperty<IdentityUser, string>(
                         "MyProperty",
                         (entityBuilder, propertyBuilder) =>
                         {
                             propertyBuilder.HasMaxLength(128);
                         }
                     );

             * See the documentation for more:
             * https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities
             */
        });
    }
}
