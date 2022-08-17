FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CustomLogger.Consumer.Web/CustomLogger.Consumer.Web.csproj", "CustomLogger.Consumer.Web/"]
COPY ["CustomLogger.Consumer.ApiClient/CustomLogger.Consumer.ApiClient.csproj", "CustomLogger.Consumer.ApiClient/"]
RUN dotnet restore "CustomLogger.Consumer.Web/CustomLogger.Consumer.Web.csproj"
COPY . .
WORKDIR "/src/CustomLogger.Consumer.Web"
RUN dotnet build "CustomLogger.Consumer.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CustomLogger.Consumer.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CustomLogger.Consumer.Web.dll"]