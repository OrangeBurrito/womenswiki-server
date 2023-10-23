var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WikiContext>(options => {
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});
builder.Services.AddCors(options => {
    options.AddPolicy("WikiPolicy", policy => {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
        if (allowedOrigins != null) {
            policy.WithOrigins(allowedOrigins);
        } else {
            policy.AllowAnyOrigin();
        }
        policy.AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();
app.UseCors("WikiPolicy");

app.MapEndpoints();
app.Run();