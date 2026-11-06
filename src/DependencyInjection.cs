using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.IdentityModel.Tokens;
using WomensWiki.Infrastructure.Persistence;
using WomensWiki.Infrastructure.Services;
using WomensWiki.Infrastructure.Interfaces;
using System.Threading.RateLimiting;
using WomensWiki.Common;
using Microsoft.AspNetCore.RateLimiting;

namespace WomensWiki;

public static class DependencyInjection {
    public static void AddInfrastructure(this IServiceCollection services, string connectionString, System.Reflection.Assembly assembly) {
        services.AddDbContext<AppDbContext>(o => 
        o.UseNpgsql(connectionString, o => o.SetPostgresVersion(9,6))
            .ReplaceService<IHistoryRepository, HistoryContext>()
            .UseSnakeCaseNamingConvention()
            .UseSeeding((context, _) => {
                AppDbContextSeed.SeedInitialDataAsync((AppDbContext)context).GetAwaiter().GetResult();
            })
            .UseAsyncSeeding(async (context, _, cancellationToken) => {
                await AppDbContextSeed.SeedInitialDataAsync((AppDbContext)context);
            })
        );

        var repositoryTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Select(t => new {
                Implementation = t,
                Interface = t.GetInterfaces().FirstOrDefault(i => typeof(IRepository).IsAssignableFrom(i) && i != typeof(IRepository))
            })
            .Where(t => t.Interface != null);

        var cachedRepositoryTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract).Where(t => t.Name.StartsWith("Cached"))
            .Where(t => t.GetInterfaces().Any(i => typeof(IRepository).IsAssignableFrom(i) && i != typeof(IRepository)))
            .ToList();

        foreach (var type in repositoryTypes) {
            var cached = cachedRepositoryTypes.FirstOrDefault(t => t.GetInterfaces().Contains(type.Interface));
            if (cached != null) {
                services.AddScoped(type.Interface, cached);
                services.AddScoped(type.Implementation);
            } else {
                services.AddScoped(type.Interface, type.Implementation);
            }
        }

        services.AddMemoryCache();
        services.AddHttpClient<IdentityProviderService>((serviceProvider, client) => {
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            client.BaseAddress = new Uri(config.GetValue<string>("Auth:BaseUrl")!);
        });
    }

    public static void AddApplication(this IServiceCollection services, System.Reflection.Assembly assembly) {
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));
    }

    public static void AddApi(this IServiceCollection services, ConfigurationManager configuration, bool isDevelopment) {
		// services.Configure<ForwardedHeadersOptions>(o => {
		// 	o.ForwardedHeaders = ForwardedHeaders.XForwardedFor;
		// 	foreach(var ip in configuration["AllowedOrigins"].Split(";")) {
		// 		o.KnownProxies.Add(IPAddress.Parse(ip));
		// 	}
		// });

        services.AddCors(o => {
            o.AddPolicy("Localhost", p => p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader() );
            o.AddPolicy("Prod", p => p.WithOrigins(configuration["AllowedOrigins"].Split(";")).AllowAnyMethod().AllowAnyHeader() );
        });

		services.AddRateLimiter(o => {
			o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
			o.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext => {
				var userId = httpContext.User.FindUserId();
				if (userId != null) {
					return RateLimitPartition.GetTokenBucketLimiter(
						$"user:{userId}",
						_ => new TokenBucketRateLimiterOptions {
							// TokenLimit = 4,
							// TokensPerPeriod = 1,
							TokenLimit = 300,
							TokensPerPeriod = 60,
							ReplenishmentPeriod = TimeSpan.FromSeconds(10),
							QueueLimit = 0
						}
					);
				} else {
					return RateLimitPartition.GetTokenBucketLimiter(
						$"ip:{httpContext.Connection.RemoteIpAddress}",
						_ => new TokenBucketRateLimiterOptions {
							TokenLimit = 200,
							TokensPerPeriod = 40,
							ReplenishmentPeriod = TimeSpan.FromSeconds(10),
							QueueLimit = 0
						}
					);
				};
			});
			// o.OnRejected = (context, cancellationToken) => {
			// 	var user = context.HttpContext.User.FindUserId();
			// 	var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>()
			// 		.CreateLogger("RateLimiter");

			// 	logger.LogWarning("Rate limit exceeded for" + ((user != null ? $" user {user}" : "anonymous " ), $"at IP {context.HttpContext.Connection.RemoteIpAddress}"));

			// 		// .LogWarning("Rate limited exceeded for" + (user != null ? $" user {user}" : $" IP: {context.HttpContext.Connection.RemoteIpAddress}"));
			// 		return ValueTask.CompletedTask;
			// };
		});
		
	services.AddSingleton<RateLimitExceededHostedService>();
	services.AddHostedService(sp => sp.GetRequiredService<RateLimitExceededHostedService>());

	services.AddOptions<RateLimiterOptions>().Configure<RateLimitExceededHostedService>((o, rateLimitExceededService) => {
		o.OnRejected = (context, cancellationToken) => {
			var user = context.HttpContext.User.FindUserId();
			var partition = (user != null ? $"user:{user}" : $"anonymous") + $" at IP:{context.HttpContext.Connection.RemoteIpAddress}";
			rateLimitExceededService.Record(partition);
			return ValueTask.CompletedTask;
		};
	});

        var authBaseUrl = configuration["Auth:BaseUrl"];

        if (!string.IsNullOrEmpty(authBaseUrl)) {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o => {
                    o.Audience = configuration["Auth:Audience"];
                    o.MetadataAddress = $"{authBaseUrl}/.well-known/openid-configuration";
                    o.TokenValidationParameters = new TokenValidationParameters {
                        ValidIssuer = authBaseUrl,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = !isDevelopment,
                        ValidateIssuer = true
                    };
                    o.RequireHttpsMetadata = !isDevelopment;
                    o.MapInboundClaims = false;
                    o.Events = new JwtBearerEvents {
                        OnTokenValidated = async context => {
                            var idpService = context.HttpContext.RequestServices.GetRequiredService<IdentityProviderService>();
                            await idpService.TransformClaimsAsync(context);
                        }
                    };
                });
        }
        services
            .AddGraphQLServer()
            .AddTypes()
            .AddSorting()
			.AddAuthorization();
    }
}