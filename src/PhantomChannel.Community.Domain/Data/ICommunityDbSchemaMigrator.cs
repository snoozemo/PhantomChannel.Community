using System.Threading.Tasks;

namespace PhantomChannel.Community.Data;

public interface IServerDbSchemaMigrator
{
    Task MigrateAsync();
}
