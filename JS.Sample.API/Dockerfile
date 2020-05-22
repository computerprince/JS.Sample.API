#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["JS.Sample.API/JS.Sample.API.csproj", "JS.Sample.API/"]
RUN dotnet restore "JS.Sample.API/JS.Sample.API.csproj"
COPY . .
WORKDIR "/src/JS.Sample.API"
RUN dotnet build "JS.Sample.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JS.Sample.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JS.Sample.API.dll"]