#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GreenSystem.Charging.Service/GreenSystem.Charging.Service.csproj", "src/GreenSystem.Charging.Service/"]
COPY ["src/GreenSystem.Charging.Groups.WebApi/GreenSystem.Charging.Groups.WebApi.csproj", "src/GreenSystem.Charging.Groups.WebApi/"]
COPY ["src/GreenSystem.Charging.Groups.Abstractions/GreenSystem.Charging.Groups.Abstractions.csproj", "src/GreenSystem.Charging.Groups.Abstractions/"]
COPY ["src/GreenSystem.Charging.Abstractions/GreenSystem.Charging.Abstractions.csproj", "src/GreenSystem.Charging.Abstractions/"]
COPY ["src/GreenSystem.Charging.Groups/GreenSystem.Charging.Groups.csproj", "src/GreenSystem.Charging.Groups/"]
COPY ["src/GreenSystem.Charging.Groups.Store/GreenSystem.Charging.Groups.Store.csproj", "src/GreenSystem.Charging.Groups.Store/"]
COPY ["src/GreenSystem.Charging.Store/GreenSystem.Charging.Store.csproj", "src/GreenSystem.Charging.Store/"]
COPY ["src/GreenSystem.Charging.Store.Abstractions/GreenSystem.Charging.Store.Abstractions.csproj", "src/GreenSystem.Charging.Store.Abstractions/"]
RUN dotnet restore "src/GreenSystem.Charging.Service/GreenSystem.Charging.Service.csproj"
COPY . .
WORKDIR "/src/src/GreenSystem.Charging.Service"
RUN dotnet build "GreenSystem.Charging.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GreenSystem.Charging.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreenSystem.Charging.Service.dll"]
