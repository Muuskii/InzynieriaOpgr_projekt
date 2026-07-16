using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamowoAPI.Data;
using SteamowoAPI.DTOs;
using SteamowoAPI.Models;

namespace SteamowoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private static readonly Regex WzorzecEmail = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var bledy = WalidujRejestracje(dto);
            if (bledy.Count > 0)
                return BadRequest(string.Join(" ", bledy));

            bool zajety = await _context.Users.AnyAsync(u => u.Login == dto.Login || u.Email == dto.Email);
            if (zajety)
                return Conflict("Uzytkownik o takim loginie lub emailu juz istnieje.");

            var user = new User
            {
                Login = dto.Login.Trim(),
                Email = dto.Email.Trim(),
                PasswordHash = HashujHaslo(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new LoginResponseDto(user.Id, user.Login, user.SaldoPortfela, user.IsAdmin));
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == dto.Login);

            if (user == null || user.PasswordHash != HashujHaslo(dto.Password))
                return Unauthorized("Nieprawidlowy login lub haslo.");

            return Ok(new LoginResponseDto(user.Id, user.Login, user.SaldoPortfela, user.IsAdmin));
        }

        // liczy hash SHA-256 z hasla i zwraca go jako ciag znakow hex (64 znaki)
        private static string HashujHaslo(string haslo)
        {
            byte[] bajtyHasla = Encoding.UTF8.GetBytes(haslo);
            byte[] hash = SHA256.HashData(bajtyHasla);

            var sb = new StringBuilder();
            foreach (byte b in hash)
                sb.Append(b.ToString("x2")); // kazdy bajt jako 2 znaki hex, male litery

            return sb.ToString();
        }

        // zwraca liste czytelnych komunikatow o bledach walidacji (pusta lista = wszystko ok)
        private static List<string> WalidujRejestracje(RegisterDto dto)
        {
            var bledy = new List<string>();

            if (string.IsNullOrWhiteSpace(dto.Login) || dto.Login.Trim().Length < 3)
                bledy.Add("Login musi miec co najmniej 3 znaki.");
            else if (dto.Login.Trim().Length > 30)
                bledy.Add("Login moze miec maksymalnie 30 znakow.");

            if (string.IsNullOrWhiteSpace(dto.Email) || !WzorzecEmail.IsMatch(dto.Email.Trim()))
                bledy.Add("Podaj poprawny adres email.");

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 6)
                bledy.Add("Haslo musi miec co najmniej 6 znakow.");
            else if (!dto.Password.Any(char.IsDigit))
                bledy.Add("Haslo musi zawierac co najmniej jedna cyfre.");

            return bledy;
        }
    }
}
