using Microsoft.AspNetCore.Mvc;
<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using GameTracker.Helpers;
using GameTracker.Models;
using GameTracker.ViewModels;
=======
using GameTracker.Models;
using GameTracker.Helpers;
>>>>>>> d1e6f029f72950e120414f1a398a648eec697857

namespace GameTracker.Controllers
{
    public class UsersController : Controller
    {
        private readonly GameTrackerContext _context;

        public UsersController(GameTrackerContext context)
        {
            _context = context;
        }

<<<<<<< HEAD
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

        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                return RedirectToLogin();
            }

            return View(new CreateUserViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateUserViewModel model)
        {
            if (!IsAdmin())
            {
                return RedirectToLogin();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_context.Users.Any(u => u.Login == model.Login))
            {
                ModelState.AddModelError(nameof(model.Login), "Użytkownik o tej nazwie już istnieje.");
                return View(model);
            }

            var user = new User
            {
                Login = model.Login,
                PasswordHash = PasswordHelper.HashPassword(model.Password),
                Role = model.Role,
                ApiKey = string.IsNullOrWhiteSpace(model.ApiKey) ? Guid.NewGuid().ToString("N") : model.ApiKey
            };

            _context.Users.Add(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
=======
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
>>>>>>> d1e6f029f72950e120414f1a398a648eec697857
