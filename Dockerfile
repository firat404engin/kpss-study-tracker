# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["KPSSStudyTracker/KPSSStudyTracker.csproj", "KPSSStudyTracker/"]
RUN dotnet restore "KPSSStudyTracker/KPSSStudyTracker.csproj"

# Copy everything and build
COPY . .
WORKDIR "/src/KPSSStudyTracker"
RUN dotnet build "KPSSStudyTracker.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "KPSSStudyTracker.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KPSSStudyTracker.dll"]
