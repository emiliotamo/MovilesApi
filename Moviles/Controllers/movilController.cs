using Microsoft.AspNetCore.Mvc;
using Moviles;
using System;
using System.Reflection;

namespace movil.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class miapiController : ControllerBase
    {
        private static readonly List<Movil> Lista = new List<Movil>
        {
            new Movil("Samsung", 799.99m, 128),
            new Movil("Apple", 999.99m, 256),
            new Movil("Xiaomi", 299.99m, 128),
            new Movil("OnePlus", 499.99m, 128),
            new Movil("Google", 899.99m, 256)
        };

        private readonly ILogger<miapiController> _logger;

        public miapiController(ILogger<miapiController> logger)
        {
            _logger = logger;
        }

        [HttpGet("moviles")]
        public IActionResult GetMoviles()
        {
            _logger.LogInformation("Obteniendo la lista de móviles.");
            return Ok(Lista);
        }

        [HttpPost]
        public ActionResult<Movil> Post([FromBody] Movil nuevoMovil)
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

            Lista.Add(nuevoMovil);

            return CreatedAtAction(nameof(GetMoviles), new { marca = nuevoMovil.Marca }, nuevoMovil);
        }

        [HttpPut("moviles/{marcaActual}")]
        public IActionResult ActualizarMarca(string marcaActual, [FromBody] string nuevaMarca)
        {
            if (string.IsNullOrWhiteSpace(nuevaMarca))
            {
                return BadRequest(new { mensaje = "La nueva marca no puede estar vacía." });
            }

            var movil = Lista.Find(m => m.Marca.Equals(marcaActual, StringComparison.OrdinalIgnoreCase));
            if (movil == null)
            {
                return NotFound(new { mensaje = $"Móvil con marca '{marcaActual}' no encontrado." });
            }

            movil.Marca = nuevaMarca;

            return Ok(new { mensaje = "Marca actualizada exitosamente.", movil });
        }

        [HttpDelete("moviles/{marca}")]
        public IActionResult EliminarMovil(string marca)
        {
            var movil = Lista.Find(m => m.Marca.Equals(marca, StringComparison.OrdinalIgnoreCase));
            if (movil == null)
            {
                return NotFound(new { mensaje = $"Móvil con marca '{marca}' no encontrado." });
            }

            Lista.Remove(movil);

            return Ok(new { mensaje = "Móvil eliminado exitosamente.", movil });
        }
    }
}