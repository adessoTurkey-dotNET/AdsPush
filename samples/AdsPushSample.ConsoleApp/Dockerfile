FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["samples/AdsPushSample.ConsoleApp/AdsPushSample.ConsoleApp.csproj", "samples/AdsPushSample.ConsoleApp/"]
RUN dotnet restore "samples/AdsPushSample.ConsoleApp/AdsPushSample.ConsoleApp.csproj"
COPY . .
WORKDIR "/src/samples/AdsPushSample.ConsoleApp"
RUN dotnet build "AdsPushSample.ConsoleApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AdsPushSample.ConsoleApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AdsPushSample.ConsoleApp.dll"]
