using Microsoft.EntityFrameworkCore;

var connectionString = "Server=localhost;Port=5432;Database=wiki;User Id=postgres;Password=pachydermcashflow;";

var options = new DbContextOptionsBuilder<WikiContext>()
    .UseNpgsql(connectionString).Options;

var wikiContext = new WikiContext(options);

Console.WriteLine(wikiContext);