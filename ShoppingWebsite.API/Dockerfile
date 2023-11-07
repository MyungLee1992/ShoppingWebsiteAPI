#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#ENV ASPNETCORE_URLS=https://+:443
#ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ShoppingWebsite.API/ShoppingWebsiteAPI.csproj", "ShoppingWebsite.API/"]
RUN dotnet restore "ShoppingWebsite.API/ShoppingWebsiteAPI.csproj"
COPY . .
WORKDIR "/src/ShoppingWebsite.API"
RUN dotnet build "ShoppingWebsiteAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ShoppingWebsiteAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ShoppingWebsiteAPI.dll"]