using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace PhantomChannel.Community.Data;

/* This is used if database provider does't define
 * ICommunityDbSchemaMigrator implementation.
 */
public class NullCommunityDbSchemaMigrator : ICommunityDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
