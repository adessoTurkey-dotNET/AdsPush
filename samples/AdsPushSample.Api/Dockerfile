FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["samples/AdsPushSampleApi/AdsPushSampleApi.csproj", "samples/AdsPushSampleApi/"]
RUN dotnet restore "samples/AdsPushSampleApi/AdsPushSampleApi.csproj"
COPY . .
WORKDIR "/src/samples/AdsPushSampleApi"
RUN dotnet build "AdsPushSampleApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdsPushSampleApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdsPushSampleApi.dll"]
