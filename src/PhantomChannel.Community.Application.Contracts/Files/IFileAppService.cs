using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Http;

namespace PhantomChannel.Community.Files;
public interface IFileAppService : IApplicationService
{
    Task<FileDto> UploadAsync(IFormFile file);
}
