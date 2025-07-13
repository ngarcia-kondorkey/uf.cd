using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uf.cd.db.Model; // Namespace del modelo
using Microsoft.AspNetCore.Http; // Necesario para StatusCodes

namespace uf.cd.api.Controllers // Namespace especificado para el controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampInfoAcademTipoTituloController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampInfoAcademTipoTituloController> _logger;

        public CampInfoAcademTipoTituloController(ExtranetContext context, IConfiguration configuration, ILogger<CampInfoAcademTipoTituloController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampInfoAcademTipoTitulo
        // Obtiene todos los tipos de título académico.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampInfoAcademTipoTitulo>>> GetCampInfoAcademTiposTitulos()
        {
            if (_context.CampInfoAcademTipoTitulos == null)
            {
                return NotFound("La entidad 'CampInfoAcademTiposTitulos' no está configurada en el DbContext.");
            }
            // Considera ordenar los tipos, por ejemplo, por CittOrden si existe y es relevante.
            return await _context.CampInfoAcademTipoTitulos.OrderBy(t => t.CittOrden).ThenBy(t => t.CittTipoTitulo).ToListAsync();
        }

        // GET: api/CampInfoAcademTipoTitulo/{id}
        // Busca un tipo de título por su clave primaria CittIdTipoTitulo (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampInfoAcademTipoTitulo>> GetCampInfoAcademTipoTitulo(int id)
        {
            if (_context.CampInfoAcademTipoTitulos == null)
            {
                 return NotFound("La entidad 'CampInfoAcademTiposTitulos' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campInfoAcademTipoTitulo = await _context.CampInfoAcademTipoTitulos.FindAsync(id);

            if (campInfoAcademTipoTitulo == null)
            {
                return NotFound();
            }

            return campInfoAcademTipoTitulo;
        }

        // PUT: api/CampInfoAcademTipoTitulo/{id}
        // Para actualizar un tipo de título existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampInfoAcademTipoTitulo(int id, CampInfoAcademTipoTitulo campInfoAcademTipoTitulo)
        {
            if (id != campInfoAcademTipoTitulo.CittIdTipoTitulo)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del tipo de título.");
            }

             if (_context.CampInfoAcademTipoTitulos == null)
            {
                 return NotFound("La entidad 'CampInfoAcademTiposTitulos' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (CittIdTipoTitulo)
            _context.Entry(campInfoAcademTipoTitulo).State = EntityState.Modified;
             _context.Entry(campInfoAcademTipoTitulo).Property(x => x.CittIdTipoTitulo).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampInfoAcademTipoTituloExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (CittIdTipoTitulo). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampInfoAcademTipoTitulo
        // Para crear un nuevo tipo de título.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampInfoAcademTipoTitulo>> PostCampInfoAcademTipoTitulo(CampInfoAcademTipoTitulo campInfoAcademTipoTitulo)
        {
             if (_context.CampInfoAcademTipoTitulos == null)
            {
                 return Problem("La entidad 'CampInfoAcademTiposTitulos' no está configurada en el DbContext.");
            }

            _context.CampInfoAcademTipoTitulos.Add(campInfoAcademTipoTitulo);
            try
            {
                 // Asume que CittIdTipoTitulo es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CittIdTipoTitulo NO es autogenerado y ya existe
                 if (CampInfoAcademTipoTituloExists(campInfoAcademTipoTitulo.CittIdTipoTitulo))
                 {
                     return Conflict($"Ya existe un tipo de título con ID {campInfoAcademTipoTitulo.CittIdTipoTitulo}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el tipo de título: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampInfoAcademTipoTitulo), new { id = campInfoAcademTipoTitulo.CittIdTipoTitulo }, campInfoAcademTipoTitulo);
        }

        // DELETE: api/CampInfoAcademTipoTitulo/{id}
        // Elimina un tipo de título.
        // Nota: Podría estar restringido o fallar si existen registros (ej: en CampInfoAcademicaPrevium) con este tipo.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampInfoAcademTipoTitulo(int id)
        {
             if (_context.CampInfoAcademTipoTitulos == null)
            {
                 return NotFound("La entidad 'CampInfoAcademTiposTitulos' no está configurada en el DbContext.");
            }

            var campInfoAcademTipoTitulo = await _context.CampInfoAcademTipoTitulos.FindAsync(id);
            if (campInfoAcademTipoTitulo == null)
            {
                return NotFound();
            }

            _context.CampInfoAcademTipoTitulos.Remove(campInfoAcademTipoTitulo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (ej. si la tabla CampInfoAcademicaPrevium tiene una FK hacia CittIdTipoTitulo)
                  return Problem($"No se pudo eliminar el tipo de título. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un tipo de título existe por su ID
        private bool CampInfoAcademTipoTituloExists(int id)
        {
            return (_context.CampInfoAcademTipoTitulos?.Any(e => e.CittIdTipoTitulo == id)).GetValueOrDefault();
        }
    }
}