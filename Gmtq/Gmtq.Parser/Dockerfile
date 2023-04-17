FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Gmtq.Parser/Gmtq.Parser.csproj", "Gmtq.Parser/"]
RUN dotnet restore "Gmtq.Parser/Gmtq.Parser.csproj"
COPY . .
WORKDIR "/src/Gmtq.Parser"
RUN dotnet build "Gmtq.Parser.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gmtq.Parser.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gmtq.Parser.dll"]
