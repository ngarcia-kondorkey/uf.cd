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
    public class CampEstadoAsignaturaController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampEstadoAsignaturaController> _logger;

        public CampEstadoAsignaturaController(ExtranetContext context, IConfiguration configuration, ILogger<CampEstadoAsignaturaController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampEstadoAsignatura
        // Obtiene todos los estados de asignatura.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampEstadoAsignatura>>> GetCampEstadosAsignatura()
        {
            if (_context.CampEstadoAsignaturas == null)
            {
                return NotFound("La entidad 'CampEstadosAsignatura' no está configurada en el DbContext.");
            }
            return await _context.CampEstadoAsignaturas.ToListAsync();
        }

        // GET: api/CampEstadoAsignatura/{id}
        // Busca un estado de asignatura por su clave primaria CeasIdEstadoasignatura (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampEstadoAsignatura>> GetCampEstadoAsignatura(int id)
        {
            if (_context.CampEstadoAsignaturas == null)
            {
                 return NotFound("La entidad 'CampEstadosAsignatura' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campEstadoAsignatura = await _context.CampEstadoAsignaturas.FindAsync(id);

            if (campEstadoAsignatura == null)
            {
                return NotFound();
            }

            return campEstadoAsignatura;
        }

        // PUT: api/CampEstadoAsignatura/{id}
        // Para actualizar un estado de asignatura existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampEstadoAsignatura(int id, CampEstadoAsignatura campEstadoAsignatura)
        {
            if (id != campEstadoAsignatura.CeasIdEstadoasignatura)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del estado de asignatura.");
            }

             if (_context.CampEstadoAsignaturas == null)
            {
                 return NotFound("La entidad 'CampEstadosAsignatura' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (CeasIdEstadoasignatura)
            _context.Entry(campEstadoAsignatura).State = EntityState.Modified;
             _context.Entry(campEstadoAsignatura).Property(x => x.CeasIdEstadoasignatura).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampEstadoAsignaturaExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (CeasIdEstadoasignatura). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampEstadoAsignatura
        // Para crear un nuevo estado de asignatura.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampEstadoAsignatura>> PostCampEstadoAsignatura(CampEstadoAsignatura campEstadoAsignatura)
        {
             if (_context.CampEstadoAsignaturas == null)
            {
                 return Problem("La entidad 'CampEstadosAsignatura' no está configurada en el DbContext.");
            }

            _context.CampEstadoAsignaturas.Add(campEstadoAsignatura);
            try
            {
                 // Asume que CeasIdEstadoasignatura es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CeasIdEstadoasignatura NO es autogenerado y ya existe
                 if (CampEstadoAsignaturaExists(campEstadoAsignatura.CeasIdEstadoasignatura))
                 {
                     return Conflict($"Ya existe un estado de asignatura con ID {campEstadoAsignatura.CeasIdEstadoasignatura}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el estado de asignatura: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampEstadoAsignatura), new { id = campEstadoAsignatura.CeasIdEstadoasignatura }, campEstadoAsignatura);
        }

        // DELETE: api/CampEstadoAsignatura/{id}
        // Elimina un estado de asignatura.
        // Nota: Podría estar restringido o fallar si existen registros (ej: en CampCursoAlumno) con este estado.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampEstadoAsignatura(int id)
        {
             if (_context.CampEstadoAsignaturas == null)
            {
                 return NotFound("La entidad 'CampEstadosAsignatura' no está configurada en el DbContext.");
            }

            var campEstadoAsignatura = await _context.CampEstadoAsignaturas.FindAsync(id);
            if (campEstadoAsignatura == null)
            {
                return NotFound();
            }

            _context.CampEstadoAsignaturas.Remove(campEstadoAsignatura);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (ej. si la tabla CampCursoAlumno tiene una FK hacia CeasIdEstadoasignatura)
                  return Problem($"No se pudo eliminar el estado de asignatura. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }


            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un estado de asignatura existe por su ID
        private bool CampEstadoAsignaturaExists(int id)
        {
            return (_context.CampEstadoAsignaturas?.Any(e => e.CeasIdEstadoasignatura == id)).GetValueOrDefault();
        }
    }
}