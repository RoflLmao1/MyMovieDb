using MyMovieDb.Localization;
using Volo.Abp.Domain;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace MyMovieDb;

[DependsOn(
    typeof(AbpDddDomainSharedModule),
    typeof(AbpValidationModule)
    )]
public class MyMovieDbDomainSharedModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        Configure<AbpVirtualFileSystemOptions>(options =>
        {
            options.FileSets.AddEmbedded<MyMovieDbDomainSharedModule>();
        });

        Configure<AbpLocalizationOptions>(options =>
        {
            options.Resources
                .Add<MyMovieDbResource>("en")
                .AddBaseTypes(typeof(AbpValidationResource))
                .AddVirtualJson("/Localization/MyMovieDb");

            options.DefaultResourceType = typeof(MyMovieDbResource);
        });

        Configure<AbpExceptionLocalizationOptions>(options =>
        {
            options.MapCodeNamespace(MyMovieDbDomainErrorCodes.Namespace, typeof(MyMovieDbResource));
            options.MapCodeNamespace(MyMovieDbDomainErrorCodes.Movie.Namespace, typeof(MyMovieDbResource));
        });
    }
}
