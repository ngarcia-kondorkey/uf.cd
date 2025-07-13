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
    public class UsuRolesUsuarioController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuRolesUsuarioController> _logger;

        public UsuRolesUsuarioController(ExtranetContext context, IConfiguration configuration, ILogger<UsuRolesUsuarioController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/UsuRolesUsuario
        // Obtiene todas las asignaciones de roles a usuarios.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuRolesUsuario>>> GetUsuRolesUsuarios()
        {
            if (_context.UsuRolesUsuarios == null)
            {
                return NotFound("La entidad 'UsuRolesUsuarios' no está configurada en el DbContext.");
            }
             // Considera filtrar por rol o usuario y paginar si la lista es muy grande.
            return await _context.UsuRolesUsuarios.ToListAsync();
        }

        // GET: api/UsuRolesUsuario/{rol}/{nombre}
        // Busca una asignación específica por su clave primaria compuesta asumida (Rol, NombreUsuario).
        // Nota: Considera codificar 'rol' y 'nombre' si pueden contener caracteres especiales para URL.
        [HttpGet("{rol}/{nombre}")]
        public async Task<ActionResult<UsuRolesUsuario>> GetUsuRolesUsuario(string rol, string nombre)
        {
            if (_context.UsuRolesUsuarios == null)
            {
                 return NotFound("La entidad 'UsuRolesUsuarios' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var usuRolesUsuario = await _context.UsuRolesUsuarios.FindAsync(rol, nombre);

            if (usuRolesUsuario == null)
            {
                return NotFound();
            }

            return usuRolesUsuario;
        }

        // PUT: api/UsuRolesUsuario/{rol}/{nombre}
        // Para actualizar una asignación existente (ej: cambiar estado habilitado).
        // Nota: Generalmente las asignaciones se crean/eliminan, no se actualizan mucho más allá del estado.
        [HttpPut("{rol}/{nombre}")]
        public async Task<IActionResult> PutUsuRolesUsuario(string rol, string nombre, UsuRolesUsuario usuRolesUsuario)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (rol != usuRolesUsuario.UsruUsroRol || nombre != usuRolesUsuario.UsruUsuaNombre)
            {
                return BadRequest("La clave compuesta (Rol, NombreUsuario) de la ruta no coincide con la clave del objeto.");
            }

             if (_context.UsuRolesUsuarios == null)
            {
                 return NotFound("La entidad 'UsuRolesUsuarios' no está configurada en el DbContext.");
            }

            DateTime? _DateTimeNullable = DateTime.UtcNow; // Tu variable/propiedad DateTime?
            DateOnly? _DateOnlyNullable = null; // Inicializa como null

            if (_DateTimeNullable.HasValue)
            {
                // Convierte solo si el DateTime? tiene un valor
                _DateOnlyNullable = DateOnly.FromDateTime(_DateTimeNullable.Value);
            }

             // Establecer fecha y usuario de modificación
             usuRolesUsuario.UsruFchModi = _DateOnlyNullable;
             // usuRolesUsuario.UsruUsrModi = User.Identity?.Name; // Obtener usuario autenticado si aplica

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(usuRolesUsuario).State = EntityState.Modified;
            _context.Entry(usuRolesUsuario).Property(x => x.UsruUsroRol).IsModified = false; // Previene modificación PK
            _context.Entry(usuRolesUsuario).Property(x => x.UsruUsuaNombre).IsModified = false; // Previene modificación PK
            _context.Entry(usuRolesUsuario).Property(x => x.UsruFchAlta).IsModified = false; // No modificar fecha alta
            _context.Entry(usuRolesUsuario).Property(x => x.UsruUsrAlta).IsModified = false; // No modificar usuario alta

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuRolesUsuarioExists(rol, nombre))
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

        // POST: api/UsuRolesUsuario
        // Para asignar un rol a un usuario.
        [HttpPost]
        public async Task<ActionResult<UsuRolesUsuario>> PostUsuRolesUsuario(UsuRolesUsuario usuRolesUsuario)
        {
             if (_context.UsuRolesUsuarios == null)
            {
                 return Problem("La entidad 'UsuRolesUsuarios' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(usuRolesUsuario.UsruUsroRol) || string.IsNullOrWhiteSpace(usuRolesUsuario.UsruUsuaNombre))
             {
                 return BadRequest("UsroRol y UsuaNombre son requeridos.");
             }

            DateOnly fechaAComparar = DateOnly.FromDateTime(DateTime.UtcNow);
             // Establecer fecha y usuario de alta
             usuRolesUsuario.UsruFchAlta = fechaAComparar; // O DateTime.Now
             // usuRolesUsuario.UsruUsrAlta = User.Identity?.Name; // Obtener usuario autenticado si aplica
             usuRolesUsuario.UsruFchModi = null; // Asegurar que fecha modi sea null al crear
             usuRolesUsuario.UsruUsrModi = null;

            _context.UsuRolesUsuarios.Add(usuRolesUsuario);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (UsuRolesUsuarioExists(usuRolesUsuario.UsruUsroRol, usuRolesUsuario.UsruUsuaNombre))
                {
                    return Conflict($"El usuario '{usuRolesUsuario.UsruUsuaNombre}' ya tiene asignado el rol '{usuRolesUsuario.UsruUsroRol}'.");
                }
                else
                {
                    // Podría ser un error de FK si el usuario o el rol no existen
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetUsuRolesUsuario), new {
                 rol = usuRolesUsuario.UsruUsroRol, // Considera URL encoding
                 nombre = usuRolesUsuario.UsruUsuaNombre // Considera URL encoding
                 }, usuRolesUsuario);
        }

        // DELETE: api/UsuRolesUsuario/{rol}/{nombre}
        // Elimina la asignación de un rol a un usuario.
        [HttpDelete("{rol}/{nombre}")]
        public async Task<IActionResult> DeleteUsuRolesUsuario(string rol, string nombre)
        {
             if (_context.UsuRolesUsuarios == null)
            {
                 return NotFound("La entidad 'UsuRolesUsuarios' no está configurada en el DbContext.");
            }

            var usuRolesUsuario = await _context.UsuRolesUsuarios.FindAsync(rol, nombre);
            if (usuRolesUsuario == null)
            {
                return NotFound();
            }

            _context.UsuRolesUsuarios.Remove(usuRolesUsuario);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool UsuRolesUsuarioExists(string rol, string nombre)
        {
             // Considerar comparación insensible a mayúsculas/minúsculas si aplica
            return (_context.UsuRolesUsuarios?.Any(e =>
                e.UsruUsroRol == rol &&
                e.UsruUsuaNombre == nombre)).GetValueOrDefault();
        }
    }
}