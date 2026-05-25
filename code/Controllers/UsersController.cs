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
