FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GingerCodeSite/GingerCodeSite.csproj", "GingerCodeSite/"]
RUN dotnet restore "GingerCodeSite/GingerCodeSite.csproj"
COPY . .
WORKDIR "/src/GingerCodeSite"
RUN dotnet build "GingerCodeSite.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GingerCodeSite.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GingerCodeSite.dll"]
