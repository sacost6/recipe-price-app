# RecipeCostAPI 🍳

A .NET 8 Web API designed to calculate and manage the costs of culinary recipes. This project uses **PostgreSQL** for data storage and **Entity Framework Core** for ORM, fully containerized with **Docker**.



---

## 🚀 Features
* **Ingredient Management:** Track costs based on specific units (Gram, Kilogram, etc.).
* **Recipe Costing:** Automatic calculation of total recipe costs and cost-per-serving.
* **DTO Mapping:** Clean separation between Database Models and API Response shapes.
* **Containerized Infrastructure:** Ready-to-go database and cache layers using Docker Compose.

---

## 🛠️ Tech Stack
* **Framework:** .NET 8.0
* **Database:** PostgreSQL 16 (via Docker)
* **ORM:** Entity Framework Core (Npgsql)
* **Containerization:** Docker & Docker Compose

---

## 🏁 Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* [Docker Desktop](https://www.docker.com/products/docker-desktop/)
* [EF Core Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) (`dotnet tool install --global dotnet-ef`)


### Installation & Setup

1. **Clone the repository:**
  ```bash
   git clone <your-repo-url>
   cd RecipeCostAPI
```

3. **Restore NuGet Packages:**
```bash 
   dotnet restore
```

4. **Spin up the Infrastructure:**
```bash
   Ensure Docker is running, then start the PostgreSQL container:
   docker-compose up -d
```

5. **Apply Database Migrations:**
  ```bash
   This creates the tables in your Docker container:
   dotnet ef database update
```

6. **Run the API:**
```bash
   dotnet run
```
The API will typically be available at http://localhost:5000 or http://localhost:5200 (check Properties/launchSettings.json).

---

### Project Structure
* **Models/**: Database entities (Recipe, Ingredient, RecipeIngredient).
* **DTOs/**: Data Transfer Objects for API requests/responses.
* **Mappers/**: Extension methods to convert Entities to DTOs.
* **Data/**: AppDbContext and Entity Framework configurations.
* **Services/**: Business logic and pricing calculation engines.

---

### Troubleshooting

**Port Conflicts (5432)**
If you have a local instance of PostgreSQL running, it may block the Docker container.
* To stop local Postgres (Windows): Run PowerShell as Admin: Stop-Service -Name postgresql*
* Alternative: Change the port mapping in docker-compose.yml to "5433:5432" and update your appsettings.json connection string.

**Authentication Errors**
If you see "password authentication failed", ensure your appsettings.json connection string matches the POSTGRES_PASSWORD in docker-compose.yml.
* Note: If you change the password in Docker after the first run, you must run "docker-compose down -v" to reset the volume.

---

### License
This project is licensed under the MIT License.
