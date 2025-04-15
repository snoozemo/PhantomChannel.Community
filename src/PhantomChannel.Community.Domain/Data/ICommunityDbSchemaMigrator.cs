using System.Threading.Tasks;

namespace PhantomChannel.Community.Data;

public interface ICommunityDbSchemaMigrator
{
    Task MigrateAsync();
}
