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
    public class UsuMenuesApliController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuMenuesApliController> _logger;

        public UsuMenuesApliController(ExtranetContext context, IConfiguration configuration, ILogger<UsuMenuesApliController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/UsuMenuesApli
        // Obtiene todos los items de menú para todas las aplicaciones.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuMenuesApli>>> GetUsuMenuesAplis()
        {
            if (_context.UsuMenuesAplis == null)
            {
                return NotFound("La entidad 'UsuMenuesAplis' no está configurada en el DbContext.");
            }
            // Considera ordenar por aplicación, nivel, orden, etc. y paginar.
            return await _context.UsuMenuesAplis
                .OrderBy(m => m.UsmaUsapApli)
                .ThenBy(m => m.UsmaNivel1)
                .ThenBy(m => m.UsmaNivel2)
                .ThenBy(m => m.UsmaOrden)
                .ToListAsync();
        }

        // GET: api/UsuMenuesApli/{apli}/{item}
        // Busca un item de menú específico por su clave primaria compuesta asumida (Aplicación, Item).
        // Nota: Considera codificar 'apli' e 'item' si pueden contener caracteres especiales para URL.
        [HttpGet("{apli}/{item}")]
        public async Task<ActionResult<UsuMenuesApli>> GetUsuMenuesApli(string apli, string item)
        {
            if (_context.UsuMenuesAplis == null)
            {
                 return NotFound("La entidad 'UsuMenuesAplis' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var usuMenuesApli = await _context.UsuMenuesAplis.FindAsync(apli, item);

            if (usuMenuesApli == null)
            {
                return NotFound();
            }

            return usuMenuesApli;
        }

        // PUT: api/UsuMenuesApli/{apli}/{item}
        // Para actualizar un item de menú existente usando la clave compuesta.
        [HttpPut("{apli}/{item}")]
        public async Task<IActionResult> PutUsuMenuesApli(string apli, string item, UsuMenuesApli usuMenuesApli)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (apli != usuMenuesApli.UsmaUsapApli || item != usuMenuesApli.UsmaItem)
            {
                return BadRequest("La clave compuesta (Apli, Item) de la ruta no coincide con la clave del objeto.");
            }

             if (_context.UsuMenuesAplis == null)
            {
                 return NotFound("La entidad 'UsuMenuesAplis' no está configurada en el DbContext.");
            }

            DateOnly fechaAComparar = DateOnly.FromDateTime(DateTime.UtcNow);
             // Establecer fecha y usuario de modificación
             usuMenuesApli.UsmaFchModi = fechaAComparar; // O DateTime.Now
             // usuMenuesApli.UsmaUsrModi = User.Identity?.Name; // Obtener usuario autenticado si aplica

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(usuMenuesApli).State = EntityState.Modified;
            _context.Entry(usuMenuesApli).Property(x => x.UsmaUsapApli).IsModified = false; // Previene modificación PK
            _context.Entry(usuMenuesApli).Property(x => x.UsmaItem).IsModified = false; // Previene modificación PK
            _context.Entry(usuMenuesApli).Property(x => x.UsmaFchAlta).IsModified = false; // No modificar fecha alta
            _context.Entry(usuMenuesApli).Property(x => x.UsmaUsrAlta).IsModified = false; // No modificar usuario alta


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuMenuesApliExists(apli, item))
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
                 return BadRequest($"No se puede modificar la clave primaria. Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/UsuMenuesApli
        // Para crear un nuevo item de menú.
        [HttpPost]
        public async Task<ActionResult<UsuMenuesApli>> PostUsuMenuesApli(UsuMenuesApli usuMenuesApli)
        {
             if (_context.UsuMenuesAplis == null)
            {
                 return Problem("La entidad 'UsuMenuesAplis' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(usuMenuesApli.UsmaUsapApli) ||
                string.IsNullOrWhiteSpace(usuMenuesApli.UsmaItem) ||
                string.IsNullOrWhiteSpace(usuMenuesApli.UsmaNivel1))
             {
                 return BadRequest("UsapApli, UsmaItem y UsmaNivel1 son requeridos.");
             }

            DateOnly fechaAComparar = DateOnly.FromDateTime(DateTime.UtcNow);

              // Establecer fecha y usuario de alta
             usuMenuesApli.UsmaFchAlta = fechaAComparar; // O DateTime.Now
             // usuMenuesApli.UsmaUsrAlta = User.Identity?.Name; // Obtener usuario autenticado si aplica
             usuMenuesApli.UsmaFchModi = null; // Asegurar que fecha modi sea null al crear
             usuMenuesApli.UsmaUsrModi = null;

            _context.UsuMenuesAplis.Add(usuMenuesApli);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (UsuMenuesApliExists(usuMenuesApli.UsmaUsapApli, usuMenuesApli.UsmaItem))
                {
                    return Conflict($"Ya existe un item de menú con esta clave (Apli={usuMenuesApli.UsmaUsapApli}, Item={usuMenuesApli.UsmaItem}).");
                }
                else
                {
                    // Podría ser un error de FK si la aplicación no existe
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetUsuMenuesApli), new {
                 apli = usuMenuesApli.UsmaUsapApli, // Considera URL encoding
                 item = usuMenuesApli.UsmaItem // Considera URL encoding
                 }, usuMenuesApli);
        }

        // DELETE: api/UsuMenuesApli/{apli}/{item}
        // Elimina un item de menú.
        // Nota: Podría fallar si existen permisos/roles asociados a este item.
        [HttpDelete("{apli}/{item}")]
        public async Task<IActionResult> DeleteUsuMenuesApli(string apli, string item)
        {
             if (_context.UsuMenuesAplis == null)
            {
                 return NotFound("La entidad 'UsuMenuesAplis' no está configurada en el DbContext.");
            }

            var usuMenuesApli = await _context.UsuMenuesAplis.FindAsync(apli, item);
            if (usuMenuesApli == null)
            {
                return NotFound();
            }

            _context.UsuMenuesAplis.Remove(usuMenuesApli);
             try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                  return Problem($"No se pudo eliminar el item de menú. Es posible que esté en uso (ej: por permisos o roles). Error: {ex.InnerException?.Message ?? ex.Message}");
            }


            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool UsuMenuesApliExists(string apli, string item)
        {
             // Considerar comparación insensible a mayúsculas/minúsculas si aplica
            return (_context.UsuMenuesAplis?.Any(e =>
                e.UsmaUsapApli == apli &&
                e.UsmaItem == item)).GetValueOrDefault();
        }
    }
}