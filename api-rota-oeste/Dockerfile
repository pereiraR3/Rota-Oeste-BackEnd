FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["api-rota-oeste.csproj", "./"]
RUN dotnet restore "./api-rota-oeste.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "api-rota-oeste.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "api-rota-oeste.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "api-rota-oeste.dll"]
