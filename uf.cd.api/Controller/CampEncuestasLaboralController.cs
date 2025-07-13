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
    public class CampEncuestasLaboralController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampEncuestasLaboralController> _logger;

        public CampEncuestasLaboralController(ExtranetContext context, IConfiguration configuration, ILogger<CampEncuestasLaboralController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampEncuestasLaboral
        // Obtiene todas las respuestas de la encuesta laboral.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampEncuestasLaboral>>> GetCampEncuestasLaborales()
        {
            if (_context.CampEncuestasLaborals == null)
            {
                return NotFound("La entidad 'CampEncuestasLaborales' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todas las respuestas puede ser ineficiente.
             // Considera paginación y filtrado (ej: por alumno, carrera, año).
            return await _context.CampEncuestasLaborals.ToListAsync();
        }

        // GET: api/CampEncuestasLaboral/{id}
        // Busca una respuesta específica por su clave primaria Id (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampEncuestasLaboral>> GetCampEncuestasLaboral(int id)
        {
            if (_context.CampEncuestasLaborals == null)
            {
                 return NotFound("La entidad 'CampEncuestasLaborales' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campEncuestasLaboral = await _context.CampEncuestasLaborals.FindAsync(id);

            if (campEncuestasLaboral == null)
            {
                return NotFound();
            }

            return campEncuestasLaboral;
        }

        // PUT: api/CampEncuestasLaboral/{id}
        // Para actualizar una respuesta de encuesta existente.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampEncuestasLaboral(int id, CampEncuestasLaboral campEncuestasLaboral)
        {
            if (id != campEncuestasLaboral.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la encuesta.");
            }

             if (_context.CampEncuestasLaborals == null)
            {
                 return NotFound("La entidad 'CampEncuestasLaborales' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (Id)
            _context.Entry(campEncuestasLaboral).State = EntityState.Modified;
            _context.Entry(campEncuestasLaboral).Property(x => x.Id).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampEncuestasLaboralExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (Id). Error: {ex.Message}");
            }


            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampEncuestasLaboral
        // Para crear una nueva respuesta de encuesta.
        [HttpPost]
        public async Task<ActionResult<CampEncuestasLaboral>> PostCampEncuestasLaboral(CampEncuestasLaboral campEncuestasLaboral)
        {
             if (_context.CampEncuestasLaborals == null)
            {
                 return Problem("La entidad 'CampEncuestasLaborales' no está configurada en el DbContext.");
            }

             // Asegura que el Id no se envíe o sea 0 si es autogenerado por la BD
             // campEncuestasLaboral.Id = 0; // Descomentar si Id es Identity y quieres asegurarte

            _context.CampEncuestasLaborals.Add(campEncuestasLaboral);
            try
            {
                 // El Id probablemente será generado por la base de datos al guardar.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar otros posibles errores de base de datos
                 return Problem($"Error al guardar la respuesta de encuesta: {ex.InnerException?.Message ?? ex.Message}");
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampEncuestasLaboral), new { id = campEncuestasLaboral.Id }, campEncuestasLaboral);
        }

        // DELETE: api/CampEncuestasLaboral/{id}
        // Elimina una respuesta de encuesta.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampEncuestasLaboral(int id)
        {
             if (_context.CampEncuestasLaborals == null)
            {
                 return NotFound("La entidad 'CampEncuestasLaborales' no está configurada en el DbContext.");
            }

            var campEncuestasLaboral = await _context.CampEncuestasLaborals.FindAsync(id);
            if (campEncuestasLaboral == null)
            {
                return NotFound();
            }

            _context.CampEncuestasLaborals.Remove(campEncuestasLaboral);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si una respuesta de encuesta existe por su ID
        private bool CampEncuestasLaboralExists(int id)
        {
            return (_context.CampEncuestasLaborals?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}