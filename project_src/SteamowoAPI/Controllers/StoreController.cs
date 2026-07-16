using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamowoAPI.Data;
using SteamowoAPI.DTOs;
using SteamowoAPI.Models;

namespace SteamowoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StoreController(AppDbContext context)
        {
            _context = context;
        }

        // POST api/store/purchase
        [HttpPost("purchase")]
        public async Task<IActionResult> Purchase(PurchaseRequestDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return NotFound("Nie znaleziono uzytkownika.");

            var game = await _context.Games.FindAsync(dto.GameId);
            if (game == null) return NotFound("Nie znaleziono gry.");

            bool juzPosiada = await _context.UserGames
                .AnyAsync(ug => ug.UserId == dto.UserId && ug.GameId == dto.GameId);
            if (juzPosiada)
                return Conflict("Ta gra jest juz w Twojej bibliotece.");

            if (user.SaldoPortfela < game.Cena)
                return BadRequest("Niewystarczajace srodki na portfelu. Doladuj konto.");

            user.SaldoPortfela -= game.Cena;

            var wpis = new UserGame
            {
                UserId = user.Id,
                GameId = game.Id,
                CenaZakupu = game.Cena
            };

            _context.UserGames.Add(wpis);

            _context.Transactions.Add(new Transaction
            {
                UserId = user.Id,
                GameId = game.Id,
                Typ = TypTransakcji.Zakup,
                Kwota = -game.Cena,
                Opis = $"Zakup gry: {game.Tytul}"
            });

            await _context.SaveChangesAsync();

            return Ok(new PurchaseResponseDto($"Zakupiono {game.Tytul}.", user.SaldoPortfela));
        }

        // POST api/store/topup  - doladowanie portfela (uproszczone, bez prawdziwych platnosci)
        [HttpPost("topup")]
        public async Task<IActionResult> TopUp(TopUpDto dto)
        {
            if (dto.Kwota <= 0) return BadRequest("Kwota musi byc dodatnia.");

            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return NotFound("Nie znaleziono uzytkownika.");

            user.SaldoPortfela += dto.Kwota;

            _context.Transactions.Add(new Transaction
            {
                UserId = user.Id,
                GameId = null,
                Typ = TypTransakcji.Doladowanie,
                Kwota = dto.Kwota,
                Opis = "Doladowanie portfela"
            });

            await _context.SaveChangesAsync();

            return Ok(new { NoweSaldo = user.SaldoPortfela });
        }

        // POST api/store/refund  - zwrot gry (usuwa z biblioteki, oddaje pieniadze)
        [HttpPost("refund")]
        public async Task<IActionResult> Refund(PurchaseRequestDto dto)
        {
            var user = await _context.Users.FindAsync(dto.UserId);
            if (user == null) return NotFound("Nie znaleziono uzytkownika.");

            var wpis = await _context.UserGames
                .Include(ug => ug.Game)
                .FirstOrDefaultAsync(ug => ug.UserId == dto.UserId && ug.GameId == dto.GameId);

            if (wpis == null)
                return NotFound("Ta gra nie znajduje sie w Twojej bibliotece.");

            // prosta zasada studencka: zwrot mozliwy tylko jesli grano mniej niz 2h
            if (wpis.CzasGry >= 2)
                return BadRequest("Nie mozna zwrocic gry, w ktora grano juz 2 godziny lub wiecej.");

            user.SaldoPortfela += wpis.CenaZakupu;

            _context.Transactions.Add(new Transaction
            {
                UserId = user.Id,
                GameId = wpis.GameId,
                Typ = TypTransakcji.Zwrot,
                Kwota = wpis.CenaZakupu,
                Opis = $"Zwrot gry: {wpis.Game!.Tytul}"
            });

            _context.UserGames.Remove(wpis);
            await _context.SaveChangesAsync();

            return Ok(new PurchaseResponseDto($"Zwrocono {wpis.Game!.Tytul}.", user.SaldoPortfela));
        }
    }
}
