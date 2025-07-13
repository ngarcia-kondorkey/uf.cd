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
    public class CampFinalesAlumnoController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampFinalesAlumnoController> _logger;

        public CampFinalesAlumnoController(ExtranetContext context, IConfiguration configuration, ILogger<CampFinalesAlumnoController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampFinalesAlumno
        // Obtiene todos los registros de finales por alumno. ¡Considera paginación y filtrado para producción!
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampFinalesAlumno>>> GetCampFinalesAlumnos()
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampFinalesAlumnos == null)
            {
                return NotFound("La entidad 'CampFinalesAlumnos' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los registros puede ser muy ineficiente.
            return await _context.CampFinalesAlumnos.ToListAsync();
        }

        // GET: api/CampFinalesAlumno/filter?idAlumno=X&idCarrera=Y&lu=Z&idMateria=W
        // Obtiene registros filtrados por los parámetros proporcionados (opcionales)
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CampFinalesAlumno>>> GetFilteredCampFinalesAlumnos(
            [FromQuery] string? idAlumno,
            [FromQuery] int? idCarrera,
            [FromQuery] int? lu,
            [FromQuery] int? idMateria)
        {
             if (_context.CampFinalesAlumnos == null)
            {
                return NotFound("La entidad 'CampFinalesAlumnos' no está configurada en el DbContext.");
            }

            var query = _context.CampFinalesAlumnos.AsQueryable();

            if (!string.IsNullOrEmpty(idAlumno))
            {
                query = query.Where(c => c.CfalIdAlumno == idAlumno);
            }
            if (idCarrera.HasValue)
            {
                query = query.Where(c => c.CfalIdCarrera == idCarrera.Value);
            }
             if (lu.HasValue)
            {
                query = query.Where(c => c.CfalLu == lu.Value);
            }
             if (idMateria.HasValue)
            {
                query = query.Where(c => c.CfalIdMateria == idMateria.Value);
            }
            // Puedes añadir más filtros aquí si es necesario

            return await query.ToListAsync();
        }

        // --- Métodos POST, PUT, DELETE y GET por ID OMITIDOS ---
        // Debido a que todas las propiedades del modelo son nullable, no hay una
        // forma estándar y fiable de identificar un registro único para realizar
        // operaciones como obtener por ID, actualizar o eliminar basadas en una
        // clave primaria. La lógica para manipular estos datos probablemente
        // reside en otro lugar o requeriría identificar los registros de forma
        // más compleja (quizás buscando por todos los campos no nulos esperados).

    }
}