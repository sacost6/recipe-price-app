using Microsoft.EntityFrameworkCore;
using RecipeCostAPI.Data;
using RecipeCostAPI.Middleware;
using RecipeCostAPI.Services;
using RecipeCostAPI.Services.Interfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add Controller Support
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Handle potential reference loops
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

        // Tells the API to send "Gram" instead of 0
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());

        // 
    });

// Register DIs for services
builder.Services.AddScoped<IConverterService, ConverterService>();
builder.Services.AddScoped<IPricingService, PricingService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped<IIngredientService, IngredientService>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// Get connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add DbContext using Npgql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));


// Services Area 
// Define a string variable for the name of our policy
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add CORS services to the cotainer and define a policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000",
                                             "http://localhost:5168",
                                             "https://localhost:5168",
                                             "http://localhost:5210",
                                             "https://localhost:5210")
                            
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
                      
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// SEED THE DATABASE
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<AppDbContext>();
        await DbInitializer.SeedData(context);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

// ==========================================
// THE MIDDLEWARE PIPELINE IS VERY IMPORTANT IN ASP.NET CORE
// ==========================================
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();

// IMPORTANT: app.UseCors must go here!
// Must be placed AFTER app.UseHttpsRedirection() and BEFORE app.MapGet() (or any other MapXXX) and BEFORE auth
app.UseCors(MyAllowSpecificOrigins);
app.MapControllers(); 

app.Run();

public partial class Program
{
}
