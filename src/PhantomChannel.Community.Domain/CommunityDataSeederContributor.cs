using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace PhantomChannel.Community;

public class CommunityDataSeederContributor(
    IRepository<IdentityRole, Guid> roleRepository,
     IGuidGenerator guidGenerator
    )
        : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<IdentityRole, Guid> _roleRepository = roleRepository;

    private readonly IGuidGenerator _guidGenerator = guidGenerator;

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _roleRepository.GetCountAsync() <= 1)
        {
            await _roleRepository.InsertAsync(
               new IdentityRole(_guidGenerator.Create(), "forumsUsers")
               {
                   IsDefault = true,
                   IsStatic = true,
                   IsPublic = true
               },
               autoSave: true
        );
            //TODO
            // 给这个角色添加默认权限

        }


    }
}