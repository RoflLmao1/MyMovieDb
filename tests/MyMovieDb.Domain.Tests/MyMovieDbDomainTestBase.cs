using Volo.Abp.Modularity;

namespace MyMovieDb;

public abstract class MyMovieDbDomainTestBase<TStartupModule> : MyMovieDbTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
}
