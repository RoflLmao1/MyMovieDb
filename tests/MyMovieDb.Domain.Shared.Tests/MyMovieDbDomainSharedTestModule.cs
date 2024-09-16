using Volo.Abp.Modularity;

namespace MyMovieDb;

[DependsOn(
    typeof(MyMovieDbDomainSharedModule),
    typeof(MyMovieDbTestBaseModule)
)]
public class MyMovieDbDomainSharedTestModule : AbpModule
{

}
