using Volo.Abp;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Timing;

namespace MyMovieDb;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpTestBaseModule)
)]
public class MyMovieDbTestBaseModule : AbpModule
{
    private void ConfigureClock()
    {
        Configure<AbpClockOptions>(options =>
        {
            options.Kind = DateTimeKind.Utc;
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        ConfigureClock();
    }
}
