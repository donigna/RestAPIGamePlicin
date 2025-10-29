using Microsoft.AspNetCore.Mvc;
using Backend_Plicin.Data;
using Backend_Plicin.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_Plicin.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PlayerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Players.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            player.CreatedAt = DateTime.UtcNow;
            player.UpdatedAt = DateTime.UtcNow;

            _context.Players.Add(player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayers), new { id = player.Id }, player);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlayer(int id,[FromBody] Player updatedPlayer)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            if (id != updatedPlayer.Id)
                return BadRequest("ID di URL dan body tidak sama.");

            player.Uang = updatedPlayer.Uang;
            player.Level = updatedPlayer.Level;
            player.GameCount = updatedPlayer.GameCount;
            player.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("restart/{id}")]
        public async Task<IActionResult> RestartGame(int id)
        {
            var player = await _context.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            player.GameCount += 1;
            player.Uang = 0;
            player.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return Ok(player);
        }
    }
}
