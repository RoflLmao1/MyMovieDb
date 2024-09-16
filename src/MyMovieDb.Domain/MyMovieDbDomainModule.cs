using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace MyMovieDb;

[DependsOn(
    typeof(MyMovieDbDomainSharedModule),
    typeof(AbpDddDomainModule)
    )]
public class MyMovieDbDomainModule : AbpModule
{
}
