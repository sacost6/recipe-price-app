# RecipeCost.Shared

A shared C# library for the RecipeCost application that contains common data transfer objects and shared utilities.

## Overview

`RecipeCost.Shared` is a .NET class library that provides shared components used across the RecipeCost application ecosystem. It contains data models and DTOs that facilitate communication between different parts of the RecipeCost system.

## Features

- **Ingredient DTO**: Data transfer object for managing ingredient information
  - Unique identifier
  - Ingredient name
  - Base unit of measurement (e.g., Gram, oz, cup)
  - Cost per base unit for cost calculations

## Requirements

- .NET 8.0 or later
- C# 11 or later

## Installation

This is a shared library intended to be referenced by other RecipeCost projects:

```bash
dotnet add reference RecipeCost.Shared
```

## Project Structure

```
RecipeCost.Shared/
??? RecipeCost.Shared.csproj  # Project file
??? IngredientDto.cs           # Ingredient data transfer object
??? README.md                  # This file
```

## Usage

### IngredientDto

The `IngredientDto` class represents an ingredient with its associated cost information:

```csharp
using RecipeCost.Shared;

var ingredient = new IngredientDto
{
    Id = 1,
    Name = "Flour",
    BaseUnit = "Gram",
    CostPerBaseUnit = 0.002m // $0.002 per gram
};
```

## Building

To build the project:

```bash
dotnet build
```

To build in Release mode:

```bash
dotnet build --configuration Release
```

## Related Projects

- **RecipeCost**: Main application that uses this shared library
- Repository: https://github.com/sacost6/RecipeCost

## Contributing

When adding new shared components to this library:

1. Ensure new classes follow the existing naming conventions (e.g., DTOs should end with `Dto`)
2. Maintain nullable reference type checking enabled
3. Use implicit usings where appropriate
4. Update this README with any new features or usage examples

## License

See the main RecipeCost repository for license information.
