using Microsoft.AspNetCore.Mvc;
using GameTracker.Models;
using GameTracker.Helpers;

namespace GameTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly GameTrackerContext _context;

        public AccountController(GameTrackerContext context)
        {
            _context = context;
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public IActionResult Login(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Login i hasło są wymagane.");
                return View();
            }

            // Szukamy użytkownika w bazie
            var user = _context.Users.FirstOrDefault(u => u.Login == login);

            if (user == null || !PasswordHelper.VerifyPassword(password, user.PasswordHash))
            {
                ModelState.AddModelError("", "Nieprawidłowy login lub hasło.");
                return View();
            }

            // Zapisanie użytkownika w sesji
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserLogin", user.Login);
            HttpContext.Session.SetString("UserRole", user.Role);

            return RedirectToAction("Index", "Home");
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
