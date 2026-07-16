# Steamowo - projekt studencki (C#, WPF + Web API + SQL Server)

## Struktura
- **SteamowoAPI** – ASP.NET Core Web API (kontrolery, EF Core, SQL Server)
- **SteamowoClient** – aplikacja WPF (logowanie, sklep, biblioteka gier)

## Wymagania
- Visual Studio 2022 (z workloadami: ASP.NET, .NET desktop development)
- .NET 8 SDK
- SQL Server Management Studio 22 + działający lokalny SQL Server (np. `localhost\SQLEXPRESS`)

## Konfiguracja bazy danych
1. Otwórz `SteamowoAPI/appsettings.json` i popraw connection string, jeśli Twoja instancja
   SQL Server nazywa się inaczej niż `localhost\SQLEXPRESS` (sprawdź nazwę serwera w SSMS 22
   przy logowaniu – to jest Twój `Server=`).
2. Migracje EF Core wygeneruj i zastosuj z poziomu Package Manager Console
   (Visual Studio -> Tools -> NuGet Package Manager -> Package Manager Console),
   z projektem domyślnym `SteamowoAPI`:
   ```
   Add-Migration InitialCreate
   Update-Database
   ```
   Baza `SteamowoDb` zostanie utworzona automatycznie (Program.cs wywołuje `db.Database.Migrate()`
   przy starcie, więc wystarczy nawet samo uruchomienie API po dodaniu migracji).
3. W SSMS 22 możesz potem podejrzeć tabele: `Users`, `Games`, `UserGames`.

## Uruchomienie
1. Ustaw **SteamowoAPI** jako projekt startowy i uruchom (F5) – API wystartuje pod
   `http://localhost:5205`, zobaczysz Swaggera pod `http://localhost:5205/swagger`.
2. Następnie uruchom **SteamowoClient** (możesz ustawić oba projekty jako startowe:
   PPM na solucji -> Properties -> Multiple startup projects -> oba "Start").
3. W oknie logowania zarejestruj nowe konto (login, email, hasło), po czym zalogujesz się
   automatycznie do głównego okna.

## Funkcje
- Rejestracja / logowanie (hasła hashowane przez BCrypt)
- Sklep z listą gier (4 przykładowe gry dodane jako dane startowe)
- Zakup gry – odejmuje środki z wirtualnego portfela, dodaje grę do biblioteki
- Doładowanie portfela (uproszczone – przycisk "+50 zł", bez prawdziwych płatności)
- Biblioteka gier użytkownika z datą zakupu i czasem gry

## Możliwe rozszerzenia (na wyższą ocenę)
- Prawdziwe uwierzytelnianie JWT zamiast przekazywania UserId w żądaniach
- Wyszukiwarka / filtrowanie gier w sklepie
- Opinie i oceny gier
- Lista znajomych
- Okładki gier (obecnie tylko pole `UrlOkladki`, można podpiąć obrazki)
