using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Volo.Abp.Modularity;

namespace MyMovieDb.Adapter.Tmdb;

[DependsOn(
    typeof(MyMovieDbDomainModule)
    )]
public class MyMovieDbAdapterTmdbModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        var services = context.Services;
        var configuration = services.GetConfiguration();

        services.Configure<TmdbOptions>(
            configuration.GetSection(TmdbOptions.ConfigurationPath));

        services.AddHttpClient<ITmdbService, TmdbService>(client =>
        {
            var tmdbOption = configuration
                .GetSection(TmdbOptions.ConfigurationPath)
                .Get<TmdbOptions>() ?? new TmdbOptions();

            client.BaseAddress = new Uri(tmdbOption.BaseUri);
            client.Timeout = TimeSpan.FromSeconds(tmdbOption.Timeout);
            client.DefaultRequestHeaders.Authorization
                = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", tmdbOption.AccessToken);
        });
    }
}
