FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MoneyTrack.Api/MoneyTrack.Api.csproj", "MoneyTrack.Api/"]
COPY ["MoneyTrack.Application/MoneyTrack.Application.csproj", "MoneyTrack.Application/"]
COPY ["MoneyTrack.Domain/MoneyTrack.Domain.csproj", "MoneyTrack.Domain/"]
COPY ["MoneyTrack.Infrastructure/MoneyTrack.Infrastructure.csproj", "MoneyTrack.Infrastructure/"]
COPY ["MoneyTrack.Persistence/MoneyTrack.Persistence.csproj", "MoneyTrack.Persistence/"]
RUN dotnet restore "MoneyTrack.Api/MoneyTrack.Api.csproj"
COPY . .
WORKDIR "/src/MoneyTrack.Api"
RUN dotnet build "MoneyTrack.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MoneyTrack.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENTRYPOINT ["dotnet", "MoneyTrack.Api.dll"]