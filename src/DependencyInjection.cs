using Microsoft.EntityFrameworkCore;

public static class DependencyInjection {
    public static IServiceCollection AddWikiServices(this IServiceCollection services, WebApplicationBuilder builder) {
        services.AddDbContext<WikiContext>(options => {
            options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
        });
        services.AddCors(options => {
            IConfiguration config = builder.Configuration;
            options.AddPolicy("WikiPolicy", policy => policy.WithOrigins(config.GetSection("AllowedOrigins").Get<string[]>() ?? new string[] {})
            .AllowAnyMethod().AllowAnyHeader());
        });
        services.AddScoped<SeedService>();
        services.AddScoped<ArticleService>();

        return services;
    }
    
    public static IServiceCollection SetupGraphQL(this IServiceCollection services, WebApplicationBuilder builder) {
        services.AddGraphQLServer()
        .RegisterDbContext<WikiContext>()
        .RegisterService<ArticleService>()
        .AddQueryType<Query>()
        .AddMutationType<Mutation>()
        .AddSorting();
        return services;
    }
}