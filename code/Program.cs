using Microsoft.EntityFrameworkCore;
using GameTracker.Models;

var builder = WebApplication.CreateBuilder(args);

// Dodanie obsługi bazy danych SQLite
builder.Services.AddDbContext<GameTrackerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//data seeding
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<GameTrackerContext>();
        
        // Upewniamy się, że baza danych jest stworzona
        context.Database.EnsureCreated();

        // Jeśli nie ma żadnych użytkowników, dodaj pierwszego (Admina)
        if (!context.Users.Any())
        {
            context.Users.Add(new User 
            { 
                Login = "admin", 
                PasswordHash = "hashed_password_here", // W Etapie 2 podmienimy to na realny hash
                Role = "Admin",
                ApiKey = "API-KEY-123" // Ten klucz podasz w symulatorze
            });
            context.SaveChanges();
            Console.WriteLine("--> Dodano domyślnego administratora.");
        }

        // Jeśli nie ma żadnych poziomów, dodaj przykładowy poziom
        if (!context.GameLevels.Any())
        {
            context.GameLevels.Add(new GameLevel
            {
                Name = "Poziom Testowy",
                DifficultyMultiplier = 1.0
            });
            context.SaveChanges();
            Console.WriteLine("--> Dodano domyślny poziom gry.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("--> Wystąpił błąd podczas inicjalizacji bazy danych: " + ex.Message);
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();