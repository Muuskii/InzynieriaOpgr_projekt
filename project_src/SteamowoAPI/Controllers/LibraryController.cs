using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamowoAPI.Data;
using SteamowoAPI.DTOs;

namespace SteamowoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly AppDbContext _context;

        public LibraryController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/library/3  -> biblioteka gier uzytkownika o id=3
        [HttpGet("{userId}")]
        public async Task<ActionResult<List<LibraryItemDto>>> GetLibrary(int userId)
        {
            var biblioteka = await _context.UserGames
                .Where(ug => ug.UserId == userId)
                .Include(ug => ug.Game)
                .Select(ug => new LibraryItemDto(
                    ug.GameId,
                    ug.Game!.Tytul,
                    ug.Game.Gatunek,
                    ug.DataZakupu,
                    ug.CzasGry,
                    ug.Game.UrlOkladki))
                .ToListAsync();

            return Ok(biblioteka);
        }

        // PUT api/library/3/playtime/2?godziny=1.5  -> dodanie czasu gry
        [HttpPut("{userId}/playtime/{gameId}")]
        public async Task<IActionResult> DodajCzasGry(int userId, int gameId, [FromQuery] double godziny)
        {
            var wpis = await _context.UserGames
                .FirstOrDefaultAsync(ug => ug.UserId == userId && ug.GameId == gameId);

            if (wpis == null) return NotFound("Ta gra nie znajduje sie w bibliotece uzytkownika.");

            wpis.CzasGry += godziny;
            await _context.SaveChangesAsync();

            return Ok(new { wpis.CzasGry });
        }
    }
}
