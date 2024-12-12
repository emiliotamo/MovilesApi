using Microsoft.AspNetCore.Mvc;
using Moviles;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace movil.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class miapiController : ControllerBase
    {
        private readonly MovilDbContext _context;
        private readonly ILogger<miapiController> _logger;

        public miapiController(MovilDbContext context, ILogger<miapiController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet("moviles")]
        public async Task<IActionResult> GetMoviles()
        {
            _logger.LogInformation("Obteniendo la lista de móviles.");
            var moviles = await _context.Moviles.ToListAsync();
            return Ok(moviles);
        }

        [HttpPost]
        public async Task<ActionResult<Movil>> Post([FromBody] Movil nuevoMovil)
        {
            if (nuevoMovil == null || string.IsNullOrWhiteSpace(nuevoMovil.Marca))
            {
                return BadRequest(new { mensaje = "Los datos del móvil no son válidos." });
            }

            if (nuevoMovil.Capacidad <= 0)
            {
                nuevoMovil.Capacidad = 64; // Valor predeterminado
            }

            if (nuevoMovil.Precio <= 0)
            {
                nuevoMovil.Precio = 499.99m; // Valor predeterminado
            }

            _context.Moviles.Add(nuevoMovil);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMoviles), new { marca = nuevoMovil.Marca }, nuevoMovil);
        }

        [HttpPut("moviles/{marcaActual}")]
        public async Task<IActionResult> ActualizarMarca(string marcaActual, [FromBody] string nuevaMarca)
        {
            if (string.IsNullOrWhiteSpace(nuevaMarca))
            {
                return BadRequest(new { mensaje = "La nueva marca no puede estar vacía." });
            }

            var movil = await _context.Moviles
                .FirstOrDefaultAsync(m => m.Marca.Equals(marcaActual, StringComparison.OrdinalIgnoreCase));

            if (movil == null)
            {
                return NotFound(new { mensaje = $"Móvil con marca '{marcaActual}' no encontrado." });
            }

            movil.Marca = nuevaMarca;
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Marca actualizada exitosamente.", movil });
        }

        [HttpDelete("moviles/{marca}")]
        public async Task<IActionResult> EliminarMovil(string marca)
        {
            var movil = await _context.Moviles
                .FirstOrDefaultAsync(m => m.Marca.Equals(marca, StringComparison.OrdinalIgnoreCase));

            if (movil == null)
            {
                return NotFound(new { mensaje = $"Móvil con marca '{marca}' no encontrado." });
            }

            _context.Moviles.Remove(movil);
            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Móvil eliminado exitosamente.", movil });
        }
    }
}