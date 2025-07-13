using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uf.cd.db.Model; // Namespace del modelo
using System; // Necesario para DateTime
using Microsoft.AspNetCore.Http; // Necesario para StatusCodes

namespace uf.cd.api.Controllers // Namespace especificado para el controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampEncuestasPeriodoController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampEncuestasPeriodoController> _logger;

        public CampEncuestasPeriodoController(ExtranetContext context, IConfiguration configuration, ILogger<CampEncuestasPeriodoController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampEncuestasPeriodo
        // Obtiene todos los períodos de encuesta.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampEncuestasPeriodo>>> GetCampEncuestasPeriodos()
        {
            if (_context.CampEncuestasPeriodos == null)
            {
                return NotFound("La entidad 'CampEncuestasPeriodos' no está configurada en el DbContext.");
            }
            // Considera ordenar los períodos, por ejemplo, por fecha de inicio.
            return await _context.CampEncuestasPeriodos.OrderByDescending(p => p.Desde).ToListAsync();
        }

        // GET: api/CampEncuestasPeriodo/{id}
        // Busca un período específico por su clave primaria Id (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampEncuestasPeriodo>> GetCampEncuestasPeriodo(int id)
        {
            if (_context.CampEncuestasPeriodos == null)
            {
                 return NotFound("La entidad 'CampEncuestasPeriodos' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campEncuestasPeriodo = await _context.CampEncuestasPeriodos.FindAsync(id);

            if (campEncuestasPeriodo == null)
            {
                return NotFound();
            }

            return campEncuestasPeriodo;
        }

        // PUT: api/CampEncuestasPeriodo/{id}
        // Para actualizar un período de encuesta existente.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampEncuestasPeriodo(int id, CampEncuestasPeriodo campEncuestasPeriodo)
        {
            if (id != campEncuestasPeriodo.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del período.");
            }

             if (_context.CampEncuestasPeriodos == null)
            {
                 return NotFound("La entidad 'CampEncuestasPeriodos' no está configurada en el DbContext.");
            }

            // Valida que la fecha Desde sea anterior o igual a Hasta
            if (campEncuestasPeriodo.Desde > campEncuestasPeriodo.Hasta)
            {
                 return BadRequest("La fecha 'Desde' no puede ser posterior a la fecha 'Hasta'.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (Id)
            _context.Entry(campEncuestasPeriodo).State = EntityState.Modified;
            _context.Entry(campEncuestasPeriodo).Property(x => x.Id).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampEncuestasPeriodoExists(id))
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

        // POST: api/CampEncuestasPeriodo
        // Para crear un nuevo período de encuesta.
        [HttpPost]
        public async Task<ActionResult<CampEncuestasPeriodo>> PostCampEncuestasPeriodo(CampEncuestasPeriodo campEncuestasPeriodo)
        {
             if (_context.CampEncuestasPeriodos == null)
            {
                 return Problem("La entidad 'CampEncuestasPeriodos' no está configurada en el DbContext.");
            }

             // Valida que la fecha Desde sea anterior o igual a Hasta
            if (campEncuestasPeriodo.Desde > campEncuestasPeriodo.Hasta)
            {
                 return BadRequest("La fecha 'Desde' no puede ser posterior a la fecha 'Hasta'.");
            }

             // Asegura que Id no se envíe o sea 0 si es autogenerado por la BD
             // campEncuestasPeriodo.Id = 0; // Descomentar si Id es Identity y quieres asegurarte

            _context.CampEncuestasPeriodos.Add(campEncuestasPeriodo);
            try
            {
                 // El Id probablemente será generado por la base de datos al guardar.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar otros posibles errores de base de datos
                 return Problem($"Error al guardar el período de encuesta: {ex.InnerException?.Message ?? ex.Message}");
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampEncuestasPeriodo), new { id = campEncuestasPeriodo.Id }, campEncuestasPeriodo);
        }

        // DELETE: api/CampEncuestasPeriodo/{id}
        // Elimina un período de encuesta.
        // Nota: Podría fallar si existen encuestas asociadas a este período.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampEncuestasPeriodo(int id)
        {
             if (_context.CampEncuestasPeriodos == null)
            {
                 return NotFound("La entidad 'CampEncuestasPeriodos' no está configurada en el DbContext.");
            }

            var campEncuestasPeriodo = await _context.CampEncuestasPeriodos.FindAsync(id);
            if (campEncuestasPeriodo == null)
            {
                return NotFound();
            }

            _context.CampEncuestasPeriodos.Remove(campEncuestasPeriodo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (La excepción específica puede variar según el motor de BD)
                  return Problem($"No se pudo eliminar el período. Es posible que esté en uso por encuestas. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un período existe por su ID
        private bool CampEncuestasPeriodoExists(int id)
        {
            return (_context.CampEncuestasPeriodos?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}