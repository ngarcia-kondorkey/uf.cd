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
    public class CampInfoAcademNiveleController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampInfoAcademNiveleController> _logger;

        public CampInfoAcademNiveleController(ExtranetContext context, IConfiguration configuration, ILogger<CampInfoAcademNiveleController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampInfoAcademNivele
        // Obtiene todos los niveles académicos.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampInfoAcademNivele>>> GetCampInfoAcademNiveles()
        {
            if (_context.CampInfoAcademNiveles == null)
            {
                return NotFound("La entidad 'CampInfoAcademNiveles' no está configurada en el DbContext.");
            }
            // Considera ordenar los niveles, por ejemplo, por CialOrden si existe y es relevante.
            return await _context.CampInfoAcademNiveles.OrderBy(n => n.CialOrden).ThenBy(n=> n.CialNivel).ToListAsync();
        }

        // GET: api/CampInfoAcademNivele/{id}
        // Busca un nivel académico por su clave primaria CialIdNivel (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampInfoAcademNivele>> GetCampInfoAcademNivele(int id)
        {
            if (_context.CampInfoAcademNiveles == null)
            {
                 return NotFound("La entidad 'CampInfoAcademNiveles' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campInfoAcademNivele = await _context.CampInfoAcademNiveles.FindAsync(id);

            if (campInfoAcademNivele == null)
            {
                return NotFound();
            }

            return campInfoAcademNivele;
        }

        // PUT: api/CampInfoAcademNivele/{id}
        // Para actualizar un nivel académico existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampInfoAcademNivele(int id, CampInfoAcademNivele campInfoAcademNivele)
        {
            if (id != campInfoAcademNivele.CialIdNivel)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del nivel académico.");
            }

             if (_context.CampInfoAcademNiveles == null)
            {
                 return NotFound("La entidad 'CampInfoAcademNiveles' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (CialIdNivel)
            _context.Entry(campInfoAcademNivele).State = EntityState.Modified;
            _context.Entry(campInfoAcademNivele).Property(x => x.CialIdNivel).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampInfoAcademNiveleExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (CialIdNivel). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampInfoAcademNivele
        // Para crear un nuevo nivel académico.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampInfoAcademNivele>> PostCampInfoAcademNivele(CampInfoAcademNivele campInfoAcademNivele)
        {
             if (_context.CampInfoAcademNiveles == null)
            {
                 return Problem("La entidad 'CampInfoAcademNiveles' no está configurada en el DbContext.");
            }

            _context.CampInfoAcademNiveles.Add(campInfoAcademNivele);
            try
            {
                 // Asume que CialIdNivel es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CialIdNivel NO es autogenerado y ya existe
                 if (CampInfoAcademNiveleExists(campInfoAcademNivele.CialIdNivel))
                 {
                     return Conflict($"Ya existe un nivel académico con ID {campInfoAcademNivele.CialIdNivel}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el nivel académico: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampInfoAcademNivele), new { id = campInfoAcademNivele.CialIdNivel }, campInfoAcademNivele);
        }

        // DELETE: api/CampInfoAcademNivele/{id}
        // Elimina un nivel académico.
        // Nota: Podría estar restringido o fallar si existen registros (ej: en CampInfoAcademicaPrevium) con este nivel.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampInfoAcademNivele(int id)
        {
             if (_context.CampInfoAcademNiveles == null)
            {
                 return NotFound("La entidad 'CampInfoAcademNiveles' no está configurada en el DbContext.");
            }

            var campInfoAcademNivele = await _context.CampInfoAcademNiveles.FindAsync(id);
            if (campInfoAcademNivele == null)
            {
                return NotFound();
            }

            _context.CampInfoAcademNiveles.Remove(campInfoAcademNivele);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (ej. si la tabla CampInfoAcademicaPrevium tiene una FK hacia CialIdNivel)
                  return Problem($"No se pudo eliminar el nivel académico. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un nivel académico existe por su ID
        private bool CampInfoAcademNiveleExists(int id)
        {
            return (_context.CampInfoAcademNiveles?.Any(e => e.CialIdNivel == id)).GetValueOrDefault();
        }
    }
}