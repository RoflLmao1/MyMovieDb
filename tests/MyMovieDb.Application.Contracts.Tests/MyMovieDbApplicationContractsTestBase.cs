using Volo.Abp.Modularity;

namespace MyMovieDb;

public abstract class MyMovieDbApplicationContractsTestBase<TStartupModule> : MyMovieDbTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
}
