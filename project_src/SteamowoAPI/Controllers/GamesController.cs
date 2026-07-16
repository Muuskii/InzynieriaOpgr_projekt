using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamowoAPI.Data;
using SteamowoAPI.DTOs;
using SteamowoAPI.Models;

namespace SteamowoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GamesController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/games  -> wszystkie gry w sklepie
        [HttpGet]
        public async Task<ActionResult<List<GameDto>>> GetAll()
        {
            var gry = await _context.Games
                .Select(g => new GameDto(g.Id, g.Tytul, g.Opis, g.Producent, g.Gatunek, g.Cena, g.UrlOkladki, g.DataWydania))
                .ToListAsync();

            return Ok(gry);
        }

        // GET api/games/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameDto>> GetOne(int id)
        {
            var g = await _context.Games.FindAsync(id);
            if (g == null) return NotFound();

            return Ok(new GameDto(g.Id, g.Tytul, g.Opis, g.Producent, g.Gatunek, g.Cena, g.UrlOkladki, g.DataWydania));
        }

        private async Task<bool> CzyAdmin(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            return user != null && user.IsAdmin;
        }

        // POST api/games  - dodanie nowej gry (tylko admin)
        [HttpPost]
        public async Task<IActionResult> Dodaj(UpsertGameDto dto)
        {
            if (!await CzyAdmin(dto.RequestUserId))
                return Forbid();

            var gra = new Game
            {
                Tytul = dto.Tytul,
                Opis = dto.Opis,
                Producent = dto.Producent,
                Gatunek = dto.Gatunek,
                Cena = dto.Cena,
                UrlOkladki = dto.UrlOkladki,
                DataWydania = dto.DataWydania
            };

            _context.Games.Add(gra);
            await _context.SaveChangesAsync();

            return Ok(new GameDto(gra.Id, gra.Tytul, gra.Opis, gra.Producent, gra.Gatunek, gra.Cena, gra.UrlOkladki, gra.DataWydania));
        }

        // PUT api/games/5  - edycja gry (tylko admin)
        [HttpPut("{id}")]
        public async Task<IActionResult> Edytuj(int id, UpsertGameDto dto)
        {
            if (!await CzyAdmin(dto.RequestUserId))
                return Forbid();

            var gra = await _context.Games.FindAsync(id);
            if (gra == null) return NotFound("Nie znaleziono gry.");

            gra.Tytul = dto.Tytul;
            gra.Opis = dto.Opis;
            gra.Producent = dto.Producent;
            gra.Gatunek = dto.Gatunek;
            gra.Cena = dto.Cena;
            gra.UrlOkladki = dto.UrlOkladki;
            gra.DataWydania = dto.DataWydania;

            await _context.SaveChangesAsync();

            return Ok(new GameDto(gra.Id, gra.Tytul, gra.Opis, gra.Producent, gra.Gatunek, gra.Cena, gra.UrlOkladki, gra.DataWydania));
        }

        // DELETE api/games/5?requestUserId=1  - usuniecie gry (tylko admin)
        [HttpDelete("{id}")]
        public async Task<IActionResult> Usun(int id, [FromQuery] int requestUserId)
        {
            if (!await CzyAdmin(requestUserId))
                return Forbid();

            var gra = await _context.Games.FindAsync(id);
            if (gra == null) return NotFound("Nie znaleziono gry.");

            _context.Games.Remove(gra);
            await _context.SaveChangesAsync();

            return Ok(new { Wiadomosc = $"Usunieto {gra.Tytul}." });
        }
    }
}
