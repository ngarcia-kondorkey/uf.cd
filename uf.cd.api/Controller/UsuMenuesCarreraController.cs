using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uf.cd.db.Model; // Asumiendo que el modelo está en este namespace
using Microsoft.AspNetCore.Http; // Necesario para StatusCodes

namespace uf.cd.api.Controllers // Namespace especificado para el controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuMenuesCarreraController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuMenuesCarreraController> _logger;

        public UsuMenuesCarreraController(ExtranetContext context, IConfiguration configuration, ILogger<UsuMenuesCarreraController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/UsuMenuesCarrera
        // Obtiene la configuración de menú para todas las carreras.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuMenuesCarrera>>> GetUsuMenuesCarreras()
        {
            if (_context.UsuMenuesCarreras == null)
            {
                return NotFound("La entidad 'UsuMenuesCarreras' no está configurada en el DbContext.");
            }
            return await _context.UsuMenuesCarreras.ToListAsync();
        }

        // GET: api/UsuMenuesCarrera/{idCarrera}
        // Busca la configuración de menú para una carrera específica por su clave primaria CcaaIdCarrera (int)
        [HttpGet("{idCarrera}")]
        public async Task<ActionResult<UsuMenuesCarrera>> GetUsuMenuesCarrera(int idCarrera)
        {
            if (_context.UsuMenuesCarreras == null)
            {
                 return NotFound("La entidad 'UsuMenuesCarreras' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var usuMenuesCarrera = await _context.UsuMenuesCarreras.FindAsync(idCarrera);

            if (usuMenuesCarrera == null)
            {
                return NotFound();
            }

            return usuMenuesCarrera;
        }

        // PUT: api/UsuMenuesCarrera/{idCarrera}
        // Para actualizar la configuración de menú para una carrera existente.
        // Nota: Probablemente restringido a roles administrativos.
        [HttpPut("{idCarrera}")]
        public async Task<IActionResult> PutUsuMenuesCarrera(int idCarrera, UsuMenuesCarrera usuMenuesCarrera)
        {
            if (idCarrera != usuMenuesCarrera.CcaaIdCarrera)
            {
                return BadRequest("El ID de carrera de la ruta no coincide con el ID del objeto.");
            }

             if (_context.UsuMenuesCarreras == null)
            {
                 return NotFound("La entidad 'UsuMenuesCarreras' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (CcaaIdCarrera)
            _context.Entry(usuMenuesCarrera).State = EntityState.Modified;
            _context.Entry(usuMenuesCarrera).Property(x => x.CcaaIdCarrera).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuMenuesCarreraExists(idCarrera))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
             catch(InvalidOperationException ex)
            {
                 // Captura el error si se intentó modificar la PK (ya prevenido arriba)
                 return BadRequest($"No se puede modificar la clave primaria (CcaaIdCarrera). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/UsuMenuesCarrera
        // Para crear la configuración de menú para una nueva carrera.
        // Nota: Probablemente restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<UsuMenuesCarrera>> PostUsuMenuesCarrera(UsuMenuesCarrera usuMenuesCarrera)
        {
             if (_context.UsuMenuesCarreras == null)
            {
                 return Problem("La entidad 'UsuMenuesCarreras' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales (ej: que CcaaIdCarrera exista en la tabla de Carreras)

            _context.UsuMenuesCarreras.Add(usuMenuesCarrera);
            try
            {
                 // Asume que CcaaIdCarrera debe ser único (una configuración por carrera).
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CcaaIdCarrera ya existe
                 if (UsuMenuesCarreraExists(usuMenuesCarrera.CcaaIdCarrera))
                 {
                     return Conflict($"Ya existe una configuración de menú para la carrera con ID {usuMenuesCarrera.CcaaIdCarrera}");
                 }
                 else
                 {
                      // O un error de FK si la carrera no existe
                     return Problem($"Error al guardar la configuración de menú: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetUsuMenuesCarrera), new { idCarrera = usuMenuesCarrera.CcaaIdCarrera }, usuMenuesCarrera);
        }

        // DELETE: api/UsuMenuesCarrera/{idCarrera}
        // Elimina la configuración de menú para una carrera.
        // Nota: Probablemente restringido a roles administrativos.
        [HttpDelete("{idCarrera}")]
        public async Task<IActionResult> DeleteUsuMenuesCarrera(int idCarrera)
        {
             if (_context.UsuMenuesCarreras == null)
            {
                 return NotFound("La entidad 'UsuMenuesCarreras' no está configurada en el DbContext.");
            }

            var usuMenuesCarrera = await _context.UsuMenuesCarreras.FindAsync(idCarrera);
            if (usuMenuesCarrera == null)
            {
                return NotFound();
            }

            _context.UsuMenuesCarreras.Remove(usuMenuesCarrera);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si existe configuración para una carrera por su ID
        private bool UsuMenuesCarreraExists(int idCarrera)
        {
            return (_context.UsuMenuesCarreras?.Any(e => e.CcaaIdCarrera == idCarrera)).GetValueOrDefault();
        }
    }
}