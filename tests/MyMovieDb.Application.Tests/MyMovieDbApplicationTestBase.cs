using Volo.Abp.Modularity;

namespace MyMovieDb;

public abstract class MyMovieDbApplicationTestBase<TStartupModule> : MyMovieDbTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
}
