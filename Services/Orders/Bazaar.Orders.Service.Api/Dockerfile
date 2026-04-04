FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Orders/Bazaar.Orders.Service.Api/Bazaar.Orders.Service.Api.csproj", "Services/Orders/Bazaar.Orders.Service.Api/"]
COPY ["Services/Orders/Bazaar.Orders.Service.Commands/Bazaar.Orders.Service.Commands.csproj", "Services/Orders/Bazaar.Orders.Service.Commands/"]
COPY ["Services/Orders/Bazaar.Orders.Service.DataAccess/Bazaar.Orders.Service.DataAccess.csproj", "Services/Orders/Bazaar.Orders.Service.DataAccess/"]
COPY ["Services/Orders/Bazaar.Orders.Service.Domain/Bazaar.Orders.Service.Domain.csproj", "Services/Orders/Bazaar.Orders.Service.Domain/"]
COPY ["Services/Orders/Bazaar.Orders.Service.Queries/Bazaar.Orders.Service.Queries.csproj", "Services/Orders/Bazaar.Orders.Service.Queries/"]
COPY ["Shared/Bazaar.Shared.Infrastructure/Bazaar.Shared.Infrastructure.csproj", "Shared/Bazaar.Shared.Infrastructure/"]
COPY ["Shared/Bazaar.Shared.Models/Bazaar.Shared.Models.csproj", "Shared/Bazaar.Shared.Models/"]
RUN dotnet restore "./Services/Orders/Bazaar.Orders.Service.Api/Bazaar.Orders.Service.Api.csproj"
COPY . .
WORKDIR "/src/Services/Orders/Bazaar.Orders.Service.Api"
RUN dotnet build "./Bazaar.Orders.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bazaar.Orders.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bazaar.Orders.Service.Api.dll"]