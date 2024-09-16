using Volo.Abp.Modularity;

namespace MyMovieDb;

public abstract class MyMovieDbAdapterTmdbTestBase<TStartupModule> : MyMovieDbTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
}
