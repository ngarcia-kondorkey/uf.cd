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
    public class UsuMenuesRolController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuMenuesRolController> _logger;

        public UsuMenuesRolController(ExtranetContext context, IConfiguration configuration, ILogger<UsuMenuesRolController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/UsuMenuesRol
        // Obtiene todos los permisos de rol para items de menú.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuMenuesRol>>> GetUsuMenuesRoles()
        {
            if (_context.UsuMenuesRols == null)
            {
                return NotFound("La entidad 'UsuMenuesRoles' no está configurada en el DbContext.");
            }
            // Considera filtrar por aplicación o rol y paginar si la lista es muy grande.
            return await _context.UsuMenuesRols.ToListAsync();
        }

        // GET: api/UsuMenuesRol/{apli}/{rol}/{item}
        // Busca un permiso específico por su clave primaria compuesta asumida (Aplicación, Rol, Item).
        // Nota: Considera codificar 'apli', 'rol', e 'item' si pueden contener caracteres especiales para URL.
        [HttpGet("{apli}/{rol}/{item}")]
        public async Task<ActionResult<UsuMenuesRol>> GetUsuMenuesRol(string apli, string rol, string item)
        {
            if (_context.UsuMenuesRols == null)
            {
                 return NotFound("La entidad 'UsuMenuesRoles' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var usuMenuesRol = await _context.UsuMenuesRols.FindAsync(apli, rol, item);

            if (usuMenuesRol == null)
            {
                return NotFound();
            }

            return usuMenuesRol;
        }

        // PUT: api/UsuMenuesRol/{apli}/{rol}/{item}
        // Para actualizar un permiso existente usando la clave compuesta.
        [HttpPut("{apli}/{rol}/{item}")]
        public async Task<IActionResult> PutUsuMenuesRol(string apli, string rol, string item, UsuMenuesRol usuMenuesRol)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (apli != usuMenuesRol.UsmrUsapApli || rol != usuMenuesRol.UsmrUsroRol || item != usuMenuesRol.UsmrItem)
            {
                return BadRequest("La clave compuesta (Apli, Rol, Item) de la ruta no coincide con la clave del objeto.");
            }

             if (_context.UsuMenuesRols == null)
            {
                 return NotFound("La entidad 'UsuMenuesRoles' no está configurada en el DbContext.");
            }

            DateOnly fechaAComparar = DateOnly.FromDateTime(DateTime.UtcNow);
             // Establecer fecha y usuario de modificación
             usuMenuesRol.UsmrFchModi = fechaAComparar; // O DateTime.Now
             // usuMenuesRol.UsmrUsrModi = User.Identity?.Name; // Obtener usuario autenticado si aplica

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(usuMenuesRol).State = EntityState.Modified;
            _context.Entry(usuMenuesRol).Property(x => x.UsmrUsapApli).IsModified = false; // Previene modificación PK
            _context.Entry(usuMenuesRol).Property(x => x.UsmrUsroRol).IsModified = false; // Previene modificación PK
            _context.Entry(usuMenuesRol).Property(x => x.UsmrItem).IsModified = false; // Previene modificación PK
            _context.Entry(usuMenuesRol).Property(x => x.UsmrFchAlta).IsModified = false; // No modificar fecha alta
            _context.Entry(usuMenuesRol).Property(x => x.UsmrUsrAlta).IsModified = false; // No modificar usuario alta


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuMenuesRolExists(apli, rol, item))
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

        // POST: api/UsuMenuesRol
        // Para crear un nuevo permiso de rol para un item de menú.
        [HttpPost]
        public async Task<ActionResult<UsuMenuesRol>> PostUsuMenuesRol(UsuMenuesRol usuMenuesRol)
        {
             if (_context.UsuMenuesRols == null)
            {
                 return Problem("La entidad 'UsuMenuesRoles' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(usuMenuesRol.UsmrUsapApli) ||
                string.IsNullOrWhiteSpace(usuMenuesRol.UsmrUsroRol) ||
                string.IsNullOrWhiteSpace(usuMenuesRol.UsmrItem))
             {
                 return BadRequest("UsapApli, UsroRol y Item son requeridos.");
             }

            DateOnly fechaAComparar = DateOnly.FromDateTime(DateTime.UtcNow);

              // Establecer fecha y usuario de alta
             usuMenuesRol.UsmrFchAlta = fechaAComparar; // O DateTime.Now
             // usuMenuesRol.UsmrUsrAlta = User.Identity?.Name; // Obtener usuario autenticado si aplica
             usuMenuesRol.UsmrFchModi = null; // Asegurar que fecha modi sea null al crear
             usuMenuesRol.UsmrUsrModi = null;

            _context.UsuMenuesRols.Add(usuMenuesRol);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (UsuMenuesRolExists(usuMenuesRol.UsmrUsapApli, usuMenuesRol.UsmrUsroRol, usuMenuesRol.UsmrItem))
                {
                    return Conflict($"Ya existe un permiso para esta aplicación/rol/item.");
                }
                else
                {
                    // Podría ser un error de FK si la aplicación, rol o item no existen
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetUsuMenuesRol), new {
                 apli = usuMenuesRol.UsmrUsapApli, // Considera URL encoding
                 rol = usuMenuesRol.UsmrUsroRol, // Considera URL encoding
                 item = usuMenuesRol.UsmrItem // Considera URL encoding
                 }, usuMenuesRol);
        }

        // DELETE: api/UsuMenuesRol/{apli}/{rol}/{item}
        // Elimina un permiso de rol para un item de menú.
        [HttpDelete("{apli}/{rol}/{item}")]
        public async Task<IActionResult> DeleteUsuMenuesRol(string apli, string rol, string item)
        {
             if (_context.UsuMenuesRols == null)
            {
                 return NotFound("La entidad 'UsuMenuesRoles' no está configurada en el DbContext.");
            }

            var usuMenuesRol = await _context.UsuMenuesRols.FindAsync(apli, rol, item);
            if (usuMenuesRol == null)
            {
                return NotFound();
            }

            _context.UsuMenuesRols.Remove(usuMenuesRol);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool UsuMenuesRolExists(string apli, string rol, string item)
        {
             // Considerar comparación insensible a mayúsculas/minúsculas si aplica
            return (_context.UsuMenuesRols?.Any(e =>
                e.UsmrUsapApli == apli &&
                e.UsmrUsroRol == rol &&
                e.UsmrItem == item)).GetValueOrDefault();
        }
    }
}