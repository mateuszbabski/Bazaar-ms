FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Products/Bazaar.Products.Service.Api/Bazaar.Products.Service.Api.csproj", "Services/Products/Bazaar.Products.Service.Api/"]
COPY ["Services/Products/Bazaar.Products.Service.Commands/Bazaar.Products.Service.Commands.csproj", "Services/Products/Bazaar.Products.Service.Commands/"]
COPY ["Services/Products/Bazaar.Products.Service.DataAccess/Bazaar.Products.Service.DataAccess.csproj", "Services/Products/Bazaar.Products.Service.DataAccess/"]
COPY ["Services/Products/Bazaar.Products.Service.Domain/Bazaar.Products.Service.Domain.csproj", "Services/Products/Bazaar.Products.Service.Domain/"]
COPY ["Services/Products/Bazaar.Products.Service.Queries/Bazaar.Products.Service.Queries.csproj", "Services/Products/Bazaar.Products.Service.Queries/"]
COPY ["Shared/Bazaar.Shared.Infrastructure/Bazaar.Shared.Infrastructure.csproj", "Shared/Bazaar.Shared.Infrastructure/"]
COPY ["Shared/Bazaar.Shared.Models/Bazaar.Shared.Models.csproj", "Shared/Bazaar.Shared.Models/"]
RUN dotnet restore "./Services/Products/Bazaar.Products.Service.Api/Bazaar.Products.Service.Api.csproj"
COPY . .
WORKDIR "/src/Services/Products/Bazaar.Products.Service.Api"
RUN dotnet build "./Bazaar.Products.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bazaar.Products.Service.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bazaar.Products.Service.Api.dll"]