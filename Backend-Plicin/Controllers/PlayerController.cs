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
        private readonly AppDbContext _db;

        public PlayerController(AppDbContext db) => _db = db;


        // Mendapatkan seluruh pemain
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _db.Players.ToListAsync();
        }

        // Mendapatkan pemain dengan urutan uang tertinggi
        [HttpGet("top")]
        public async Task<ActionResult<IEnumerable<Player>>> GetTopPlayers()
        {
            var topPlayers = await _db.Players
                .OrderByDescending(p => p.Uang)
                .ToListAsync();
            return topPlayers;
        }

        // Membuat pemain baru
        [HttpPost]
        public async Task<ActionResult<Player>> CreatePlayer(Player player)
        {
            player.CreatedAt = DateTime.UtcNow;
            player.UpdatedAt = DateTime.UtcNow;

            _db.Players.Add(player);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayers), new { id = player.Id }, player);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdatePlayer(int id,[FromBody] Player updatedPlayer)
        {
            var player = await _db.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            if (id != updatedPlayer.Id)
                return BadRequest("ID di URL dan body tidak sama.");

            player.Uang = updatedPlayer.Uang;
            player.Level = updatedPlayer.Level;
            player.GameCount = updatedPlayer.GameCount;
            player.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("restart/{id:int}")]
        public async Task<IActionResult> RestartGame(int id)
        {
            var player = await _db.Players.FindAsync(id);
            if (player == null)
                return NotFound();

            player.GameCount += 1;
            player.Uang = 0;
            player.UpdatedAt = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return Ok(player);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _db.Players.FindAsync(id);
            if (player == null)
                return NotFound();
            _db.Players.Remove(player);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
