# ==========================================
# STAGE 1: Build and Publish
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files first so restore can be cached
COPY ["RecipeCostAPI/RecipeCostAPI.csproj", "RecipeCostAPI/"]
COPY ["RecipeCost.Shared/RecipeCost.Shared.csproj", "RecipeCost.Shared/"]
RUN dotnet restore "RecipeCostAPI/RecipeCostAPI.csproj"

COPY . .

# Build and publish the application
WORKDIR "/src/RecipeCostAPI"
RUN dotnet publish "RecipeCostAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ==========================================
# STAGE 2: Run (The Production Image)
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# The port .NET 8 uses by default in containers
EXPOSE 8080

# Copy the compiled files from Stage 1 into this new Stage 2 container
COPY --from=build /app/publish .

# Tell the container to boot up the web server and keep running
ENTRYPOINT ["dotnet", "RecipeCostAPI.dll"]
