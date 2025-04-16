using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using WomensWiki.Common;
using WomensWiki.Common.Interfaces;
using WomensWiki.src.Common;

namespace WomensWiki;

public static class DependencyInjection {
    public static void AddInfrastructure(this IServiceCollection services, string connectionString, System.Reflection.Assembly assembly) {
        services.AddDbContext<AppDbContext>(o => 
        o.UseNpgsql(connectionString, o => o.SetPostgresVersion(9,6))
            .ReplaceService<IHistoryRepository, HistoryContext>()
            .UseSnakeCaseNamingConvention()
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
    }

    public static void AddApplication(this IServiceCollection services, System.Reflection.Assembly assembly) {
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));
    }

    public static void AddApi(this IServiceCollection services) {
        services.AddCors(o => o.AddPolicy("Localhost", p => { p.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }));
        services.AddGraphQLServer().AddTypes().AddSorting();
    }
}