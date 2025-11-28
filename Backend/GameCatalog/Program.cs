using GameCatalog.Repositories;
using GameCatalog.Services;
using System.Text.Json.Serialization;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Register our services
builder.Services.AddSingleton<IGameRepository, InMemoryGameRepository>();
builder.Services.AddSingleton<GameService>();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseCors("AllowAll");

// Serve static files from Frontend folder - go up to GameCatalog root, then to Frontend
var currentDir = AppContext.BaseDirectory; // bin/Debug/net10.0/
var projectRoot = Path.Combine(currentDir, "..", "..", "..");
var frontendPath = Path.Combine(projectRoot, "..", "..", "Frontend");
frontendPath = Path.GetFullPath(frontendPath);

Console.WriteLine($"📁 Frontend path: {frontendPath}");

if (!Directory.Exists(frontendPath))
{
    Console.WriteLine($"⚠️ Frontend path not found at: {frontendPath}");
}

// UseDefaultFiles MUST come before UseStaticFiles
app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(frontendPath)
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(frontendPath),
    RequestPath = ""
});

app.MapControllers();

Console.WriteLine("🐉 Game of Thrones Item Catalog is running!");
Console.WriteLine("🌐 Web UI: http://localhost:5000");

app.Run("http://localhost:5000");
