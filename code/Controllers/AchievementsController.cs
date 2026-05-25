using Microsoft.AspNetCore.Mvc;
using GameTracker.Models;
using System.Linq;

namespace GameTracker.Controllers
{
    public class AchievementsController : Controller
    {
        private readonly GameTrackerContext _context;

        public AchievementsController(GameTrackerContext context)
        {
            _context = context;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // GET: /Achievements/Index
        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            
            var achievements = _context.Achievements.ToList();
            return View(achievements);
        }

        // GET: /Achievements/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: /Achievements/Create
        [HttpPost]
        public IActionResult Create(Achievement achievement)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                _context.Achievements.Add(achievement);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(achievement);
        }

        // GET: /Achievements/Edit/5
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var achievement = _context.Achievements.Find(id);
            if (achievement == null) return NotFound();

            return View(achievement);
        }

        // POST: /Achievements/Edit/5
        [HttpPost]
        public IActionResult Edit(Achievement achievement)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                _context.Achievements.Update(achievement);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(achievement);
        }

        // POST: /Achievements/Delete/5
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var achievement = _context.Achievements.Find(id);
            if (achievement != null)
            {
                _context.Achievements.Remove(achievement);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}