FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["CoronaVirus/CoronaVirus.csproj", "CoronaVirus/"]
RUN dotnet restore "CoronaVirus/CoronaVirus.csproj"
COPY . .
WORKDIR "/src/CoronaVirus"
RUN dotnet build "CoronaVirus.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CoronaVirus.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CoronaVirus.dll"]