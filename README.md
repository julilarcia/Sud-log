Markdown
# 🎮 GameTracker

## 👥 Autorzy
* **Osoba A (Lider backendu i zarządzania danymi):** Kaja [Wpisz Nazwisko]
* **Osoba B (Lider autoryzacji i widoków publicznych):** [Wpisz Imię i Nazwisko koleżanki]

---

## 🎯 Cel Aplikacji
GameTracker to kompletny system webowy działający w architekturze Klient-Serwer, służący do gromadzenia i analizy wyników graczy, a także zarządzania trybami trudności i odznakami. Projekt powstał w technologii **ASP.NET Core (MVC + REST API)** oraz **SQLite** (baza danych z obsługą Entity Framework Core). 

Projekt dzieli się na dwa połączone ze sobą moduły:
1. **Aplikacja Serwerowa (Portal Webowy):** Oferuje bezpieczny system logowania oparty na mechanizmie Sesji, publiczne Tabele Wyników oraz chroniony Panel Administratora (dostępny tylko dla ról 'Admin') pozwalający na zarządzanie zasobami (graczami, poziomami gry i osiągnięciami).
2. **Aplikacja Kliencka (Symulator Gry):** Niezależny program konsolowy wysyłający asynchroniczne żądania `HTTP POST` do serwera, symulujący kończenie rozgrywki przez gracza.

---

## 🚀 Instrukcja Uruchomienia (Serwer + Baza Danych)

1. Sklonuj repozytorium na swój komputer.
2. Otwórz folder główny projektu (ten zawierający folder `code`) w terminalu / VS Code.
3. Przejdź do folderu z serwerem:
   ```bash
   cd code
Uruchom aplikację:

Bash
dotnet run
Data Seeding: Przy pierwszym uruchomieniu system sam stworzy plik bazy danych GameTrackerData.db i doda domyślne konto Administratora:

Login: admin

Hasło: admin123

🔌 Instrukcja Testowania REST API (Program Konsolowy)
Zabezpieczone REST API pozwala na automatyczne wysyłanie punktów "z gry" na serwer. Wymaga ono autoryzacji z użyciem nagłówków HTTP (X-Player-Login oraz X-Api-Key).

Aby przetestować wysyłanie danych:

Upewnij się, że główny Serwer (krok wyżej) cały czas działa w tle. Zapisz port, na którym działa serwer (np. http://localhost:5264).

Otwórz nową kartę terminala i przejdź do folderu z aplikacją kliencką:

Bash
cd GameTrackerClient
Uruchom symulator:

Bash
dotnet run
Postępuj zgodnie z instrukcjami na ekranie. Użyj danych startowego administratora, by wysłać pierwszy wynik:

Twój login: admin

Twój ApiKey: API-KEY-123

ID poziomu: 1

Punkty: (wpisz dowolną liczbę)

Program konsolowy powinien wyświetlić na zielono komunikat o sukcesie zwrócony bezpośrednio przez Serwer.


Zapisz plik `README.md` (`Cmd + S`) i to wszystko! Twój projekt jest teraz profesjonalnie opisany, ma zabezpieczone API, zahasowane hasła, ochronę przed zajętymi portami i piękny panel administratora. Jesteście gotowe do commitowania i wysłania tego cudeńka do prowadzącego!