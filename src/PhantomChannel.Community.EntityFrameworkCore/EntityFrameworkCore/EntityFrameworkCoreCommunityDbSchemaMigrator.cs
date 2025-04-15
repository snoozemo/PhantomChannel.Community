using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhantomChannel.Community.Data;
using Volo.Abp.DependencyInjection;

namespace PhantomChannel.Community.EntityFrameworkCore;

public class EntityFrameworkCoreCommunityDbSchemaMigrator
    : ICommunityDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreCommunityDbSchemaMigrator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolving the CommunityDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<CommunityDbContext>()
            .Database
            .MigrateAsync();
    }
}
