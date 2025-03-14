FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BillingManager.API/BillingManager.API.csproj", "src/BillingManager.API/"]
COPY ["src/BillingManager.Infra.Data/BillingManager.Infra.Data.csproj", "src/BillingManager.Infra.Data/"]
COPY ["src/BillingManager.Domain/BillingManager.Domain.csproj", "src/BillingManager.Domain/"]
COPY ["src/BillingManager.Application/BillingManager.Application.csproj", "src/BillingManager.Application/"]
COPY ["src/BillingManager.Services/BillingManager.Services.csproj", "src/BillingManager.Services/"]
COPY ["src/BillingManager.Infra.CrossCutting.IoC/BillingManager.Infra.CrossCutting.IoC.csproj", "src/BillingManager.Infra.CrossCutting.IoC/"]
RUN dotnet restore "src/BillingManager.API/BillingManager.API.csproj"
COPY . .
WORKDIR "/src/src/BillingManager.API"
RUN dotnet build "BillingManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BillingManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BillingManager.API.dll"]
