FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
WORKDIR /womenswiki
ENV HOST 0.0.0.0

COPY . ./
RUN dotnet restore
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine
WORKDIR /womenswiki
COPY --from=build /womenswiki/out .
ENTRYPOINT ["dotnet", "WomensWiki.dll"]