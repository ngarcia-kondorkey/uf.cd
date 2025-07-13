using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uf.cd.db.Model; // Asumiendo que el modelo está en este namespace
using System; // Necesario para DateTime
using Microsoft.AspNetCore.Http; // Necesario para StatusCodes

namespace uf.cd.api.Controllers // Namespace especificado para el controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuAplicacioneController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuAplicacioneController> _logger;

        public UsuAplicacioneController(ExtranetContext context, IConfiguration configuration, ILogger<UsuAplicacioneController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/UsuAplicacione
        // Obtiene todas las aplicaciones.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuAplicacione>>> GetUsuAplicaciones()
        {
            if (_context.UsuAplicaciones == null)
            {
                return NotFound("La entidad 'UsuAplicaciones' no está configurada en el DbContext.");
            }
            return await _context.UsuAplicaciones.OrderBy(a => a.UsapApli).ToListAsync();
        }

        // GET: api/UsuAplicacione/{apli}
        // Busca una aplicación por su clave primaria UsapApli (string)
        [HttpGet("{apli}")]
        public async Task<ActionResult<UsuAplicacione>> GetUsuAplicacione(string apli)
        {
            if (_context.UsuAplicaciones == null)
            {
                 return NotFound("La entidad 'UsuAplicaciones' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            // Considerar normalización/validación de 'apli' si es necesario
            var usuAplicacione = await _context.UsuAplicaciones.FindAsync(apli);

            if (usuAplicacione == null)
            {
                return NotFound();
            }

            return usuAplicacione;
        }

        // PUT: api/UsuAplicacione/{apli}
        // Para actualizar una aplicación existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{apli}")]
        public async Task<IActionResult> PutUsuAplicacione(string apli, UsuAplicacione usuAplicacione)
        {
            if (apli != usuAplicacione.UsapApli)
            {
                return BadRequest("El 'Apli' de la ruta no coincide con el 'Apli' del objeto.");
            }

             if (_context.UsuAplicaciones == null)
            {
                 return NotFound("La entidad 'UsuAplicaciones' no está configurada en el DbContext.");
            }

            DateOnly fechaAComparar = DateOnly.FromDateTime(DateTime.UtcNow);

            // Establecer fecha y usuario de modificación
             usuAplicacione.UsapFchModi = fechaAComparar; // O DateTime.Now
            // usuAplicacione.UsapUsrModi = User.Identity?.Name; // Obtener usuario autenticado si aplica

            // Solo permite modificar campos que no son parte de la clave primaria (UsapApli)
            _context.Entry(usuAplicacione).State = EntityState.Modified;
             _context.Entry(usuAplicacione).Property(x => x.UsapApli).IsModified = false; // Previene modificación PK
             _context.Entry(usuAplicacione).Property(x => x.UsapFchAlta).IsModified = false; // No modificar fecha alta
             _context.Entry(usuAplicacione).Property(x => x.UsapUsrAlta).IsModified = false; // No modificar usuario alta

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuAplicacioneExists(apli))
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
                 return BadRequest($"No se puede modificar la clave primaria (UsapApli). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/UsuAplicacione
        // Para crear una nueva aplicación.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<UsuAplicacione>> PostUsuAplicacione(UsuAplicacione usuAplicacione)
        {
             if (_context.UsuAplicaciones == null)
            {
                 return Problem("La entidad 'UsuAplicaciones' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(usuAplicacione.UsapApli))
             {
                 return BadRequest("El campo 'UsapApli' es requerido.");
             }

            DateOnly fechaAComparar = DateOnly.FromDateTime(DateTime.UtcNow);
             // Establecer fecha y usuario de alta
             usuAplicacione.UsapFchAlta = fechaAComparar; // O DateTime.Now
             // usuAplicacione.UsapUsrAlta = User.Identity?.Name; // Obtener usuario autenticado si aplica
             usuAplicacione.UsapFchModi = null; // Asegurar que fecha modi sea null al crear
             usuAplicacione.UsapUsrModi = null;

            _context.UsuAplicaciones.Add(usuAplicacione);
            try
            {
                 // Asume que UsapApli debe ser único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si UsapApli ya existe
                 if (UsuAplicacioneExists(usuAplicacione.UsapApli))
                 {
                     return Conflict($"Ya existe una aplicación con Apli {usuAplicacione.UsapApli}");
                 }
                 else
                 {
                     return Problem($"Error al guardar la aplicación: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetUsuAplicacione), new { apli = usuAplicacione.UsapApli }, usuAplicacione);
        }

        // DELETE: api/UsuAplicacione/{apli}
        // Elimina una aplicación.
        // Nota: Podría estar restringido o fallar si existen permisos/roles asociados a esta aplicación.
        [HttpDelete("{apli}")]
        public async Task<IActionResult> DeleteUsuAplicacione(string apli)
        {
             if (_context.UsuAplicaciones == null)
            {
                 return NotFound("La entidad 'UsuAplicaciones' no está configurada en el DbContext.");
            }

            var usuAplicacione = await _context.UsuAplicaciones.FindAsync(apli);
            if (usuAplicacione == null)
            {
                return NotFound();
            }

            _context.UsuAplicaciones.Remove(usuAplicacione);
             try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                  return Problem($"No se pudo eliminar la aplicación. Es posible que esté en uso (ej: por permisos o roles). Error: {ex.InnerException?.Message ?? ex.Message}");
            }


            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si una aplicación existe por su ID (Apli)
        private bool UsuAplicacioneExists(string apli)
        {
            // Considerar comparación insensible a mayúsculas/minúsculas si aplica para 'apli'
            return (_context.UsuAplicaciones?.Any(e => e.UsapApli == apli)).GetValueOrDefault();
        }
    }
}