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
    public class UsuRoleController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuRoleController> _logger;

        public UsuRoleController(ExtranetContext context, IConfiguration configuration, ILogger<UsuRoleController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/UsuRole
        // Obtiene todos los roles.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuRole>>> GetUsuRoles()
        {
            if (_context.UsuRoles == null)
            {
                return NotFound("La entidad 'UsuRoles' no está configurada en el DbContext.");
            }
            return await _context.UsuRoles.OrderBy(r => r.UsroRol).ToListAsync();
        }

        // GET: api/UsuRole/{rol}
        // Busca un rol específico por su clave primaria UsroRol (string)
        [HttpGet("{rol}")]
        public async Task<ActionResult<UsuRole>> GetUsuRole(string rol)
        {
            if (_context.UsuRoles == null)
            {
                 return NotFound("La entidad 'UsuRoles' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            // Considerar normalización/validación de 'rol' si es necesario
            var usuRole = await _context.UsuRoles.FindAsync(rol);

            if (usuRole == null)
            {
                return NotFound();
            }

            return usuRole;
        }

        // PUT: api/UsuRole/{rol}
        // Para actualizar un rol existente.
        [HttpPut("{rol}")]
        public async Task<IActionResult> PutUsuRole(string rol, UsuRole usuRole)
        {
            if (rol != usuRole.UsroRol)
            {
                return BadRequest("El 'Rol' de la ruta no coincide con el 'Rol' del objeto.");
            }

             if (_context.UsuRoles == null)
            {
                 return NotFound("La entidad 'UsuRoles' no está configurada en el DbContext.");
            }

            DateOnly fechaAComparar = DateOnly.FromDateTime(DateTime.UtcNow);

             // Establecer fecha y usuario de modificación
             usuRole.UsroFchModi = fechaAComparar; // O DateTime.Now
             // usuRole.UsroUsrModi = User.Identity?.Name; // Obtener usuario autenticado si aplica

            // Solo permite modificar campos que no son parte de la clave primaria (UsroRol)
            _context.Entry(usuRole).State = EntityState.Modified;
             _context.Entry(usuRole).Property(x => x.UsroRol).IsModified = false; // Previene modificación PK
             _context.Entry(usuRole).Property(x => x.UsroFchAlta).IsModified = false; // No modificar fecha alta
             _context.Entry(usuRole).Property(x => x.UsroUsrAlta).IsModified = false; // No modificar usuario alta

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuRoleExists(rol))
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
                 return BadRequest($"No se puede modificar la clave primaria (UsroRol). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/UsuRole
        // Para crear un nuevo rol.
        [HttpPost]
        public async Task<ActionResult<UsuRole>> PostUsuRole(UsuRole usuRole)
        {
             if (_context.UsuRoles == null)
            {
                 return Problem("La entidad 'UsuRoles' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(usuRole.UsroRol))
             {
                 return BadRequest("El campo 'UsroRol' es requerido.");
             }

            DateOnly fechaAComparar = DateOnly.FromDateTime(DateTime.UtcNow);
             // Establecer fecha y usuario de alta
             usuRole.UsroFchAlta = fechaAComparar; // O DateTime.Now
             // usuRole.UsroUsrAlta = User.Identity?.Name; // Obtener usuario autenticado si aplica
             usuRole.UsroFchModi = null; // Asegurar que fecha modi sea null al crear
             usuRole.UsroUsrModi = null;


            _context.UsuRoles.Add(usuRole);
            try
            {
                 // Asume que UsroRol debe ser único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si UsroRol ya existe
                 if (UsuRoleExists(usuRole.UsroRol))
                 {
                     return Conflict($"Ya existe un rol con el nombre {usuRole.UsroRol}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el rol: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetUsuRole), new { rol = usuRole.UsroRol }, usuRole);
        }

        // DELETE: api/UsuRole/{rol}
        // Elimina un rol.
        // Nota: Podría estar restringido o fallar si existen usuarios o permisos asociados a este rol.
        [HttpDelete("{rol}")]
        public async Task<IActionResult> DeleteUsuRole(string rol)
        {
             if (_context.UsuRoles == null)
            {
                 return NotFound("La entidad 'UsuRoles' no está configurada en el DbContext.");
            }

            var usuRole = await _context.UsuRoles.FindAsync(rol);
            if (usuRole == null)
            {
                return NotFound();
            }

            _context.UsuRoles.Remove(usuRole);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                  return Problem($"No se pudo eliminar el rol. Es posible que esté en uso (ej: por usuarios o permisos). Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un rol existe por su nombre (PK)
        private bool UsuRoleExists(string rol)
        {
            // Considerar comparación insensible a mayúsculas/minúsculas si aplica
            return (_context.UsuRoles?.Any(e => e.UsroRol == rol)).GetValueOrDefault();
        }
    }
}