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
    public class CampMateriasACursarController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampMateriasACursarController> _logger;

        public CampMateriasACursarController(ExtranetContext context, IConfiguration configuration, ILogger<CampMateriasACursarController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampMateriasACursar
        // Obtiene todos los registros de materias a cursar. ¡Considera paginación y filtrado para producción!
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampMateriasACursar>>> GetCampMateriasACursarAll() // Renombrado para evitar colisión con clase
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampMateriasACursars == null)
            {
                return NotFound("La entidad 'CampMateriasACursar' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los registros puede ser muy ineficiente.
            return await _context.CampMateriasACursars.ToListAsync();
        }

        // GET: api/CampMateriasACursar/filter?idAlumno=X&idCarrera=Y&lu=Z&idPlan=W&idMateria=V&clMateria=U&anio=A
        // Obtiene registros filtrados por los parámetros proporcionados (opcionales)
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CampMateriasACursar>>> GetFilteredCampMateriasACursar(
            [FromQuery] string? idAlumno,
            [FromQuery] int? idCarrera,
            [FromQuery] int? lu,
            [FromQuery] int? idPlan,
            [FromQuery] int? idMateria,
            [FromQuery] string? clMateria,
            [FromQuery] short? anio)
        {
             if (_context.CampMateriasACursars == null)
            {
                return NotFound("La entidad 'CampMateriasACursar' no está configurada en el DbContext.");
            }

            var query = _context.CampMateriasACursars.AsQueryable();

            if (!string.IsNullOrEmpty(idAlumno))
            {
                query = query.Where(c => c.CmcuIdAlumno == idAlumno);
            }
            if (idCarrera.HasValue)
            {
                query = query.Where(c => c.CmcuIdCarrera == idCarrera.Value);
            }
             if (lu.HasValue)
            {
                query = query.Where(c => c.CmcuLu == lu.Value);
            }
             if (idPlan.HasValue)
            {
                query = query.Where(c => c.CmcuIdPlan == idPlan.Value);
            }
             if (idMateria.HasValue)
            {
                query = query.Where(c => c.CmcuIdMateria == idMateria.Value);
            }
            if (!string.IsNullOrEmpty(clMateria))
            {
                query = query.Where(c => c.CmcuClMateria == clMateria);
            }
             if (anio.HasValue)
            {
                query = query.Where(c => c.CmcuAnio == anio.Value);
            }
            // Puedes añadir más filtros aquí si es necesario

            return await query.ToListAsync();
        }

        // --- Métodos POST, PUT, DELETE y GET por ID OMITIDOS ---
        // Debido a que todas las propiedades del modelo son nullable, no hay una
        // forma estándar y fiable de identificar un registro único para realizar
        // operaciones como obtener por ID, actualizar o eliminar basadas en una
        // clave primaria. La lógica para determinar qué materias puede cursar un
        // alumno probablemente resida en otro lugar (ej: basada en correlativas y plan).

    }
}