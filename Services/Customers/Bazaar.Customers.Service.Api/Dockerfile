# Zobacz https://aka.ms/customizecontainer, aby dowiedzieć się, jak dostosować kontener debugowania i jak program Visual Studio używa tego pliku Dockerfile do kompilowania obrazów w celu szybszego debugowania.

# Ten etap jest używany podczas uruchamiania z programu VS w trybie szybkim (domyślnie dla konfiguracji debugowania)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Ten etap służy do kompilowania projektu usługi
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Customers/Bazaar.Customers.Service.Api/Bazaar.Customers.Service.Api.csproj", "Services/Customers/Bazaar.Customers.Service.Api/"]
COPY ["Services/Customers/Bazaar.Customers.Service.Commands/Bazaar.Customers.Service.Commands.csproj", "Services/Customers/Bazaar.Customers.Service.Commands/"]
COPY ["Services/Customers/Bazaar.Customers.Service.DataAccess/Bazaar.Customers.Service.DataAccess.csproj", "Services/Customers/Bazaar.Customers.Service.DataAccess/"]
COPY ["Services/Customers/Bazaar.Customers.Service.Domain/Bazaar.Customers.Service.Domain.csproj", "Services/Customers/Bazaar.Customers.Service.Domain/"]
COPY ["Services/Customers/Bazaar.Customers.Service.Queries/Bazaar.Customers.Service.Queries.csproj", "Services/Customers/Bazaar.Customers.Service.Queries/"]
COPY ["Shared/Bazaar.Shared.Infrastructure/Bazaar.Shared.Infrastructure.csproj", "Shared/Bazaar.Shared.Infrastructure/"]
COPY ["Shared/Bazaar.Shared.Models/Bazaar.Shared.Models.csproj", "Shared/Bazaar.Shared.Models/"]
RUN dotnet restore "./Services/Customers/Bazaar.Customers.Service.Api/Bazaar.Customers.Service.Api.csproj"
COPY . .
WORKDIR "/src/Services/Customers/Bazaar.Customers.Service.Api"
RUN dotnet build "./Bazaar.Customers.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Ten etap służy do publikowania projektu usługi do skopiowania do etapu końcowego
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bazaar.Customers.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Ten etap jest używany w środowisku produkcyjnym lub w przypadku uruchamiania z programu VS w trybie regularnym (domyślnie, gdy nie jest używana konfiguracja debugowania)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bazaar.Customers.Service.Api.dll"]