# Use the official .NET 8.0 runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the .NET 8.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the project file and restore dependencies
COPY ProjectPulseAPI/ProjectPulseAPI.csproj ProjectPulseAPI/
RUN dotnet restore "ProjectPulseAPI/ProjectPulseAPI.csproj"

# Copy the entire source code
COPY . .
WORKDIR /src/ProjectPulseAPI

# Build the application
RUN dotnet build "ProjectPulseAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ProjectPulseAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage - create the runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Create a non-root user for security
RUN addgroup --system --gid 1001 dotnetgroup
RUN adduser --system --uid 1001 --ingroup dotnetgroup dotnetuser
USER dotnetuser

# Set environment variables for production
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "ProjectPulseAPI.dll"]