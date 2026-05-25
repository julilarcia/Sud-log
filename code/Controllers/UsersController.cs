using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameTracker.Helpers;
using GameTracker.Models;
using GameTracker.ViewModels;

namespace GameTracker.Controllers
{
    public class UsersController : Controller
    {
        private readonly GameTrackerContext _context;

        public UsersController(GameTrackerContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return string.Equals(HttpContext.Session.GetString("UserRole"), "Admin", StringComparison.OrdinalIgnoreCase);
        }

        private IActionResult RedirectToLogin()
        {
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Index()
        {
            if (!IsAdmin())
            {
                return RedirectToLogin();
            }

            var users = _context.Users
                .Select(u => new UserListItemViewModel
                {
                    Id = u.Id,
                    Login = u.Login,
                    Role = u.Role,
                    ApiKey = u.ApiKey
                })
                .ToList();

            return View(users);
        }

        [HttpPost]
        public IActionResult Create(string login, string password, string role)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            // 1. ZABEZPIECZENIE: Jeśli dane są puste, przerwij i pokaż błąd
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Login i hasło nie mogą być puste!");
                return View();
            }

            // 2. Sprawdzamy czy login jest już zajęty
            if (_context.Users.Any(u => u.Login == login))
            {
                ModelState.AddModelError("", "Użytkownik o takim loginie już istnieje!");
                return View();
            }

            // Generujemy unikalny ApiKey dla gracza
            string generatedApiKey = Guid.NewGuid().ToString("N");

            // Tworzymy użytkownika
            var newUser = new User
            {
                Id = 0, // Wymuszenie autoincrement w SQLite
                Login = login.Trim(),
                PasswordHash = PasswordHelper.HashPassword(password), // Bezpieczny hash
                Role = string.IsNullOrEmpty(role) ? "Player" : role,
                ApiKey = generatedApiKey
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
