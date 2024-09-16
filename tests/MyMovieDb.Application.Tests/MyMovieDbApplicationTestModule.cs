using Volo.Abp.Modularity;

namespace MyMovieDb;

[DependsOn(
    typeof(MyMovieDbApplicationModule),
    typeof(MyMovieDbTestBaseModule)
)]
public class MyMovieDbApplicationTestModule : AbpModule
{

}
