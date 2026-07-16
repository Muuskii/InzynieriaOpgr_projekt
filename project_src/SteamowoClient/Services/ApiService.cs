using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SteamowoClient.Models;

namespace SteamowoClient.Services
{
    // Prosty wrapper na HttpClient - cala komunikacja z naszym API leci przez ta klase
    public class ApiService
    {
        private readonly HttpClient _http;
        private const string BaseUrl = "http://localhost:5205/api";

        public ApiService()
        {
            _http = new HttpClient();
        }

        private async Task<T?> PostAsync<T>(string endpoint, object dane)
        {
            var json = JsonConvert.SerializeObject(dane);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var odpowiedz = await _http.PostAsync($"{BaseUrl}/{endpoint}", content);
            var tresc = await odpowiedz.Content.ReadAsStringAsync();

            if (!odpowiedz.IsSuccessStatusCode)
                throw new ApplicationException(string.IsNullOrWhiteSpace(tresc) ? odpowiedz.ReasonPhrase : tresc);

            return JsonConvert.DeserializeObject<T>(tresc);
        }

        private async Task<T?> PutAsync<T>(string endpoint, object dane)
        {
            var json = JsonConvert.SerializeObject(dane);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var odpowiedz = await _http.PutAsync($"{BaseUrl}/{endpoint}", content);
            var tresc = await odpowiedz.Content.ReadAsStringAsync();

            if (!odpowiedz.IsSuccessStatusCode)
                throw new ApplicationException(string.IsNullOrWhiteSpace(tresc) ? odpowiedz.ReasonPhrase : tresc);

            return JsonConvert.DeserializeObject<T>(tresc);
        }

        private async Task DeleteAsync(string endpoint)
        {
            var odpowiedz = await _http.DeleteAsync($"{BaseUrl}/{endpoint}");
            if (!odpowiedz.IsSuccessStatusCode)
            {
                var tresc = await odpowiedz.Content.ReadAsStringAsync();
                throw new ApplicationException(string.IsNullOrWhiteSpace(tresc) ? odpowiedz.ReasonPhrase : tresc);
            }
        }

        private async Task<T?> GetAsync<T>(string endpoint)
        {
            var odpowiedz = await _http.GetAsync($"{BaseUrl}/{endpoint}");
            var tresc = await odpowiedz.Content.ReadAsStringAsync();

            if (!odpowiedz.IsSuccessStatusCode)
                throw new ApplicationException(string.IsNullOrWhiteSpace(tresc) ? odpowiedz.ReasonPhrase : tresc);

            return JsonConvert.DeserializeObject<T>(tresc);
        }

        public Task<LoginResponse?> Login(string login, string haslo) =>
            PostAsync<LoginResponse>("auth/login", new { Login = login, Password = haslo });

        public Task<LoginResponse?> Register(string login, string email, string haslo) =>
            PostAsync<LoginResponse>("auth/register", new { Login = login, Email = email, Password = haslo });

        public Task<List<GameDto>?> PobierzGry() =>
            GetAsync<List<GameDto>>("games");

        public Task<List<LibraryItemDto>?> PobierzBiblioteke(int userId) =>
            GetAsync<List<LibraryItemDto>>($"library/{userId}");

        public Task<List<TransactionDto>?> PobierzHistorie(int userId) =>
            GetAsync<List<TransactionDto>>($"transactions/{userId}");

        public async Task<decimal> KupGre(int userId, int gameId)
        {
            var wynik = await PostAsync<PurchaseResponse>("store/purchase", new { UserId = userId, GameId = gameId });
            return wynik!.NoweSaldo;
        }

        public async Task<decimal> DoladujPortfel(int userId, decimal kwota)
        {
            var wynik = await PostAsync<TopUpResponse>("store/topup", new { UserId = userId, Kwota = kwota });
            return wynik!.NoweSaldo;
        }

        public async Task<decimal> ZwrocGre(int userId, int gameId)
        {
            var wynik = await PostAsync<PurchaseResponse>("store/refund", new { UserId = userId, GameId = gameId });
            return wynik!.NoweSaldo;
        }

        // --- Panel admina: CRUD gier ---

        public Task<GameDto?> DodajGre(int requestUserId, GameDto gra) =>
            PostAsync<GameDto>("games", new
            {
                RequestUserId = requestUserId,
                gra.Tytul,
                gra.Opis,
                gra.Producent,
                gra.Gatunek,
                gra.Cena,
                gra.UrlOkladki,
                gra.DataWydania
            });

        public Task<GameDto?> EdytujGre(int requestUserId, GameDto gra) =>
            PutAsync<GameDto>($"games/{gra.Id}", new
            {
                RequestUserId = requestUserId,
                gra.Tytul,
                gra.Opis,
                gra.Producent,
                gra.Gatunek,
                gra.Cena,
                gra.UrlOkladki,
                gra.DataWydania
            });

        public Task UsunGre(int requestUserId, int gameId) =>
            DeleteAsync($"games/{gameId}?requestUserId={requestUserId}");
    }
}
