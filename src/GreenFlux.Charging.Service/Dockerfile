#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GreenFlux.Charging.Service/GreenFlux.Charging.Service.csproj", "src/GreenFlux.Charging.Service/"]
COPY ["src/GreenFlux.Charging.Groups.WebApi/GreenFlux.Charging.Groups.WebApi.csproj", "src/GreenFlux.Charging.Groups.WebApi/"]
COPY ["src/GreenFlux.Charging.Groups.Abstractions/GreenFlux.Charging.Groups.Abstractions.csproj", "src/GreenFlux.Charging.Groups.Abstractions/"]
COPY ["src/GreenFlux.Charging.Abstractions/GreenFlux.Charging.Abstractions.csproj", "src/GreenFlux.Charging.Abstractions/"]
COPY ["src/GreenFlux.Charging.Groups/GreenFlux.Charging.Groups.csproj", "src/GreenFlux.Charging.Groups/"]
COPY ["src/GreenFlux.Charging.Groups.Store/GreenFlux.Charging.Groups.Store.csproj", "src/GreenFlux.Charging.Groups.Store/"]
COPY ["src/GreenFlux.Charging.Store/GreenFlux.Charging.Store.csproj", "src/GreenFlux.Charging.Store/"]
COPY ["src/GreenFlux.Charging.Store.Abstractions/GreenFlux.Charging.Store.Abstractions.csproj", "src/GreenFlux.Charging.Store.Abstractions/"]
RUN dotnet restore "src/GreenFlux.Charging.Service/GreenFlux.Charging.Service.csproj"
COPY . .
WORKDIR "/src/src/GreenFlux.Charging.Service"
RUN dotnet build "GreenFlux.Charging.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GreenFlux.Charging.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GreenFlux.Charging.Service.dll"]
