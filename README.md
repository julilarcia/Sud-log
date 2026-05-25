Markdown
# GameTracker

## Opis projektu

GameTracker to aplikacja internetowa napisana w technologii **ASP.NET Core MVC** z wykorzystaniem **REST API**, **Entity Framework Core** oraz bazy danych **SQLite**.

System służy do gromadzenia, zarządzania i analizowania wyników graczy. Aplikacja umożliwia logowanie użytkowników, zapisywanie wyników rozgrywek, zarządzanie poziomami gry, osiągnięciami oraz prezentowanie rankingów i profili graczy.

---

## Autorzy

- **Kaja Dragun** : kdragun@student.agh.edu.pl

- **Julia Dorobis** : jdorobis@student.agh.edu.pl
---

## Technologie

Projekt wykorzystuje:

- ASP.NET Core MVC
- REST API
- Entity Framework Core
- SQLite
- Razor Views
- Sesje ASP.NET Core
- C#
- Bootstrap
- Konsolową aplikację kliencką z `HttpClient`

---

## Główne funkcjonalności

### Aplikacja webowa

Aplikacja webowa umożliwia:

- logowanie użytkowników,
- wylogowanie użytkownika,
- zapamiętywanie zalogowanego użytkownika przy pomocy mechanizmu sesji,
- zabezpieczenie wybranych widoków przed dostępem niezalogowanych użytkowników,
- zarządzanie poziomami gry,
- zarządzanie osiągnięciami,
- zarządzanie użytkownikami przez administratora,
- przeglądanie wyników,
- wyświetlanie rankingu graczy,
- wyświetlanie profilu gracza z historią gier i odblokowanymi osiągnięciami.

### REST API

Aplikacja udostępnia REST API umożliwiające:

- pobieranie wyników,
- dodawanie nowych wyników,
- modyfikowanie danych,
- usuwanie danych,
- autoryzację żądań na podstawie loginu użytkownika oraz klucza API.

Autoryzacja REST API odbywa się przez przesyłanie danych w nagłówkach HTTP:

```http
X-Player-Login: login_uzytkownika
X-Api-Key: klucz_api
```

### Aplikacja konsolowa

Do projektu dołączona jest osobna aplikacja konsolowa, która demonstruje działanie REST API. Program symuluje klienta gry, wysyłając wynik gracza do serwera za pomocą żądania HTTP.

---

## Struktura bazy danych

Aplikacja korzysta z bazy danych SQLite oraz Entity Framework Core. Dane są obsługiwane przez modele MVC, bez używania natywnych zapytań SQL w kodzie aplikacji.

Główne tabele projektu:

### `Users`

Tabela użytkowników systemu.

Przechowuje:

- login,
- skrót hasła,
- rolę użytkownika,
- klucz API.

Tabela ta służy do obsługi logowania i autoryzacji.

### `GameLevels`

Tabela poziomów lub trybów gry.

Przechowuje:

- nazwę poziomu,
- mnożnik trudności.

### `Scores`

Tabela wyników graczy.

Przechowuje:

- liczbę punktów,
- datę uzyskania wyniku,
- użytkownika, który uzyskał wynik,
- poziom gry, na którym uzyskano wynik.

Relacje:

- `Scores` -> `Users`
- `Scores` -> `GameLevels`

### `Achievements`

Tabela osiągnięć możliwych do zdobycia.

Przechowuje:

- nazwę osiągnięcia,
- opis osiągnięcia.

### `UserAchievements`

Tabela łącząca użytkowników z odblokowanymi osiągnięciami.

Przechowuje:

- użytkownika,
- osiągnięcie,
- datę odblokowania osiągnięcia.

Relacje:

- `UserAchievements` -> `Users`
- `UserAchievements` -> `Achievements`

---

## Role użytkowników

System obsługuje role użytkowników.

### Administrator

Administrator może:

- dodawać nowych użytkowników,
- przeglądać istniejących użytkowników,
- zarządzać poziomami gry,
- zarządzać osiągnięciami,
- przeglądać dane systemowe.

### Gracz

Gracz może:

- zalogować się do aplikacji,
- przeglądać swoje wyniki,
- sprawdzić swój profil,
- zobaczyć historię gier,
- zobaczyć odblokowane osiągnięcia,
- korzystać z funkcjonalności publicznych, takich jak ranking.

---

## Uruchomienie projektu

### Wymagania

Do uruchomienia projektu wymagane są:

- .NET 8 SDK
- Git
- Visual Studio Code lub Visual Studio
- SQLite obsługiwany przez Entity Framework Core

---

### 1. Sklonowanie repozytorium

```bash
git clone https://github.com/julilarcia/GameTracker.git
cd GameTracker
```

---

### 2. Uruchomienie aplikacji serwerowej

Przejdź do folderu aplikacji MVC:

```bash
cd code
```

Przywróć zależności:

```bash
dotnet restore
```

Uruchom aplikację:

```bash
dotnet run
```

Po uruchomieniu aplikacja będzie dostępna pod adresem wyświetlonym w terminalu, na przykład:

```text
http://localhost:5264
```

lub:

```text
https://localhost:7264
```

---

## Baza danych

Aplikacja korzysta z pliku bazy danych SQLite:

```text
GameTrackerData.db
```

Jeżeli baza danych nie istnieje, zostanie utworzona podczas uruchamiania aplikacji lub po wykonaniu migracji Entity Framework Core.

W przypadku konieczności ręcznego utworzenia lub aktualizacji bazy danych można użyć polecenia:

```bash
dotnet ef database update
```

---

## Domyślne konto administratora

Przy pierwszym uruchomieniu aplikacji powinno zostać utworzone konto administratora.

