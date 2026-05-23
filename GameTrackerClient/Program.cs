using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GameTrackerClient
{
    class Program
    {
        // Tworzymy jednego, statycznego klienta HTTP do komunikacji sieciowej
        private static readonly HttpClient client = new HttpClient();

        static async Task Main(string[] args)
        {
            Console.WriteLine("=== SYMULATOR KLIENTA GRY (REST API) ===");

            
            string apiUrl = "http://localhost:5264/api/scoresapi";

            // Dane testowe, które powinny być w bazie danych po Waszym pierwszym uruchomieniu
            Console.Write("Podaj Twój login: ");
            string login = Console.ReadLine();

            Console.Write("Podaj Twój ApiKey: ");
            string apiKey = Console.ReadLine();

            Console.Write("Podaj ID poziomu (np. 1): ");
            int levelId = int.Parse(Console.ReadLine());

            Console.Write("Podaj zdobyte punkty: ");
            int points = int.Parse(Console.ReadLine());

            Console.Write("Podaj Twoje ID użytkownika (np. 1): ");
            int userId = int.Parse(Console.ReadLine());

            // 1. Przygotowujemy obiekt z danymi (musi mieć taki sam układ jak ScoreSubmitDto w API)
            var scorePayload = new
            {
                UserId = userId,
                GameLevelId = levelId,
                Points = points
            };

            // 2. Tworzymy obiekt żądania POST
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);

            // 3. DODAJEMY NAGŁÓWKI AUTORYZACYJNE (To tu dzieje się magia Zadania 3!)
            request.Headers.Add("X-Player-Login", login);
            request.Headers.Add("X-Api-Key", apiKey);

            // 4. Dodajemy ciało żądania (nasz wynik przerobiony na format JSON)
            request.Content = JsonContent.Create(scorePayload);

            Console.WriteLine("\nWysyłanie wyniku do serwera...");

            try
            {
                // 5. Wykonujemy rzeczywiste połączenie sieciowe
                HttpResponseMessage response = await client.SendAsync(request);

                // 6. Odczytujemy odpowiedź serwera
                if (response.IsSuccessStatusCode)
                {
                    string successMessage = await response.Content.ReadAsStringAsync();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\nSukces! Serwer odpowiedział: {successMessage}");
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nBłąd serwera (Status {response.StatusCode}): {errorMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nNie udało się nawiązać połączenia z serwerem: {ex.Message}");
            }

            Console.ResetColor();
            Console.WriteLine("\nNaciśnij dowolny klawisz, aby zamknąć...");
            Console.ReadKey();
        }
    }
}