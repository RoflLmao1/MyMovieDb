using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.OpenApi.Models;
using MyMovieDb.Auth.Auth0;
using MyMovieDb.EntityFrameworkCore;
using System.Net;
using System.Threading.RateLimiting;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Caching.StackExchangeRedis;
using Volo.Abp.Localization;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;

namespace MyMovieDb;

[DependsOn(
    typeof(MyMovieDbApplicationModule),
    typeof(MyMovieDbAAuthAuth0Module),
    typeof(MyMovieDbEntityFrameworkCoreMySqlModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpAutofacModule),
    typeof(AbpCachingStackExchangeRedisModule),
    typeof(AbpSwashbuckleModule)
    )]
public class MyMovieDbHttpApiModule : AbpModule
{
    private void ConfigureAntiForgery(IServiceCollection services)
    {
        services.Configure<AbpAntiForgeryOptions>(options =>
        {
            options.AutoValidate = false;
        });
    }

    private void ConfigureApiVersioning(IServiceCollection services)
    {
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
            options.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("X-Api-Version")
            );
        }).AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });
    }

    private void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.Authority = $"https://{configuration["Auth0:Domain"]}/";
            options.TokenValidationParameters =
                new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidAudience = configuration["Auth0:Audience"],
                    ValidIssuer = $"{configuration["Auth0:Domain"]}",
                    ValidateLifetime = true,
                    // Workaround to resolve issue with JWT validation failing returning HTTP 401
                    // Refer to https://github.com/dotnet/aspnetcore/issues/52075#issuecomment-2037161895
                    SignatureValidator = (token, _) => new JsonWebToken(token)
                };
        });
    }

    private void ConfigureExceptionHttpStatusCodeMappings(IServiceCollection services)
    {
        services.Configure<AbpExceptionHttpStatusCodeOptions>(options =>
        {
            options.Map(MyMovieDbDomainErrorCodes.Movie.InvalidMovie.Code, HttpStatusCode.NotFound);
        });
    }

    private void ConfigureLocalizationServices()
    {
        Configure<AbpLocalizationOptions>(options =>
        {
            options.Languages.Add(new LanguageInfo("en", "en", "English"));
        });
    }

    private void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "MyMovieDb",
                    Version = "v1.0.0"
                });

            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "Using the Authorization header with the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            };

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securitySchema);

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                { securitySchema, new[] { JwtBearerDefaults.AuthenticationScheme } }
            });
        });
    }

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        base.ConfigureServices(context);

        var services = context.Services;
        var configuration = services.GetConfiguration();

        ConfigureAntiForgery(services);
        ConfigureApiVersioning(services);
        ConfigureAuthentication(services, configuration);
        ConfigureExceptionHttpStatusCodeMappings(services);
        ConfigureLocalizationServices();
        ConfigureSwagger(services);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddRateLimiter(options =>
        {
            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
                    factory: partition => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        // Setting low limits for easier demo of rate limiting
                        PermitLimit = 10,
                        QueueLimit = 0,
                        Window = TimeSpan.FromMinutes(1)
                    })
            );
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        base.OnApplicationInitialization(context);

        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseAbpExceptionHandling();
        app.UseAbpRequestLocalization(options =>
        {
            var supportedCultures = new string[]
            {
                "en"
            };

            options.SetDefaultCulture("en");
            options.AddSupportedCultures(supportedCultures);
            options.AddSupportedUICultures(supportedCultures);
        });

        app.UseHttpsRedirection();
        app.UseRateLimiter();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseConfiguredEndpoints();
    }
}
