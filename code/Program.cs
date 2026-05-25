using Microsoft.EntityFrameworkCore;
using GameTracker.Models;
using GameTracker.Helpers; // Wymagane do hashowania hasła!

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GameTrackerContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// 1. DODANA OBSŁUGA SESJI (Zadanie 1 Osoby B)
// ==========================================
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// ==========================================
// 2. INICJALIZACJA DANYCH I NAPRAWA BAZY
// ==========================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<GameTrackerContext>();
        context.Database.EnsureCreated();

        // SPOSÓB NA ZEPSUTE HASŁO: Szukamy starego admina i go usuwamy
        var brokenAdmin = context.Users.FirstOrDefault(u => u.Login == "admin" && u.PasswordHash == "hashed_password_here");
        if (brokenAdmin != null)
        {
            context.Users.Remove(brokenAdmin);
            context.SaveChanges();
            Console.WriteLine("--> Usunięto starego admina z zepsutym hasłem.");
        }

        // Dodajemy admina z prawdziwym hashem
        if (!context.Users.Any(u => u.Login == "admin"))
        {
            context.Users.Add(new User 
            { 
                Login = "admin", 
                PasswordHash = PasswordHelper.HashPassword("admin123"), // Poprawny Hash!
                Role = "Admin",
                ApiKey = "API-KEY-123" 
            });
            context.SaveChanges();
            Console.WriteLine("--> Dodano administratora z POPRAWNYM hashem.");
        }

        if (!context.GameLevels.Any())
        {
            context.GameLevels.Add(new GameLevel { Name = "Poziom Testowy", DifficultyMultiplier = 1.0 });
            context.SaveChanges();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("--> Wystąpił błąd DB: " + ex.Message);
    }
}

// ==========================================
// 3. KONFIGURACJA ŚRODOWISKA
// ==========================================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// NAPRAWA BŁĘDÓW 404 (Pozwala przeglądarce pobrać pliki CSS/JS)
app.UseStaticFiles(); 

app.UseRouting();

// NAPRAWA LOGOWANIA (Pozwala zapisywać dane w sesji po wpisaniu dobrego hasła)
app.UseSession(); 

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();