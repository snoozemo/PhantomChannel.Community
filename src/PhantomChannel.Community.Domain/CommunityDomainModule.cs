using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PhantomChannel.Community.Localization;
using PhantomChannel.Community.MultiTenancy;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement.Identity;
using Volo.Abp.SettingManagement;
using Volo.Abp.BlobStoring.Database;
using Volo.Abp.Caching;
using Volo.Abp.OpenIddict;
using Volo.Abp.PermissionManagement.OpenIddict;
using Volo.Abp.AuditLogging;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Emailing;
using Volo.Abp.FeatureManagement;
using Volo.Abp.Identity;
using Volo.Abp.TenantManagement;
using Volo.Abp.BlobStoring.Minio;
using Volo.Abp.BlobStoring;
using System;

namespace PhantomChannel.Community;

[DependsOn(
    typeof(CommunityDomainSharedModule),
    typeof(AbpAuditLoggingDomainModule),
    typeof(AbpCachingModule),
    typeof(AbpBackgroundJobsDomainModule),
    typeof(AbpFeatureManagementDomainModule),
    typeof(AbpPermissionManagementDomainIdentityModule),
    typeof(AbpPermissionManagementDomainOpenIddictModule),
    typeof(AbpSettingManagementDomainModule),
    typeof(AbpEmailingModule),
    typeof(AbpIdentityDomainModule),
    typeof(AbpOpenIddictDomainModule),
    typeof(AbpTenantManagementDomainModule),
    typeof(BlobStoringDatabaseDomainModule)
    )]
[DependsOn(typeof(AbpBlobStoringMinioModule))]
public class CommunityDomainModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<AbpMultiTenancyOptions>(options =>
        {
            options.IsEnabled = MultiTenancyConsts.IsEnabled;
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
            options.Languages.Add(new LanguageInfo("zh-Hans", "zh-Hans", "简体中文"));
        });

        Configure<AbpBlobStoringOptions>(options =>
        {
            var configuration = context.Services.GetConfiguration();

            var minioEndpoint = configuration["Minio:Endpoint"];
            var minioAccessKey = configuration["Minio:AccessKey"];
            var minioSecretKey = configuration["Minio:SecretKey"];
            var minioBucketName = configuration["Minio:BucketName"];

            if (string.IsNullOrEmpty(minioEndpoint) || string.IsNullOrEmpty(minioAccessKey) || string.IsNullOrEmpty(minioSecretKey) || string.IsNullOrEmpty(minioBucketName))
            {
                throw new Exception("Minio配置信息不完整");
            }

            options.Containers.ConfigureDefault(container =>
            {
                container.UseMinio(minio =>
                {
                    minio.EndPoint = minioEndpoint;
                    minio.AccessKey = minioAccessKey;
                    minio.SecretKey = minioSecretKey;
                    minio.BucketName = minioBucketName;
                    minio.WithSSL = false;
                });
            });
        });

#if DEBUG
        context.Services.Replace(ServiceDescriptor.Singleton<IEmailSender, NullEmailSender>());
#endif
    }
}
