using Microsoft.EntityFrameworkCore;
public static class DependencyInjection {
    public static IServiceCollection AddWikiServices(this IServiceCollection services, WebApplicationBuilder builder) {
        services.AddDbContext<WikiContext>(options => {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
        });
        services.AddCors(options => {
            IConfiguration config = builder.Configuration;
            options.AddDefaultPolicy(builder => builder.WithOrigins(config.GetSection("AllowedOrigins").Value ?? "")
            .AllowAnyMethod().AllowAnyHeader());
        });
        services.AddScoped<SeedService>();
        services.AddScoped<ArticleService>();

        return services;
    }
}