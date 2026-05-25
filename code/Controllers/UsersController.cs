using Microsoft.AspNetCore.Mvc;
using GameTracker.Models;
using GameTracker.Helpers;

namespace GameTracker.Controllers
{
    public class UsersController : Controller
    {
        private readonly GameTrackerContext _context;

        public UsersController(GameTrackerContext context)
        {
            _context = context;
        }

        // Metoda pomocnicza - sprawdza czy zalogowany jest Admin
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // GET: /Users/Index (Wyświetlanie listy użytkowników)
        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home"); // Wyrzuca intruza

            var users = _context.Users.ToList();
            return View(users);
        }

        // GET: /Users/Create (Wyświetlanie formularza dodawania)
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: /Users/Create (Odbieranie danych z formularza)
        [HttpPost]
        public IActionResult Create(string login, string password, string role)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            // Sprawdzamy czy login jest już zajęty
            if (_context.Users.Any(u => u.Login == login))
            {
                ModelState.AddModelError("", "Użytkownik o takim loginie już istnieje!");
                return View();
            }

            // Automatycznie generujemy unikalny ApiKey dla nowego gracza
            string generatedApiKey = Guid.NewGuid().ToString("N");

            // Tworzymy nowego użytkownika
            var newUser = new User
            {
                Login = login,
                PasswordHash = PasswordHelper.HashPassword(password), // Znowu używamy świetnego helpera Osoby B!
                Role = string.IsNullOrEmpty(role) ? "Player" : role,
                ApiKey = generatedApiKey
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Index"); // Po dodaniu wracamy do listy
        }
    }
}