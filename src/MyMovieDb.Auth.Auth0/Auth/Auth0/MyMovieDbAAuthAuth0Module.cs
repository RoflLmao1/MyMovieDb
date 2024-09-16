using Microsoft.Extensions.DependencyInjection;
using MyMovieDb.Adapter.Tmdb;
using MyMovieDb.Users;
using Volo.Abp.Modularity;

namespace MyMovieDb.Auth.Auth0;

[DependsOn(
    typeof(MyMovieDbDomainModule)
    )]
public class MyMovieDbAAuthAuth0Module : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        var services = context.Services;
        var configuration = services.GetConfiguration();

        services.Configure<Auth0Options>(
            configuration.GetSection(Auth0Options.ConfigurationPath));

        services.AddHttpClient<IUserAuthorizationService, Auth0UserAuthorizationService>();
    }
}
