using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SteamowoAPI.Data;

namespace SteamowoAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TransactionsController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/transactions/5  -> historia transakcji uzytkownika, od najnowszych
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetHistoria(int userId)
        {
            var historia = await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.Data)
                .Select(t => new
                {
                    t.Id,
                    Typ = t.Typ.ToString(),
                    t.Kwota,
                    t.Opis,
                    t.Data
                })
                .ToListAsync();

            return Ok(historia);
        }
    }
}
