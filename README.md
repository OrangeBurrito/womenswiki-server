# WomensWiki Server
ASP.NET Core backend for WomensWiki.

## Running Locally
- Set up a docker container running Postgres (`postgres:alpine` is recommended)
- Git clone this repository
- Add the property `WikiConnection` in the `ConnectionStrings` section of `appsettings.Development.json`, with the value being your Postgres container's connection string
- Update your local database with `dotnet ef database update`
- Run with `dotnet run` at project root level
- Test queries and mutations at the `localhost:5276/graphql` endpoint