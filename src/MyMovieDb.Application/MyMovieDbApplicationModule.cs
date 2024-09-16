using Microsoft.Extensions.DependencyInjection;
using MyMovieDb.Adapter.Tmdb;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace MyMovieDb;

[DependsOn(
    typeof(MyMovieDbDomainModule),
    typeof(MyMovieDbApplicationContractsModule),
    typeof(MyMovieDbAdapterTmdbModule),
    typeof(AbpAutoMapperModule),
    typeof(AbpDddApplicationModule)
    )]
public class MyMovieDbApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        var services = context.Services;

        services.AddMediatR(
            config => config.RegisterServicesFromAssemblyContaining<MyMovieDbApplicationModule>());

        services.Configure<AbpAutoMapperOptions>(options =>
        {
            options.AddMaps<MyMovieDbApplicationModule>();
        });
    }
}
