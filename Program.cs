using Microsoft.EntityFrameworkCore;
using GameCharactersAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure In-Memory Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("GameCharactersDB"));

var app = builder.Build();

// Enable Swagger for both Development and Production
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GameCharactersAPI v1");
    c.RoutePrefix = "swagger"; // Swagger will be at /swagger
});

// Enable HTTPS redirection and Authorization
app.UseHttpsRedirection();
app.UseAuthorization();

// Map API controllers
app.MapControllers();

// Seed some example data (optional, useful for testing)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Characters.Any())
    {
        db.Characters.AddRange(
            new GameCharactersAPI.Models.Character { Name = "Arthas", Game = "Warcraft", Level = 60, Role = "Tank" },
            new GameCharactersAPI.Models.Character { Name = "Zelda", Game = "Zelda", Level = 45, Role = "Support" },
            new GameCharactersAPI.Models.Character { Name = "Mario", Game = "Mario", Level = 55, Role = "DPS" }
        );
        db.SaveChanges();
    }
}

// Run the app
app.Run();