using Microsoft.AspNetCore.Mvc;
using GameTracker.Models;
using System.Linq;

namespace GameTracker.Controllers
{
    public class GameLevelsController : Controller
    {
        private readonly GameTrackerContext _context;

        public GameLevelsController(GameTrackerContext context)
        {
            _context = context;
        }

        // Strażnik dostępu - sprawdza czy w sesji siedzi Admin
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        // GET: /GameLevels/Index (Lista poziomów)
        public IActionResult Index()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            
            var levels = _context.GameLevels.ToList();
            return View(levels);
        }

        // GET: /GameLevels/Create (Formularz dodawania)
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: /GameLevels/Create (Zapisywanie nowego poziomu)
        [HttpPost]
        public IActionResult Create(GameLevel level)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            level.Id = 0; // Wymuszamy, by baza sama nadała ID
            // Zastępujemy kropkę przecinkiem, jeśli ktoś wpisał ją na polskim systemie
            level.DifficultyMultiplier = Convert.ToDouble(level.DifficultyMultiplier.ToString().Replace(".", ","));

            _context.GameLevels.Add(level);
            _context.SaveChanges();
            
            return RedirectToAction("Index");
        }

        // GET: /GameLevels/Edit/5 (Formularz edycji)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var level = _context.GameLevels.Find(id);
            if (level == null) return NotFound();

            return View(level);
        }

        // POST: /GameLevels/Edit/5 (Zapisywanie zmian)
        [HttpPost]
        public IActionResult Edit(GameLevel level)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                _context.GameLevels.Update(level);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(level);
        }

        // POST: /GameLevels/Delete/5 (Usuwanie poziomu)
        [HttpPost]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin()) return RedirectToAction("Index", "Home");

            var level = _context.GameLevels.Find(id);
            if (level != null)
            {
                _context.GameLevels.Remove(level);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}