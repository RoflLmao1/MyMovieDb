using Volo.Abp.Modularity;

namespace MyMovieDb;

[DependsOn(
    typeof(MyMovieDbApplicationContractsModule),
    typeof(MyMovieDbTestBaseModule)
)]
public class MyMovieDbApplicationContractsTestModule : AbpModule
{

}
