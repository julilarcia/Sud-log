using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameTracker.Models;

namespace GameTracker.Api
{
    //Konfiguracja trasy i typu kontrolera
    [Route("api/[controller]")]
    [ApiController]
    public class ScoresApiController : ControllerBase
    {
        private readonly GameTrackerContext _context;

        
        public ScoresApiController(GameTrackerContext context)
        {
            _context = context;
        }

        // Metoda GET 
        // Wywołanie: GET /api/scoresapi
        [HttpGet]
        public async Task<IActionResult> GetScores()
        {
            // Pobieramy wyniki z bazy, dołączając do nich loginy graczy i nazwy poziomów
            var scores = await _context.Scores
                .Include(s => s.User)
                .Include(s => s.GameLevel)
                .Select(s => new 
                {
                    ScoreId = s.Id,
                    Player = s.User.Login,
                    Level = s.GameLevel.Name,
                    Points = s.Points,
                    Date = s.DateAchieved
                })
                .ToListAsync();

            return Ok(scores); // Zwraca kod 200 i dane w formacie JSON
        }

        // Metoda POST 
        // Wywołanie: POST /api/scoresapi
        [HttpPost]
        public async Task<IActionResult> PostScore([FromBody] ScoreSubmitDto scoreData)
        {
            // 1. Sprawdzamy czy gracz i poziom istnieją w bazie
            var user = await _context.Users.FindAsync(scoreData.UserId);
            var level = await _context.GameLevels.FindAsync(scoreData.GameLevelId);

            if (user == null || level == null)
            {
                return BadRequest("Nie znaleziono gracza lub poziomu o podanym ID.");
            }

            // 2. utworzenia nowego wyniku
            var newScore = new Score
            {
                UserId = scoreData.UserId,
                GameLevelId = scoreData.GameLevelId,
                Points = scoreData.Points,
                DateAchieved = DateTime.Now
            };

            // 3. Zapis w bazie
            _context.Scores.Add(newScore);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Wynik zapisany pomyślnie!", scoreId = newScore.Id });
        }
    }

    //  klasa pomocnicza (Data Transfer Object) do odbierania danych z gry
    public class ScoreSubmitDto
    {
        public int UserId { get; set; }
        public int GameLevelId { get; set; }
        public int Points { get; set; }
    }
}