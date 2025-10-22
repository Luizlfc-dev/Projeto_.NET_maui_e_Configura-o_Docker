using CadastroAlimentos.Api.Data;
using CadastroAlimentos.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CadastroAlimentos.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // A URL será /api/alimentos
    public class AlimentosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AlimentosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/alimentos
        [HttpGet]
        public async Task<IActionResult> GetAlimentos()
        {
            var alimentos = await _context.Alimentos.ToListAsync();
            return Ok(alimentos);
        }

        // POST: /api/alimentos
        [HttpPost]
        public async Task<IActionResult> CreateAlimento([FromBody] Alimento alimento)
        {
            if (alimento == null)
            {
                return BadRequest();
            }

            _context.Alimentos.Add(alimento);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlimentos), new { id = alimento.Id }, alimento);
        }
    }
}