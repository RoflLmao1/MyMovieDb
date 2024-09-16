using MyMovieDb.Auth.Auth0;
using Volo.Abp.Modularity;

namespace MyMovieDb;

[DependsOn(
    typeof(MyMovieDbAAuthAuth0Module),
    typeof(MyMovieDbTestBaseModule)
)]
public class MyMovieDbAuthAuth0TestModule : AbpModule
{

}
