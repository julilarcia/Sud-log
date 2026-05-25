using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: Account/Profile
        public IActionResult Profile()
        {
            var userId = GetLoggedInUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login");
            }

            var user = _context.Users
                .Include(u => u.Scores)
                .ThenInclude(s => s.GameLevel)
                .Include(u => u.UserAchievements)
                .ThenInclude(ua => ua.Achievement)
                .FirstOrDefault(u => u.Id == userId.Value);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new GameTracker.ViewModels.ProfileViewModel
            {
                Login = user.Login,
                Role = user.Role,
                TotalPoints = user.Scores?.Sum(s => s.Points) ?? 0,
                GamesPlayed = user.Scores?.Count() ?? 0,
                Achievements = user.UserAchievements?.OrderByDescending(ua => ua.DateUnlocked).Select(ua => new GameTracker.ViewModels.AchievementRowViewModel
                {
                    Name = ua.Achievement?.Name ?? "",
                    Description = ua.Achievement?.Description ?? string.Empty,
                    UnlockedAt = ua.DateUnlocked
                }).ToList() ?? new List<GameTracker.ViewModels.AchievementRowViewModel>(),
                ScoreHistory = user.Scores?.OrderByDescending(s => s.DateAchieved).Select(s => new GameTracker.ViewModels.ScoreHistoryRowViewModel
                {
                    LevelName = s.GameLevel?.Name ?? "-",
                    Points = s.Points,
                    DateAchieved = s.DateAchieved
                }).ToList() ?? new List<GameTracker.ViewModels.ScoreHistoryRowViewModel>()
            };

            return View(model);
        }

        private int? GetLoggedInUserId()
        {
            var idString = HttpContext.Session.GetString("UserId");
            if (int.TryParse(idString, out var id))
            {
                return id;
            }
            return null;
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
