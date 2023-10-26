# WomensWiki Server
ASP.NET backend for WomensWiki, uses Postgres.

## Usage
To test, use an API Client or `curl` the endpoints like so:
```sh
curl 'localhost:[port]/article/create' -X POST -d 'title=Untitled Article'
curl 'localhost:[port]/article/update/[id]?title=Article Title' -X PATCH
curl 'localhost:[port]/article/delete/[id]' -X DELETE
```

To run a migration:
```sh
dotnet ef migrations add AddEntityProperty
dotnet ef database update
```