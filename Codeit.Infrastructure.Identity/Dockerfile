#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Codeit.Infrastructure.Identity/Codeit.Infrastructure.Identity.csproj", "Codeit.Infrastructure.Identity/"]
COPY ["Codeit.Infrastructure.Identity.DAL/Codeit.Infrastructure.Identity.DAL.csproj", "Codeit.Infrastructure.Identity.DAL/"]
COPY ["Codeit.Infrastructure.Identity.Model/Codeit.Infrastructure.Identity.Model.csproj", "Codeit.Infrastructure.Identity.Model/"]
RUN dotnet restore "Codeit.Infrastructure.Identity/Codeit.Infrastructure.Identity.csproj"
COPY . .
WORKDIR "/src/Codeit.Infrastructure.Identity"
RUN dotnet build "Codeit.Infrastructure.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Codeit.Infrastructure.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Codeit.Infrastructure.Identity.dll"]
