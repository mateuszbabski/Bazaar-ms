FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Baskets/Bazaar.Baskets.Service.Api/Bazaar.Baskets.Service.Api.csproj", "Services/Baskets/Bazaar.Baskets.Service.Api/"]
COPY ["Services/Baskets/Bazaar.Baskets.Service.Commands/Bazaar.Baskets.Service.Commands.csproj", "Services/Baskets/Bazaar.Baskets.Service.Commands/"]
COPY ["Services/Baskets/Bazaar.Baskets.Service.Queries/Bazaar.Baskets.Service.Queries.csproj", "Services/Baskets/Bazaar.Baskets.Service.Queries/"]
COPY ["Services/Baskets/Bazaar.Baskets.Service.DataAccess/Bazaar.Baskets.Service.DataAccess.csproj", "Services/Baskets/Bazaar.Baskets.Service.DataAccess/"]
COPY ["Services/Baskets/Bazaar.Baskets.Service.Domain/Bazaar.Baskets.Service.Domain.csproj", "Services/Baskets/Bazaar.Baskets.Service.Domain/"]
COPY ["Shared/Bazaar.Shared.Infrastructure/Bazaar.Shared.Infrastructure.csproj", "Shared/Bazaar.Shared.Infrastructure/"]
COPY ["Shared/Bazaar.Shared.Models/Bazaar.Shared.Models.csproj", "Shared/Bazaar.Shared.Models/"]
RUN dotnet restore "./Services/Baskets/Bazaar.Baskets.Service.Api/Bazaar.Baskets.Service.Api.csproj"
COPY . .
WORKDIR "/src/Services/Baskets/Bazaar.Baskets.Service.Api"
RUN dotnet build "./Bazaar.Baskets.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bazaar.Baskets.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bazaar.Baskets.Service.Api.dll"]