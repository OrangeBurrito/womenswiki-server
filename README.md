# WomensWiki Server
ASP.NET Core backend for WomensWiki.

## Usage
To run locally, clone this repo and setup a Docker instance running SQL Server. Add the connection string (`DevConnection`) to `appsettings.Development.json`, then run migrations.

Test queries/mutations at the `localhost:5276/graphql` endpoint.