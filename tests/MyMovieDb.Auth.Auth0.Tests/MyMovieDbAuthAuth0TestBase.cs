using Volo.Abp.Modularity;

namespace MyMovieDb;

public abstract class MyMovieDbAuthAuth0TestBase<TStartupModule> : MyMovieDbTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
}
