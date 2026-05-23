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
            // 1. Odbieranie danych z nagłówków żądania HTTP (ZADANIE 3)
            if (!Request.Headers.TryGetValue("X-Player-Login", out var requestLogin) ||
                !Request.Headers.TryGetValue("X-Api-Key", out var requestApiKey))
            {
                // Jeśli brakuje któregoś z nagłówków, przerywamy i zwracamy kod 401 (Brak autoryzacji)
                return Unauthorized("Brak wymaganych nagłówków autoryzacyjnych: X-Player-Login lub X-Api-Key.");
            }

            // 2. Weryfikacja: Szukamy użytkownika o podanym loginie i sprawdzamy jego ApiKey
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Login == requestLogin.ToString() && u.ApiKey == requestApiKey.ToString());

            if (user == null)
            {
                return Unauthorized("Nieautoryzowany dostęp: Niepoprawny login lub klucz API.");
            }

            // 3. Sprawdzamy czy poziom gry istnieje w bazie
            var level = await _context.GameLevels.FindAsync(scoreData.GameLevelId);
            if (level == null)
            {
                return BadRequest("Nie znaleziono poziomu o podanym ID.");
            }

            // 4. Bezpieczeństwo: Upewniamy się, że gracz wysyła wynik dla SAMEGO SIEBIE
            // (Zapobiega to sytuacji, gdzie Gracz A wysyła punkty na konto Gracza B)
            if (user.Id != scoreData.UserId)
            {
                return BadRequest("Identyfikator zalogowanego użytkownika nie zgadza się z ID w przesyłanym wyniku.");
            }

            // 5. Tworzymy i zapisujemy nowy wynik
            var newScore = new Score
            {
                UserId = user.Id,
                GameLevelId = scoreData.GameLevelId,
                Points = scoreData.Points,
                DateAchieved = DateTime.Now
            };

            _context.Scores.Add(newScore);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Wynik zapisany pomyślnie przez API!", scoreId = newScore.Id });
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