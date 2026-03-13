# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files first for better caching
COPY src/Stronger.Api/Stronger.Api.csproj StrongerWebApi/Stronger.Api/
COPY src/Stronger.Application/Stronger.Application.csproj StrongerWebApi/Stronger.Application/
COPY src/Stronger.Domain/Stronger.Domain.csproj StrongerWebApi/Stronger.Domain/
COPY src/Stronger.Infrastructure/Stronger.Infrastructure.csproj StrongerWebApi/Stronger.Infrastructure/

# Restore
RUN dotnet restore StrongerWebApi/Stronger.Api/Stronger.Api.csproj

# Copy everything else
COPY . .

# Publish
RUN dotnet publish src/Stronger.Api/Stronger.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# App listens on 8080 inside container
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Stronger.Api.dll"]