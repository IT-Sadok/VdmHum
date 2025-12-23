FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Identity/Domain/Domain.csproj", "Identity/Domain/"]
COPY ["Identity/Application/Application.csproj", "Identity/Application/"]
COPY ["Identity/Infrastructure/Infrastructure.csproj", "Identity/Infrastructure/"]
COPY ["Identity/Presentation/Presentation.csproj", "Identity/Presentation/"]
COPY ["Shared.Contracts/Shared.Contracts.csproj", "Shared.Contracts/"]

RUN dotnet restore "Identity/Presentation/Presentation.csproj"

COPY . .

WORKDIR "/src/Identity/Presentation"

RUN dotnet publish "Presentation.csproj" \
      -c $BUILD_CONFIGURATION \
      -o /app/publish \
      /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]