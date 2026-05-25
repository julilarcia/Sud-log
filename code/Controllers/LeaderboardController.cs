using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameTracker.Models;
using GameTracker.ViewModels;

namespace GameTracker.Controllers
{
    public class LeaderboardController : Controller
    {
        private readonly GameTrackerContext _context;

        public LeaderboardController(GameTrackerContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var leaderboard = _context.Scores
                .Include(s => s.User)
                .GroupBy(s => new { s.UserId, s.User.Login })
                .Select(group => new LeaderboardRowViewModel
                {
                    Login = group.Key.Login,
                    TotalPoints = group.Sum(s => s.Points),
                    GamesPlayed = group.Count(),
                    LastPlayed = group.Max(s => s.DateAchieved)
                })
                .OrderByDescending(r => r.TotalPoints)
                .ThenByDescending(r => r.LastPlayed)
                .ToList();

            return View(leaderboard);
        }
    }
}
