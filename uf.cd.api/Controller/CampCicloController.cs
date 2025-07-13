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
    public class CampCicloController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampCicloController> _logger;

        public CampCicloController(ExtranetContext context, IConfiguration configuration, ILogger<CampCicloController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampCiclo
        // Obtiene todos los registros de ciclo. ¡Considera paginación y filtrado para producción!
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampCiclo>>> GetCampCiclos() // Asumiendo DbSet se llama CampCiclos
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampCiclos == null)
            {
                return NotFound("La entidad 'CampCiclos' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los ciclos puede ser ineficiente.
            return await _context.CampCiclos.ToListAsync();
        }

        // GET: api/CampCiclo/filter?idCarrera=X&idCurso=Y&idCiclo=Z&idMateria=W
        // Obtiene ciclos filtrados por los parámetros proporcionados (opcionales)
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CampCiclo>>> GetFilteredCampCiclos(
            [FromQuery] int? idCarrera,
            [FromQuery] int? idCurso,
            [FromQuery] int? idCiclo,
            [FromQuery] int? idMateria)
        {
             if (_context.CampCiclos == null)
            {
                return NotFound("La entidad 'CampCiclos' no está configurada en el DbContext.");
            }

            var query = _context.CampCiclos.AsQueryable();

            if (idCarrera.HasValue)
            {
                query = query.Where(c => c.CcicIdCarrera == idCarrera.Value);
            }
            if (idCurso.HasValue)
            {
                query = query.Where(c => c.CcicIdCurso == idCurso.Value);
            }
             if (idCiclo.HasValue)
            {
                query = query.Where(c => c.CcicIdCiclo == idCiclo.Value);
            }
             if (idMateria.HasValue)
            {
                query = query.Where(c => c.CcicIdMateria == idMateria.Value);
            }
            // Puedes añadir más filtros aquí para otros campos (ej: por descripción)

            return await query.ToListAsync();
        }


        // POST: api/CampCiclo
        // Para crear un nuevo registro de ciclo.
        // Nota: Generalmente, los ciclos podrían tener lógica de negocio asociada
        // o ser gestionados por otro proceso, pero aquí se permite la creación directa.
        [HttpPost]
        public async Task<ActionResult<CampCiclo>> PostCampCiclo(CampCiclo campCiclo)
        {
             if (_context.CampCiclos == null)
            {
                 return Problem("La entidad 'CampCiclos' no está configurada en el DbContext.");
            }

            _context.CampCiclos.Add(campCiclo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar excepciones específicas si es necesario
                 // (ej: problemas de constraints si alguno de los IDs debe existir en otra tabla)
                 return Problem($"Error al guardar el ciclo en la base de datos: {ex.Message}");
            }

            // Devuelve 201 Created. Como no hay un GET específico por ID único fiable,
            // no se usa CreatedAtAction. Se devuelve el objeto creado.
            return StatusCode(StatusCodes.Status201Created, campCiclo);
        }

        // --- Métodos GET por ID, PUT y DELETE OMITIDOS ---
        // Debido a que todas las propiedades del modelo son nullable, no hay una
        // forma estándar y fiable de identificar un registro único para realizar
        // estas operaciones basadas en una clave primaria.
        // Si se requiere actualizar o eliminar, se necesitaría una lógica
        // más compleja para identificar el registro correcto o un rediseño
        // del modelo/tabla para incluir una clave primaria no nullable.

    }
}