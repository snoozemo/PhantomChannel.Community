using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Volo.Abp.BlobStoring;
using Volo.Abp.Application.Services;
using System;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.IO;
using PhantomChannel.Community.Permissions;
using Volo.Abp.MultiTenancy;
using Microsoft.Extensions.Configuration;

namespace PhantomChannel.Community.Files;

[Authorize(CommunityPermissions.Forums.Default)]
public class FileAppService(IBlobContainer blobContainer, ICurrentTenant _currentTenant, IConfiguration configuration) : ApplicationService, IFileAppService
{
    private readonly IBlobContainer _blobContainer = blobContainer;
    private readonly ICurrentTenant _currentTenant = _currentTenant;

    private readonly IConfiguration _configuration = configuration;

    [AllowAnonymous]
    public async Task<FileDto> UploadAsync(IFormFile file)
    {

        using var md5 = MD5.Create();
        using var stream = file.OpenReadStream();
        var hash = await md5.ComputeHashAsync(stream);
        var shortMd5 = Convert.ToHexStringLower(hash);
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{shortMd5}{extension}";
        if (!await _blobContainer.ExistsAsync(fileName))
        {
            // fix: stream error
            stream.Position = 0;
            await _blobContainer.SaveAsync(fileName, stream);

        }
        var tenantName = _currentTenant.Name ?? "host";
        //储存桶
        var BucketName = _configuration["Minio:BucketName"];

        return new FileDto
        {
            Domain = "https://oss.snoozemo.com",
            Path = $"/{BucketName}/{tenantName}/{fileName}",
            Url = $"https://oss.snoozemo.com/{BucketName}/{tenantName}/{fileName}"

        };
    }

}
