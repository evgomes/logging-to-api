FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CustomLogger.LoggingAPI/CustomLogger.LoggingAPI.csproj", "CustomLogger.LoggingAPI/"]
COPY ["CustomLogger.Data.MongoDB/CustomLogger.Data.MongoDB.csproj", "CustomLogger.Data.MongoDB/"]
COPY ["CustomLogger.Domain/CustomLogger.Domain.csproj", "CustomLogger.Domain/"]
RUN dotnet restore "CustomLogger.LoggingAPI/CustomLogger.LoggingAPI.csproj"
COPY . .
WORKDIR "/src/CustomLogger.LoggingAPI"
RUN dotnet build "CustomLogger.LoggingAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomLogger.LoggingAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomLogger.LoggingAPI.dll"]