Domyślne dane logowania:

```text
Login: admin
Hasło: admin123
ApiKey: API-KEY-123
```

Konto administratora służy do zarządzania użytkownikami, poziomami gry oraz osiągnięciami.

---

## Testowanie REST API

Do przetestowania REST API służy osobna aplikacja konsolowa znajdująca się w folderze:

```text
GameTrackerClient
```

### 1. Uruchomienie serwera

Najpierw należy uruchomić główną aplikację MVC:

```bash
cd code
dotnet run
```

Należy zapamiętać adres oraz port serwera wyświetlony w terminalu.

---

### 2. Uruchomienie aplikacji konsolowej

W nowym terminalu przejdź do folderu klienta:

```bash
cd GameTrackerClient
```

Uruchom aplikację:

```bash
dotnet run
```

Program poprosi o dane potrzebne do wysłania wyniku do API.

Przykładowe dane testowe:

```text
Login: admin
ApiKey: API-KEY-123
Id poziomu: 1
Punkty: 500
```

Po poprawnym wysłaniu żądania aplikacja konsolowa powinna wyświetlić komunikat potwierdzający zapis wyniku.

---

## Przykład żądania REST API

Przykładowe żądanie dodania wyniku:

```http
POST /api/ScoresApi
Content-Type: application/json
X-Player-Login: admin
X-Api-Key: API-KEY-123
```

Przykładowe ciało żądania:

```json
{
  "gameLevelId": 1,
  "points": 500
}
```

---

## Najważniejsze widoki aplikacji

Aplikacja zawiera następujące widoki:

- strona główna,
- logowanie,
- panel użytkownika,
- profil gracza,
- historia wyników,
- ranking graczy,
- panel zarządzania użytkownikami,
- panel zarządzania poziomami gry,
- panel zarządzania osiągnięciami.

Wszystkie główne funkcjonalności są dostępne z poziomu menu aplikacji, bez konieczności ręcznego wpisywania adresów URL.

---

## Leaderboard

Aplikacja zawiera ranking graczy, który stanowi dodatkową funkcjonalność analityczną wykraczającą poza proste wyświetlanie zawartości tabel.

Ranking może prezentować między innymi:

- login gracza,
- sumę zdobytych punktów,
- liczbę rozegranych gier,
- najlepszy wynik gracza.

Dane są wyliczane na podstawie zapisanych wyników w tabeli `Scores`.

---

## Profil gracza

Profil gracza prezentuje dane aktualnie zalogowanego użytkownika.

Widok profilu zawiera:

- login użytkownika,
- rolę użytkownika,
- liczbę rozegranych gier,
- sumę zdobytych punktów,
- historię wyników,
- listę odblokowanych osiągnięć.

---

## Zgodność z wymaganiami laboratorium

| Wymaganie | Realizacja w projekcie |
| --- | --- |
| Aplikacja ASP.NET Core MVC | Projekt serwerowy w folderze `code` |
| Minimum 4 tabele bazy danych | `GameLevels`, `Scores`, `Achievements`, `UserAchievements` |
| Obsługa bazy SQLite | `GameTrackerData.db`, Entity Framework Core |
| Dostęp do bazy przez modele MVC | Folder `Models`, `GameTrackerContext` |
| Zarządzanie danymi przez interfejs webowy | Kontrolery i widoki CRUD |
| Dane startowe przy pierwszym uruchomieniu | Domyślny administrator oraz dane początkowe |
| Logowanie i sesja | `AccountController`, `HttpContext.Session` |
| Hasła w formie skrótu | `PasswordHash`, `PasswordHelper` |
| Tylko administrator zarządza użytkownikami | `UsersController` |
| Dodatkowe zestawienia | `Leaderboard`, profil gracza |
| REST API | `ScoresApiController` |
| Autoryzacja REST API tokenem | Nagłówki `X-Player-Login` i `X-Api-Key` |
| Program konsolowy do API | Projekt `GameTrackerClient` |
| Dokumentacja | Plik `README.md` |

---

## Struktura projektu

```text
GameTracker
├── code
│   ├── Api
│   │   └── ScoresApiController.cs
│   ├── Controllers
│   │   ├── AccountController.cs
│   │   ├── AchievementsController.cs
│   │   ├── GameLevelsController.cs
│   │   ├── HomeController.cs
│   │   ├── LeaderboardController.cs
│   │   ├── ScoresController.cs
│   │   └── UsersController.cs
│   ├── Helpers
│   │   └── PasswordHelper.cs
│   ├── Models
│   │   ├── Achievement.cs
│   │   ├── GameLevel.cs
│   │   ├── GameTrackerContext.cs
│   │   ├── Score.cs
│   │   ├── User.cs
│   │   └── UserAchievement.cs
│   ├── ViewModels
│   │   ├── LeaderboardViewModels.cs
│   │   ├── ProfileViewModels.cs
│   │   └── UserViewModels.cs
│   ├── Views
│   │   ├── Account
│   │   ├── Achievements
│   │   ├── GameLevels
│   │   ├── Home
│   │   ├── Leaderboard
│   │   ├── Scores
│   │   ├── Shared
│   │   └── Users
│   ├── Program.cs
│   └── GameTracker.csproj
├── GameTrackerClient
│   ├── Program.cs
│   └── GameTrackerClient.csproj
├── README.md
└── .gitignore
```

---

## Licencja

Projekt został przygotowany na potrzeby zajęć laboratoryjnych z przedmiotu Programowanie zaawansowane 2 w ramach toku studiów Informatyka i Systemy Inteligentne na Akademii Górniczo-Hutniczej.