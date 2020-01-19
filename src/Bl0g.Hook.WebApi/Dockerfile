FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Bl0g.Hook.WebApi/Bl0g.Hook.WebApi.csproj", "Bl0g.Hook.WebApi/"]
RUN dotnet restore "Bl0g.Hook.WebApi/Bl0g.Hook.WebApi.csproj"
COPY . .
WORKDIR "/src/Bl0g.Hook.WebApi"
RUN dotnet build "Bl0g.Hook.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bl0g.Hook.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bl0g.Hook.WebApi.dll"]