using System;
using System.Threading.Tasks;
using PhantomChannel.Community.Books;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Identity;

namespace PhantomChannel.Community;

public class CommunityDataSeederContributor(
    IRepository<Book, Guid> bookRepository,
    IRepository<IdentityRole, Guid> roleRepository,
     IGuidGenerator guidGenerator
    )
        : IDataSeedContributor, ITransientDependency
{
    private readonly IRepository<Book, Guid> _bookRepository = bookRepository;
    private readonly IRepository<IdentityRole, Guid> _roleRepository = roleRepository;

    private readonly IGuidGenerator _guidGenerator = guidGenerator;

    public async Task SeedAsync(DataSeedContext context)
    {
        if (await _bookRepository.GetCountAsync() <= 0)
        {
            await _bookRepository.InsertAsync(
                new Book
                {
                    Name = "1984",
                    Type = BookType.Dystopia,
                    PublishDate = new DateTime(1949, 6, 8),
                    Price = 19.84f
                },
                autoSave: true
            );

            await _bookRepository.InsertAsync(
                new Book
                {
                    Name = "The Hitchhiker's Guide to the Galaxy",
                    Type = BookType.ScienceFiction,
                    PublishDate = new DateTime(1995, 9, 27),
                    Price = 42.0f
                },
                autoSave: true
            );
        }
        // 添加默认角色
        await _roleRepository.InsertAsync(
               new IdentityRole(_guidGenerator.Create(), "forumsUsers")
               {
                   IsDefault = true,
                   IsStatic = true,
                   IsPublic = true
               },
               autoSave: true
        );
        // 给这个角色添加默认权限
        //TODO 添加默认用户的默认权限

    }
}