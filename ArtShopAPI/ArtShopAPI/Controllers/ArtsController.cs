using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArtShopAPI.Models;
using Microsoft.AspNetCore.Authorization;

namespace ArtShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtsController : ControllerBase
    {
        private readonly ArtShopContext _context;

        public ArtsController(ArtShopContext context)
        {
            _context = context;
        }

        // GET: api/Arts
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Art>>> GetArts()
        {
            return await _context.Arts.ToListAsync();
        }

        // GET: api/Arts/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Art>> GetArt(int id)
        {
            var art = await _context.Arts.FindAsync(id);

            if (art == null)
            {
                return NotFound();
            }

            return art;
        }
        [Authorize]
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<Art>>> GetUserArt(int userId)
        {
            var uId = User.FindFirst("userId")?.Value;
            if (Int32.Parse(uId) != userId)
            {
                return NotFound(new Response { Status = "Error", Message = "Page not found" }); ;
            }
            var art = await _context.Arts.Where(c => c.UserId == userId).ToListAsync();

            if (art == null)
            {
                return NotFound(new Response { Status = "Error", Message = "Page not found" });
            }

            return art;
        }

        // PUT: api/Arts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArt(int id, Art art)
        {
            var uId = User.FindFirst("userId")?.Value;
            if (Int32.Parse(uId) != art.UserId)
            {
                return NotFound(new Response { Status = "Error", Message = "You don't have access" }); ;
            }
            if (id != art.Id)
            {
                return BadRequest();
            }

            _context.Entry(art).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }
        // POST: api/Arts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Art>> PostArt(Art art)
        {
            var uId = User.FindFirst("userId")?.Value;
            art.UserId = Int32.Parse(uId);
            _context.Arts.Add(art);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArt", new { id = art.Id }, art);
        }

        [Authorize]
        [HttpPost]
        [Route("buyArt/{ArtId}")]
        public async Task<IActionResult> BuyArt(int ArtId)
        {
            var uId = Int32.Parse(User.FindFirst("userId")?.Value);
            var art = await _context.Arts.FirstOrDefaultAsync(c => c.Id == ArtId);
            if(uId == art.UserId) return BadRequest(new Response { Status = "Error", Message = "You already owned this" });
            var buyer = await _context.Users.FirstOrDefaultAsync(c => c.Id == uId);
            if(buyer.Balance < art.Price) return BadRequest(new Response { Status = "Error", Message = "You don't have enough Money" });
            buyer.Balance -= art.Price;
            var owner = await _context.Users.FirstOrDefaultAsync(c => c.Id == art.UserId);
            owner.Balance += art.Price;
            art.UserId = buyer.Id;
            _context.SaveChanges();
            return Ok(owner);
        }

        // DELETE: api/Arts/5
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArt(int id)
        {
            var uId = User.FindFirst("userId")?.Value;
            var art = await _context.Arts.FirstOrDefaultAsync(c => c.Id == id);
            if (Int32.Parse(uId) != art.UserId)
            {
                return NotFound(new Response { Status = "Error", Message = "You don't have access" });
            }
            if (art == null)
            {
                return NotFound(new Response { Status = "Error", Message = "Bad request" });
            }

            _context.Arts.Remove(art);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtExists(int id)
        {
            return _context.Arts.Any(e => e.Id == id);
        }
    }
}
