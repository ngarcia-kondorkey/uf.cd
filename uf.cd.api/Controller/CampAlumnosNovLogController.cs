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
    public class CampAlumnosNovLogController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampAlumnosNovLogController> _logger;

        public CampAlumnosNovLogController(ExtranetContext context, IConfiguration configuration, ILogger<CampAlumnosNovLogController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampAlumnosNovLog
        // Obtiene todos los registros de log. ¡Considera paginación y filtrado para producción!
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampAlumnosNovLog>>> GetCampAlumnosNovLogs()
        {
            // Asumiendo que el DbSet se llama CampAlumnosNovLogs
            if (_context.CampAlumnosNovLogs == null)
            {
                return NotFound("La entidad 'CampAlumnosNovLogs' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los logs puede ser muy ineficiente.
            return await _context.CampAlumnosNovLogs.ToListAsync();
        }

        // GET: api/CampAlumnosNovLog/filter?idAlumno=X&idCarrera=Y&lu=Z
        // Obtiene logs filtrados por los parámetros proporcionados (opcionales)
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CampAlumnosNovLog>>> GetFilteredCampAlumnosNovLogs(
            [FromQuery] string? idAlumno,
            [FromQuery] int? idCarrera,
            [FromQuery] int? lu)
        {
             if (_context.CampAlumnosNovLogs == null)
            {
                return NotFound("La entidad 'CampAlumnosNovLogs' no está configurada en el DbContext.");
            }

            var query = _context.CampAlumnosNovLogs.AsQueryable();

            if (!string.IsNullOrEmpty(idAlumno))
            {
                query = query.Where(log => log.CanvIdAlumno == idAlumno);
            }
            if (idCarrera.HasValue)
            {
                query = query.Where(log => log.CanvIdCarrera == idCarrera.Value);
            }
             if (lu.HasValue)
            {
                query = query.Where(log => log.CanvLu == lu.Value);
            }
            // Puedes añadir más filtros aquí para otros campos (ej: por fecha)

            return await query.OrderByDescending(log => log.FechaProcesado).ToListAsync(); // Ejemplo: ordenar por fecha
        }


        // POST: api/CampAlumnosNovLog
        // Para crear un nuevo registro de log.
        // Nota: Generalmente, los logs no se actualizan ni eliminan vía API.
        [HttpPost]
        public async Task<ActionResult<CampAlumnosNovLog>> PostCampAlumnosNovLog(CampAlumnosNovLog campAlumnosNovLog)
        {
             if (_context.CampAlumnosNovLogs == null)
            {
                 return Problem("La entidad 'CampAlumnosNovLogs' no está configurada en el DbContext.");
            }

            // Opcional: Podrías establecer la fecha de procesado aquí si no viene en el request
            // if (!campAlumnosNovLog.FechaProcesado.HasValue)
            // {
            //     campAlumnosNovLog.FechaProcesado = DateTime.UtcNow;
            // }

            _context.CampAlumnosNovLogs.Add(campAlumnosNovLog);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar excepciones específicas si es necesario (ej: problemas de constraints)
                 // Devolver un Problem detalles puede ser útil para diagnóstico
                 return Problem($"Error al guardar el log en la base de datos: {ex.Message}");
            }

            // Devuelve 201 Created. Como no hay un GET específico por ID único,
            // no se usa CreatedAtAction. Se devuelve el objeto creado.
            return StatusCode(StatusCodes.Status201Created, campAlumnosNovLog);
        }

        // --- Métodos PUT y DELETE OMITIDOS ---
        // Generalmente no se implementan para tablas de log, ya que los logs
        // suelen ser inmutables una vez creados. Si necesitas modificarlos,
        // reconsidera si es realmente una tabla de log o requiere un enfoque diferente.

        // --- Método GET por ID OMITIDO ---
        // Debido a que no hay una clave primaria clara y única definida en el modelo
        // (todos los campos clave potenciales son nullable), no se puede implementar
        // de forma fiable un GET para obtener un registro *único* por ID.
        // Se proporciona el método GetFilteredCampAlumnosNovLogs en su lugar.

    }
}