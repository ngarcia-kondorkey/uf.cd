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
    public class CampEstadoAcademicoController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampEstadoAcademicoController> _logger;

        public CampEstadoAcademicoController(ExtranetContext context, IConfiguration configuration, ILogger<CampEstadoAcademicoController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampEstadoAcademico
        // Obtiene todos los estados académicos.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampEstadoAcademico>>> GetCampEstadosAcademicos()
        {
            if (_context.CampEstadoAcademicos == null)
            {
                return NotFound("La entidad 'CampEstadosAcademicos' no está configurada en el DbContext.");
            }
            return await _context.CampEstadoAcademicos.ToListAsync();
        }

        // GET: api/CampEstadoAcademico/{id}
        // Busca un estado académico por su clave primaria CeacIdEstadoacademico (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampEstadoAcademico>> GetCampEstadoAcademico(int id)
        {
            if (_context.CampEstadoAcademicos == null)
            {
                 return NotFound("La entidad 'CampEstadosAcademicos' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campEstadoAcademico = await _context.CampEstadoAcademicos.FindAsync(id);

            if (campEstadoAcademico == null)
            {
                return NotFound();
            }

            return campEstadoAcademico;
        }

        // PUT: api/CampEstadoAcademico/{id}
        // Para actualizar un estado académico existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampEstadoAcademico(int id, CampEstadoAcademico campEstadoAcademico)
        {
            if (id != campEstadoAcademico.CeacIdEstadoacademico)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del estado académico.");
            }

             if (_context.CampEstadoAcademicos == null)
            {
                 return NotFound("La entidad 'CampEstadosAcademicos' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (CeacIdEstadoacademico)
            _context.Entry(campEstadoAcademico).State = EntityState.Modified;
             _context.Entry(campEstadoAcademico).Property(x => x.CeacIdEstadoacademico).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampEstadoAcademicoExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (CeacIdEstadoacademico). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampEstadoAcademico
        // Para crear un nuevo estado académico.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampEstadoAcademico>> PostCampEstadoAcademico(CampEstadoAcademico campEstadoAcademico)
        {
             if (_context.CampEstadoAcademicos == null)
            {
                 return Problem("La entidad 'CampEstadosAcademicos' no está configurada en el DbContext.");
            }

            _context.CampEstadoAcademicos.Add(campEstadoAcademico);
            try
            {
                 // Asume que CeacIdEstadoacademico es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CeacIdEstadoacademico NO es autogenerado y ya existe
                 if (CampEstadoAcademicoExists(campEstadoAcademico.CeacIdEstadoacademico))
                 {
                     return Conflict($"Ya existe un estado académico con ID {campEstadoAcademico.CeacIdEstadoacademico}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el estado académico: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampEstadoAcademico), new { id = campEstadoAcademico.CeacIdEstadoacademico }, campEstadoAcademico);
        }

        // DELETE: api/CampEstadoAcademico/{id}
        // Elimina un estado académico.
        // Nota: Podría estar restringido o fallar si existen alumnos con este estado.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampEstadoAcademico(int id)
        {
             if (_context.CampEstadoAcademicos == null)
            {
                 return NotFound("La entidad 'CampEstadosAcademicos' no está configurada en el DbContext.");
            }

            var campEstadoAcademico = await _context.CampEstadoAcademicos.FindAsync(id);
            if (campEstadoAcademico == null)
            {
                return NotFound();
            }

            _context.CampEstadoAcademicos.Remove(campEstadoAcademico);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (ej. si la tabla CampAlumno tiene una FK hacia CeacIdEstadoacademico)
                  return Problem($"No se pudo eliminar el estado académico. Es posible que esté en uso por alumnos. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un estado académico existe por su ID
        private bool CampEstadoAcademicoExists(int id)
        {
            return (_context.CampEstadoAcademicos?.Any(e => e.CeacIdEstadoacademico == id)).GetValueOrDefault();
        }
    }
}