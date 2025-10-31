# Tahap 1: Build
# Menggunakan image SDK (Software Development Kit) untuk build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
# Salin file .csproj dan .sln (jika ada) dan restore dependencies
COPY *.csproj .
RUN dotnet restore
# Salin sisa source code dan build
COPY . .
RUN dotnet publish "TodoApiSS.csproj" -c Release -o /app/publish
# Tahap 2: Rilis
# Menggunakan image ASP.NET runtime yang lebih ringan
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
# .NET 8+ container berjalan sebagai user non-root 'app'
# dan mengekspos port 8080 secara default.
ENTRYPOINT ["dotnet", "TodoApiSS.dll"]
