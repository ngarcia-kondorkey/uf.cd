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
    public class CampEncuestasLaboraleController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampEncuestasLaboraleController> _logger;

        public CampEncuestasLaboraleController(ExtranetContext context, IConfiguration configuration, ILogger<CampEncuestasLaboraleController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampEncuestasLaborale
        // Obtiene todos los registros de la encuesta "laborale".
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampEncuestasLaborale>>> GetCampEncuestasLaborales()
        {
            if (_context.CampEncuestasLaborales == null)
            {
                return NotFound("La entidad 'CampEncuestasLaborales' no está configurada en el DbContext.");
            }
            // Considera paginación/filtrado si la tabla puede crecer mucho.
            return await _context.CampEncuestasLaborales.ToListAsync();
        }

        // GET: api/CampEncuestasLaborale/{id}
        // Busca un registro específico por su clave primaria ElId (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampEncuestasLaborale>> GetCampEncuestasLaborale(int id)
        {
            if (_context.CampEncuestasLaborales == null)
            {
                 return NotFound("La entidad 'CampEncuestasLaborales' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campEncuestasLaborale = await _context.CampEncuestasLaborales.FindAsync(id);

            if (campEncuestasLaborale == null)
            {
                return NotFound();
            }

            return campEncuestasLaborale;
        }

        // PUT: api/CampEncuestasLaborale/{id}
        // Para actualizar un registro existente.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampEncuestasLaborale(int id, CampEncuestasLaborale campEncuestasLaborale)
        {
            if (id != campEncuestasLaborale.ElId)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del registro.");
            }

             if (_context.CampEncuestasLaborales == null)
            {
                 return NotFound("La entidad 'CampEncuestasLaborales' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (ElId)
            _context.Entry(campEncuestasLaborale).State = EntityState.Modified;
            _context.Entry(campEncuestasLaborale).Property(x => x.ElId).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampEncuestasLaboraleExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (ElId). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampEncuestasLaborale
        // Para crear un nuevo registro.
        [HttpPost]
        public async Task<ActionResult<CampEncuestasLaborale>> PostCampEncuestasLaborale(CampEncuestasLaborale campEncuestasLaborale)
        {
             if (_context.CampEncuestasLaborales == null)
            {
                 return Problem("La entidad 'CampEncuestasLaborales' no está configurada en el DbContext.");
            }

             // Asegura que ElId no se envíe o sea 0 si es autogenerado por la BD
             // campEncuestasLaborale.ElId = 0; // Descomentar si ElId es Identity y quieres asegurarte

            _context.CampEncuestasLaborales.Add(campEncuestasLaborale);
            try
            {
                 // El Id probablemente será generado por la base de datos al guardar.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar otros posibles errores de base de datos
                 return Problem($"Error al guardar el registro: {ex.InnerException?.Message ?? ex.Message}");
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampEncuestasLaborale), new { id = campEncuestasLaborale.ElId }, campEncuestasLaborale);
        }

        // DELETE: api/CampEncuestasLaborale/{id}
        // Elimina un registro.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampEncuestasLaborale(int id)
        {
             if (_context.CampEncuestasLaborales == null)
            {
                 return NotFound("La entidad 'CampEncuestasLaborales' no está configurada en el DbContext.");
            }

            var campEncuestasLaborale = await _context.CampEncuestasLaborales.FindAsync(id);
            if (campEncuestasLaborale == null)
            {
                return NotFound();
            }

            _context.CampEncuestasLaborales.Remove(campEncuestasLaborale);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su ID
        private bool CampEncuestasLaboraleExists(int id)
        {
            return (_context.CampEncuestasLaborales?.Any(e => e.ElId == id)).GetValueOrDefault();
        }
    }
}