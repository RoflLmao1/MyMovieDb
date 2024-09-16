using MyMovieDb.Adapter.Tmdb;
using Volo.Abp.Modularity;

namespace MyMovieDb;

[DependsOn(
    typeof(MyMovieDbAdapterTmdbModule),
    typeof(MyMovieDbTestBaseModule)
)]
public class MyMovieDbAdapterTmdbTestModule : AbpModule
{

}
