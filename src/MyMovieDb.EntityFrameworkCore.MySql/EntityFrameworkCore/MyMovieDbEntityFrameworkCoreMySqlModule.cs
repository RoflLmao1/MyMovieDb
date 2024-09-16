using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyMovieDb.Users;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.DependencyInjection;
using Volo.Abp.EntityFrameworkCore.MySQL;
using Volo.Abp.Modularity;

namespace MyMovieDb.EntityFrameworkCore;

[DependsOn(
    typeof(MyMovieDbDomainModule),
    typeof(AbpEntityFrameworkCoreMySQLModule)
    )]
public class MyMovieDbEntityFrameworkCoreMySqlModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        Configure<AbpEntityOptions>(options =>
        {
            options.Entity<User>(entityOptions =>
            {
                entityOptions.DefaultWithDetailsFunc
                    = query => query.Include(x => x.UserFavourites);
            });

            options.Entity<UserFavourite>(entityOptions =>
            {
                entityOptions.DefaultWithDetailsFunc
                    = query => query.Include(x => x.User);
            });
        });

        context.Services.AddAbpDbContext<MyMovieDbDbContext>(options =>
        {
            /* Remove "includeAllEntities: true" to create
             * default repositories only for aggregate roots */
            options.AddDefaultRepositories(includeAllEntities: true);
        });

        Configure<AbpDbContextOptions>(options =>
        {
            /* The main point to change your DBMS. */
            options.UseMySQL();
        });
    }
}
