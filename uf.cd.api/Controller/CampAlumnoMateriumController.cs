using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uf.cd.db.Model; // Asegúrate que este namespace sea correcto

namespace uf.cd.api.Controllers 
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampAlumnoMateriumController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampAlumnoMateriumController> _logger;

        public CampAlumnoMateriumController(ExtranetContext context, IConfiguration configuration, ILogger<CampAlumnoMateriumController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampAlumnoMaterium
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampAlumnoMaterium>>> GetCampAlumnoMateria() // Asumiendo que el DbSet se llama CampAlumnoMateria
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampAlumnoMateria == null)
            {
                return NotFound("La entidad 'CampAlumnoMateria' no está configurada en el DbContext.");
            }
            // Nota: Devolver toda la tabla puede ser ineficiente para tablas grandes. Considera paginación.
            return await _context.CampAlumnoMateria.ToListAsync();
        }

        // GET: api/CampAlumnoMaterium/{idAlumno}/{idCarrera}/{lu}/{idMateria}
        // Busca por la clave primaria compuesta
        [HttpGet("{idAlumno}/{idCarrera}/{lu}/{idMateria}")]
        public async Task<ActionResult<CampAlumnoMaterium>> GetCampAlumnoMaterium(string idAlumno, int idCarrera, int lu, int idMateria)
        {
            if (_context.CampAlumnoMateria == null)
            {
                 return NotFound("La entidad 'CampAlumnoMateria' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            var campAlumnoMaterium = await _context.CampAlumnoMateria.FindAsync(idAlumno, idCarrera, lu, idMateria);

            if (campAlumnoMaterium == null)
            {
                return NotFound();
            }

            return campAlumnoMaterium;
        }

        // PUT: api/CampAlumnoMaterium/{idAlumno}/{idCarrera}/{lu}/{idMateria}
        // Para actualizar un registro existente usando la clave compuesta
        [HttpPut("{idAlumno}/{idCarrera}/{lu}/{idMateria}")]
        public async Task<IActionResult> PutCampAlumnoMaterium(string idAlumno, int idCarrera, int lu, int idMateria, CampAlumnoMaterium campAlumnoMaterium)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campAlumnoMaterium.CamaIdAlumno ||
                idCarrera != campAlumnoMaterium.CamaIdCarrera ||
                lu != campAlumnoMaterium.CamaLu ||
                idMateria != campAlumnoMaterium.CamaIdMateria)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampAlumnoMateria == null)
            {
                 return NotFound("La entidad 'CampAlumnoMateria' no está configurada en el DbContext.");
            }

            _context.Entry(campAlumnoMaterium).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampAlumnoMateriumExists(idAlumno, idCarrera, lu, idMateria))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampAlumnoMaterium
        // Para crear un nuevo registro
        [HttpPost]
        public async Task<ActionResult<CampAlumnoMaterium>> PostCampAlumnoMaterium(CampAlumnoMaterium campAlumnoMaterium)
        {
             if (_context.CampAlumnoMateria == null)
            {
                 return Problem("La entidad 'CampAlumnoMateria' no está configurada en el DbContext.");
            }

            _context.CampAlumnoMateria.Add(campAlumnoMaterium);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampAlumnoMateriumExists(campAlumnoMaterium.CamaIdAlumno, campAlumnoMaterium.CamaIdCarrera, campAlumnoMaterium.CamaLu, campAlumnoMaterium.CamaIdMateria))
                {
                    return Conflict("Ya existe un registro con esta clave compuesta.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampAlumnoMaterium), new {
                idAlumno = campAlumnoMaterium.CamaIdAlumno,
                idCarrera = campAlumnoMaterium.CamaIdCarrera,
                lu = campAlumnoMaterium.CamaLu,
                idMateria = campAlumnoMaterium.CamaIdMateria
                }, campAlumnoMaterium);
        }

        // DELETE: api/CampAlumnoMaterium/{idAlumno}/{idCarrera}/{lu}/{idMateria}
        // Elimina por clave primaria compuesta
        [HttpDelete("{idAlumno}/{idCarrera}/{lu}/{idMateria}")]
        public async Task<IActionResult> DeleteCampAlumnoMaterium(string idAlumno, int idCarrera, int lu, int idMateria)
        {
             if (_context.CampAlumnoMateria == null)
            {
                 return NotFound("La entidad 'CampAlumnoMateria' no está configurada en el DbContext.");
            }

            var campAlumnoMaterium = await _context.CampAlumnoMateria.FindAsync(idAlumno, idCarrera, lu, idMateria);
            if (campAlumnoMaterium == null)
            {
                return NotFound();
            }

            _context.CampAlumnoMateria.Remove(campAlumnoMaterium);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta
        private bool CampAlumnoMateriumExists(string idAlumno, int idCarrera, int lu, int idMateria)
        {
            // Asegúrate que CampAlumnoMateria no sea null antes de usarlo
            return (_context.CampAlumnoMateria?.Any(e =>
                e.CamaIdAlumno == idAlumno &&
                e.CamaIdCarrera == idCarrera &&
                e.CamaLu == lu &&
                e.CamaIdMateria == idMateria)).GetValueOrDefault();
        }
    }
}