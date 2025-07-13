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
    public class CampEstadoAdministraController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampEstadoAdministraController> _logger;

        public CampEstadoAdministraController(ExtranetContext context, IConfiguration configuration, ILogger<CampEstadoAdministraController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampEstadoAdministra
        // Obtiene todos los estados administrativos.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampEstadoAdministra>>> GetCampEstadosAdministrativos()
        {
            if (_context.CampEstadoAdministras == null)
            {
                return NotFound("La entidad 'CampEstadosAdministrativos' no está configurada en el DbContext.");
            }
            return await _context.CampEstadoAdministras.ToListAsync();
        }

        // GET: api/CampEstadoAdministra/{id}
        // Busca un estado administrativo por su clave primaria CeadIdEstadoadministrativo (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampEstadoAdministra>> GetCampEstadoAdministra(int id)
        {
            if (_context.CampEstadoAdministras == null)
            {
                 return NotFound("La entidad 'CampEstadosAdministrativos' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campEstadoAdministra = await _context.CampEstadoAdministras.FindAsync(id);

            if (campEstadoAdministra == null)
            {
                return NotFound();
            }

            return campEstadoAdministra;
        }

        // PUT: api/CampEstadoAdministra/{id}
        // Para actualizar un estado administrativo existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampEstadoAdministra(int id, CampEstadoAdministra campEstadoAdministra)
        {
            if (id != campEstadoAdministra.CeadIdEstadoadministrativo)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del estado administrativo.");
            }

             if (_context.CampEstadoAdministras == null)
            {
                 return NotFound("La entidad 'CampEstadosAdministrativos' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (CeadIdEstadoadministrativo)
            _context.Entry(campEstadoAdministra).State = EntityState.Modified;
             _context.Entry(campEstadoAdministra).Property(x => x.CeadIdEstadoadministrativo).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampEstadoAdministraExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (CeadIdEstadoadministrativo). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampEstadoAdministra
        // Para crear un nuevo estado administrativo.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampEstadoAdministra>> PostCampEstadoAdministra(CampEstadoAdministra campEstadoAdministra)
        {
             if (_context.CampEstadoAdministras == null)
            {
                 return Problem("La entidad 'CampEstadosAdministrativos' no está configurada en el DbContext.");
            }

            _context.CampEstadoAdministras.Add(campEstadoAdministra);
            try
            {
                 // Asume que CeadIdEstadoadministrativo es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CeadIdEstadoadministrativo NO es autogenerado y ya existe
                 if (CampEstadoAdministraExists(campEstadoAdministra.CeadIdEstadoadministrativo))
                 {
                     return Conflict($"Ya existe un estado administrativo con ID {campEstadoAdministra.CeadIdEstadoadministrativo}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el estado administrativo: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampEstadoAdministra), new { id = campEstadoAdministra.CeadIdEstadoadministrativo }, campEstadoAdministra);
        }

        // DELETE: api/CampEstadoAdministra/{id}
        // Elimina un estado administrativo.
        // Nota: Podría estar restringido o fallar si existen alumnos (u otras entidades) con este estado.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampEstadoAdministra(int id)
        {
             if (_context.CampEstadoAdministras == null)
            {
                 return NotFound("La entidad 'CampEstadosAdministrativos' no está configurada en el DbContext.");
            }

            var campEstadoAdministra = await _context.CampEstadoAdministras.FindAsync(id);
            if (campEstadoAdministra == null)
            {
                return NotFound();
            }

            _context.CampEstadoAdministras.Remove(campEstadoAdministra);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (ej. si la tabla CampAlumno tiene una FK hacia CeadIdEstadoadministrativo)
                  return Problem($"No se pudo eliminar el estado administrativo. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }


            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un estado administrativo existe por su ID
        private bool CampEstadoAdministraExists(int id)
        {
            return (_context.CampEstadoAdministras?.Any(e => e.CeadIdEstadoadministrativo == id)).GetValueOrDefault();
        }
    }
}