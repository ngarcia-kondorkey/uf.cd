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
    public class CampCorrelativaController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampCorrelativaController> _logger;

        public CampCorrelativaController(ExtranetContext context, IConfiguration configuration, ILogger<CampCorrelativaController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampCorrelativa
        // Obtiene todos los registros de correlativas. ¡Considera paginación y filtrado para producción!
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampCorrelativa>>> GetCampCorrelativas() // Asumiendo DbSet se llama CampCorrelativas
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampCorrelativas == null)
            {
                return NotFound("La entidad 'CampCorrelativas' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todas las reglas puede ser ineficiente.
            return await _context.CampCorrelativas.ToListAsync();
        }

        // GET: api/CampCorrelativa/filter?idCarrera=X&idPlan=Y&idMateria=Z&idMateriaCorrelativa=W
        // Obtiene correlativas filtradas por los parámetros proporcionados (opcionales)
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CampCorrelativa>>> GetFilteredCampCorrelativas(
            [FromQuery] int? idCarrera,
            [FromQuery] int? idPlan,
            [FromQuery] int? idMateria,
            [FromQuery] int? idMateriaCorrelativa)
        {
             if (_context.CampCorrelativas == null)
            {
                return NotFound("La entidad 'CampCorrelativas' no está configurada en el DbContext.");
            }

            var query = _context.CampCorrelativas.AsQueryable();

            if (idCarrera.HasValue)
            {
                query = query.Where(c => c.CcorIdCarrera == idCarrera.Value);
            }
            if (idPlan.HasValue)
            {
                query = query.Where(c => c.CcorIdPlan == idPlan.Value);
            }
             if (idMateria.HasValue)
            {
                // Obtener reglas donde esta materia es la principal
                query = query.Where(c => c.CcorIdMateria == idMateria.Value);
            }
             if (idMateriaCorrelativa.HasValue)
            {
                 // Obtener reglas donde esta materia es la correlativa
                query = query.Where(c => c.CcorIdMateriaCorrelativa == idMateriaCorrelativa.Value);
            }
            // Puedes añadir más filtros aquí para otros campos (ej: por CcorCondicion)

            return await query.ToListAsync();
        }


        // POST: api/CampCorrelativa
        // Para crear una nueva regla de correlatividad.
        [HttpPost]
        public async Task<ActionResult<CampCorrelativa>> PostCampCorrelativa(CampCorrelativa campCorrelativa)
        {
             if (_context.CampCorrelativas == null)
            {
                 return Problem("La entidad 'CampCorrelativas' no está configurada en el DbContext.");
            }

            // Opcional: Validación para asegurar que los IDs necesarios no sean nulos
            if (!campCorrelativa.CcorIdCarrera.HasValue ||
                !campCorrelativa.CcorIdMateria.HasValue ||
                !campCorrelativa.CcorIdPlan.HasValue ||
                !campCorrelativa.CcorIdMateriaCorrelativa.HasValue)
            {
                return BadRequest("Los IDs de carrera, plan, materia y materia correlativa son requeridos.");
            }


            _context.CampCorrelativas.Add(campCorrelativa);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar excepciones específicas si es necesario
                 // (ej: problemas de FK si los IDs deben existir en otras tablas)
                 return Problem($"Error al guardar la correlativa en la base de datos: {ex.Message}");
            }

            // Devuelve 201 Created. Como no hay un GET específico por ID único fiable,
            // no se usa CreatedAtAction. Se devuelve el objeto creado.
            return StatusCode(StatusCodes.Status201Created, campCorrelativa);
        }

        // --- Métodos GET por ID, PUT y DELETE OMITIDOS ---
        // Debido a que todas las propiedades del modelo son nullable, no hay una
        // forma estándar y fiable de identificar un registro único para realizar
        // estas operaciones basadas en una clave primaria. La identificación
        // lógica de una regla de correlatividad podría requerir una combinación
        // de varios campos (ej: IdCarrera, IdPlan, IdMateria, IdMateriaCorrelativa),
        // pero la nulabilidad impide usar esto como PK directamente en EF Core
        // sin configuración avanzada o asunciones sobre los datos.

    }
}