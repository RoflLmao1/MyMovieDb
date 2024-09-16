using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace MyMovieDb;

[DependsOn(
    typeof(MyMovieDbDomainSharedModule),
    typeof(AbpDddApplicationContractsModule)
    )]
public class MyMovieDbApplicationContractsModule : AbpModule
{
}
