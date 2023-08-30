var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddWikiServices(builder)
    .SetupGraphQL(builder);

var app = builder.Build();

app.UseCors("WikiPolicy");

app.MapGraphQL();

app.AddApiEndpoints();

app.Run();