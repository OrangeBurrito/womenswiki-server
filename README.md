# WomensWiki Server
ASP.NET backend for WomensWiki, uses Postgres.

## Usage
To test, use an API Client or `curl` the endpoints like so:
```sh
curl 'localhost:[port]/article/create' -X POST -d 'title=Article Title&content=some content here'
curl 'localhost:[port]/article/update' -X PATCH -d 'id=GUID&content=changed content'
curl 'localhost:[port]/article/delete/[id]' -X DELETE
```

To run a migration:
```sh
dotnet ef migrations add AddEntityProperty
dotnet ef database update
```