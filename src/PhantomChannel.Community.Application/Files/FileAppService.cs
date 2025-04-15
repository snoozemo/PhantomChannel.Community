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
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace PhantomChannel.Community.Files;

[Route("api/forums/file")]
[Authorize(CommunityPermissions.Forums.Default)]
public class FilesAppService(IBlobContainer blobContainer, ICurrentTenant _currentTenant, IConfiguration configuration) : ApplicationService
{
    private readonly IBlobContainer _blobContainer = blobContainer;
    private readonly ICurrentTenant _currentTenant = _currentTenant;

    private readonly IConfiguration _configuration = configuration;

    [AllowAnonymous]
    [HttpPost("upload-some")]
    public async Task<FileDto[]> UploadSomeAsync(IFormFile[] files)
    {

        var result = new List<FileDto>();
        var tenantName = _currentTenant.Name ?? "host";
        var bucketName = _configuration["Minio:BucketName"];

        foreach (var file in files)
        {
            //如果文件为空,直接返回
            if (file.Length == 0) continue;

            // 为每个文件单独处理
            using var md5 = MD5.Create();
            using var stream = file.OpenReadStream();

            var hash = await md5.ComputeHashAsync(stream);
            var shortMd5 = Convert.ToHexStringLower(hash);
            var extension = Path.GetExtension(file.FileName);
            var fileName = $"{shortMd5}{extension}";

            if (!await _blobContainer.ExistsAsync(fileName))
            {
                // 必须重置流位置
                stream.Position = 0;
                await _blobContainer.SaveAsync(fileName, stream, true);

            }

            result.Add(new FileDto
            {
                Url = $"https://oss.snoozemo.com/{bucketName}/{tenantName}/{fileName}"
            });
        }

        return [.. result];
    }

    [AllowAnonymous]
    [HttpPost("upload-one")]
    public async Task<FileDto> UploadOneAsync(IFormFile file)
    {

        var tenantName = _currentTenant.Name ?? "host";
        var bucketName = _configuration["Minio:BucketName"];

        // 为每个文件单独处理
        using var md5 = MD5.Create();
        using var stream = file.OpenReadStream();

        var hash = await md5.ComputeHashAsync(stream);
        var shortMd5 = Convert.ToHexStringLower(hash);
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{shortMd5}{extension}";

        if (!await _blobContainer.ExistsAsync(fileName))
        {
            // 必须重置流位置
            stream.Position = 0;
            await _blobContainer.SaveAsync(fileName, stream, true);

        }
        return new FileDto
        {

            Url = $"https://oss.snoozemo.com/{bucketName}/{tenantName}/{fileName}"
        };
    }

}
