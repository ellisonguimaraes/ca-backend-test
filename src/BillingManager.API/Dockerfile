FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/BillingManager.API/BillingManager.API.csproj", "BillingManager.API/"]
COPY ["src/BillingManager.Infra.Data/BillingManager.Infra.Data.csproj", "BillingManager.Infra.Data/"]
COPY ["src/BillingManager.Domain/BillingManager.Domain.csproj", "BillingManager.Domain/"]
COPY ["src/BillingManager.Application/BillingManager.Application.csproj", "BillingManager.Application/"]
COPY ["src/BillingManager.Services/BillingManager.Services.csproj", "BillingManager.Services/"]
COPY ["src/BillingManager.Infra.CrossCutting.IoC/BillingManager.Infra.CrossCutting.IoC.csproj", "BillingManager.Infra.CrossCutting.IoC/"]
RUN dotnet restore "BillingManager.API/BillingManager.API.csproj"
COPY src .
WORKDIR "/src/BillingManager.API"
RUN dotnet build "BillingManager.API.csproj" -c Release -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BillingManager.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BillingManager.API.dll"]
