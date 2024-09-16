using Volo.Abp.Modularity;

namespace MyMovieDb;

[DependsOn(
    typeof(MyMovieDbDomainModule),
    typeof(MyMovieDbTestBaseModule)
)]
public class MyMovieDbDomainTestModule : AbpModule
{

}
