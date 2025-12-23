FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["Bookings/Domain/Domain.csproj", "Bookings/Domain/"]
COPY ["Bookings/Infrastructure/Infrastructure.csproj", "Bookings/Infrastructure/"]
COPY ["Bookings/Application/Application.csproj", "Bookings/Application/"]
COPY ["Bookings/Presentation/Presentation.csproj", "Bookings/Presentation/"]
COPY ["Shared.Contracts/Shared.Contracts.csproj", "Shared.Contracts/"]
COPY ["Shared.Contracts.Grpc/Shared.Contracts.Grpc.csproj", "Shared.Contracts.Grpc/"]

RUN dotnet restore "Bookings/Presentation/Presentation.csproj"

COPY . .

WORKDIR "/src/Bookings/Presentation"

RUN dotnet publish "Presentation.csproj" \
      -c $BUILD_CONFIGURATION \
      -o /app/publish \
      /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Presentation.dll"]