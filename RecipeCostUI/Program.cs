using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using RecipeCost.Shared; // This allows the UI to use your shared models
using RecipeCostUI;
using System.Text.Json;
using System.Text.Json.Serialization;  

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configure JSON serialization options for proper enum handling
var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true,
    Converters = { new JsonStringEnumConverter() }
};

// This is your "Phone" to the API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5210") });

// Provide the JSON options as a singleton service so they can be injected where needed
builder.Services.AddSingleton(jsonOptions);

// This registers your service so pages can use it
builder.Services.AddScoped<IngredientService>();

await builder.Build().RunAsync();
