# RecipeCostUI

A modern web-based user interface for managing recipe ingredients and calculating recipe costs. Built with Blazor WebAssembly, this application provides an intuitive interface for culinary professionals and home cooks to track ingredient costs and manage recipes efficiently.

## Features

- **Ingredient Management**: Add, view, and manage ingredients with their associated costs
- **Cost Tracking**: Track cost per base unit for each ingredient
- **Unit Support**: Define and manage different units of measurement for ingredients
- **Responsive Design**: Clean, modern UI built with Blazor components

## Tech Stack

- **Framework**: ASP.NET Core Blazor WebAssembly
- **Frontend**: Razor Components, Bootstrap CSS
- **Language**: C#
- **HTTP Client**: Built-in HttpClient for API communication

## Prerequisites

Before running this project, ensure you have:

- **.NET SDK** 6.0 or higher
- **Visual Studio 2022** or **Visual Studio Code**
- **RecipeCost API** running locally (default: `http://localhost:5210`)

## Getting Started

### Installation

1. Clone the repository:
```
git clone https://github.com/sacost6/RecipeCostUI.git
cd RecipeCostUI
```

2. Restore dependencies:
```
dotnet restore
```

3. Build the project:
```
dotnet build
```

### Running the Application

1. Ensure the **RecipeCost API** is running on `http://localhost:5210`

2. Run the Blazor WebAssembly application:
```
dotnet run
```

3. Open your browser and navigate to `https://localhost:5001` (or the URL displayed in the console)

## Project Structure

```
RecipeCostUI/
??? Pages/
?   ??? Ingredients.razor        # Ingredient management page
??? Layout/
?   ??? MainLayout.razor         # Main application layout
?   ??? NavMenu.razor            # Navigation menu component
??? Services/
?   ??? IngredientService.cs    # API communication service
??? App.razor                    # Main app component with routing
??? Program.cs                   # Application configuration
```

## Usage

### Managing Ingredients

1. Navigate to the **Ingredients** page from the main menu
2. **Add New Ingredient**:
   - Enter the ingredient name
   - Specify the cost per base unit
   - Define the base unit (e.g., kg, cup, tablespoon)
   - Click **Save**
3. View all ingredients in the table below the form

## API Integration

The application communicates with the RecipeCost API through the `IngredientService`:

- **Base Address**: `http://localhost:5210`
- **Endpoints**:
  - `GET /api/ingredients` - Retrieve all ingredients
  - `POST /api/ingredients` - Create a new ingredient

## Configuration

Update the API base address in `Program.cs` if your API is running on a different port:

```csharp
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://your-api-url:port") });
```

## Development

### Adding New Features

- Create new Razor components in the `Pages/` directory for new pages
- Add service methods in `Services/` for API communication
- Import shared models from the `RecipeCost.Shared` package

### Building for Production

```
dotnet publish -c Release
```

The build output will be in the `bin/Release/net6.0/publish/` directory.

## Troubleshooting

### Connection Refused
- Verify the RecipeCost API is running on `http://localhost:5210`
- Check firewall settings and CORS configuration on the API

### Blank Page
- Open browser developer tools (F12) and check the console for errors
- Ensure all dependencies are properly restored

## Contributing

Contributions are welcome! Please follow the project's coding standards and submit pull requests to the main repository.

## License

This project is part of the RecipeCost application suite.

## Contact & Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/sacost6/RecipeCostUI).
