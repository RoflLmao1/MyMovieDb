using Volo.Abp.Modularity;

namespace MyMovieDb;

public abstract class MyMovieDbDomainSharedTestBase<TStartupModule> : MyMovieDbTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{
}
