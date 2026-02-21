using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GameCharactersAPI.Data;
using GameCharactersAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameCharactersAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharactersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CharactersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/characters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Character>>> GetCharacters()
        {
            return await _context.Characters.ToListAsync();
        }

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Character>> GetCharacterById(int id)
        {
            var character = await _context.Characters.FindAsync(id);
            if (character == null)
                return NotFound();
            return character;
        }

       
        [HttpPost] 
        public async Task<ActionResult<Character>> PostCharacter([FromBody] Character character)
        {
            if (character == null)
                return BadRequest();

            _context.Characters.Add(character);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCharacterById), new { id = character.Id }, character);
        }

        
        [HttpGet("highlevel")]
        public async Task<ActionResult<IEnumerable<Character>>> GetHighLevelCharacters()
        {
            return await _context.Characters
                .Where(c => c.Level >= 50)
                .ToListAsync();
        }
    }
}