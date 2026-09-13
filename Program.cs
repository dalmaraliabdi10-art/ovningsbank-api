using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using OvningsbankApi.Data;
using OvningsbankApi.Services;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "OvningsbankClients";

// ---- Tjänster -------------------------------------------------------------

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Enum som text i JSON ("Passningsspel" istället för 1). Gör svaren
        // självförklarande i Swagger och gör att klienterna slipper en
        // kopia av enum-ordningen som kan hamna i otakt.
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")));

// Scoped: en instans per HTTP-anrop, samma livslängd som DbContext.
builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
builder.Services.AddScoped<IExerciseService, ExerciseService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

// CORS-origins läses från appsettings istället för att hårdkodas, så att
// mobilappens adress (datorns IP i det lokala nätet) kan läggas till utan
// att koden ändras.
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                     ?? new[] { "http://localhost:5173" };

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy => policy
        .WithOrigins(allowedOrigins)
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();

// ---- Databas --------------------------------------------------------------

// EnsureCreated istället för migrationer: schemat är fast under kursen och
// den som klonar repot ska kunna köra "dotnet run" utan att först installera
// dotnet-ef och köra en migration.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
    DbSeeder.Seed(db);
}

// ---- Pipeline -------------------------------------------------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Serverar wwwroot, vilket gör uppladdade bilder åtkomliga på /uploads/<filnamn>.
app.UseStaticFiles();

app.UseCors(CorsPolicy);

app.MapControllers();

app.Run();
