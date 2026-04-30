# upick

upick is a .NET 8 recipe costing application. It includes a PostgreSQL-backed Web API, a Blazor WebAssembly UI, and a shared DTO/model library used by both projects.

## Projects

| Path | Description |
| --- | --- |
| `RecipeCostAPI` | ASP.NET Core Web API for ingredients, recipes, unit conversion, pricing, EF Core migrations, and database seeding. |
| `RecipeCostUI` | Blazor WebAssembly frontend for managing ingredients and recipes. |
| `RecipeCost.Shared` | Shared DTOs, enums, and unit helpers used by the API and UI. |
| `RecipeCostAPI.Tests` | xUnit tests, including PostgreSQL integration tests with Testcontainers. |

## Features

- Manage ingredients with user-entered units and costs.
- Convert ingredient costs into base units for recipe calculations.
- Calculate recipe totals and cost per serving.
- Persist recipes, ingredients, and recipe ingredients in PostgreSQL.
- Share request and response contracts between API and UI.
- Exercise conversion and persistence behavior with automated tests.

## Tech Stack

- .NET 8
- ASP.NET Core Web API
- Blazor WebAssembly
- Entity Framework Core
- PostgreSQL with Npgsql
- Docker Compose
- xUnit
- Testcontainers for PostgreSQL integration tests

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Docker Desktop
- Git

Optional:

- EF Core CLI tools: `dotnet tool install --global dotnet-ef`

## Getting Started

Clone the repository:

```bash
git clone https://github.com/sacost6/upick.git
cd upick
```

Restore and build the solution:

```bash
dotnet restore RecipeCost.sln
dotnet build RecipeCost.sln
```

## Run Locally

Start PostgreSQL from the API Docker Compose file:

```bash
cd RecipeCostAPI
docker compose up -d postgresdata
```

Run the API:

```bash
dotnet run --launch-profile http
```

The API listens on `http://localhost:5210`. In Development, Swagger is available at `http://localhost:5210/swagger`.

In a second terminal, run the UI:

```bash
cd RecipeCostUI
dotnet run --launch-profile http
```

The UI listens on `http://localhost:5168` and is configured to call the API at `http://localhost:5210`.

## Database

The default local connection string is in `RecipeCostAPI/appsettings.json`:

```text
Host=127.0.0.1;Port=5432;Database=RecipeDb;Username=postgres;Password=YourStrong!Passw0rd123
```

On API startup, `DbInitializer` applies EF Core migrations and seeds initial sample data when the database is empty.

To stop the local PostgreSQL container:

```bash
cd RecipeCostAPI
docker compose down
```

To remove the database volume and start fresh:

```bash
cd RecipeCostAPI
docker compose down -v
```

## Tests

Run the API test project:

```bash
dotnet test RecipeCostAPI.Tests/RecipeCostAPI.Tests.csproj
```

The integration tests start an isolated PostgreSQL container, so Docker must be running.

## API Endpoints

Primary endpoints include:

- `GET /api/ingredients`
- `GET /api/ingredients/{id}`
- `POST /api/ingredients`
- `PUT /api/ingredients/{id}`
- `DELETE /api/ingredients/{id}`
- `GET /api/recipe`
- `GET /api/recipe/{id}`
- `POST /api/recipe`
- `PUT /api/recipe/{id}`
- `DELETE /api/recipe/{id}`

## Repository Notes

- Build output, Visual Studio state, and test artifacts are ignored by the root `.gitignore`.
- The previous nested project repositories were flattened so this repository contains the actual source files directly.
- Local development settings are intended for development only; use environment variables or secret management for deployed environments.
