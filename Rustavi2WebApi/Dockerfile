FROM mcr.microsoft.com/dotnet/core/sdk:2.2 as build-env
WORKDIR /app

# Copy project files and caches restore result
COPY *.csproj .
RUN dotnet restore

# Copy rest of the code and publish
COPY . .
RUN dotnet publish -c release -o /out

#Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /app
COPY --from=build-env /out .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet rustavi2WebApi.dll
