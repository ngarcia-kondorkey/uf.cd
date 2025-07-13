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
    public class CampPlanAlumnoController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampPlanAlumnoController> _logger;

        public CampPlanAlumnoController(ExtranetContext context, IConfiguration configuration, ILogger<CampPlanAlumnoController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampPlanAlumno
        // Obtiene todos los registros de plan de alumno.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampPlanAlumno>>> GetCampPlanesAlumnos()
        {
            if (_context.CampPlanAlumnos == null)
            {
                return NotFound("La entidad 'CampPlanesAlumnos' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los registros puede ser ineficiente.
            // Considera paginación y filtrado (ej: por alumno, por plan).
            return await _context.CampPlanAlumnos.ToListAsync();
        }

        // GET: api/CampPlanAlumno/{idAlumno}/{idCarrera}/{idMateria}/{idPl}/{lu}
        // Busca un registro específico por la clave primaria compuesta asumida.
        [HttpGet("{idAlumno}/{idCarrera}/{idMateria}/{idPl}/{lu}")]
        public async Task<ActionResult<CampPlanAlumno>> GetCampPlanAlumno(string idAlumno, int idCarrera, int idMateria, int idPl, int lu)
        {
            if (_context.CampPlanAlumnos == null)
            {
                 return NotFound("La entidad 'CampPlanesAlumnos' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campPlanAlumno = await _context.CampPlanAlumnos.FindAsync(idAlumno, idCarrera, idMateria, idPl, lu);

            if (campPlanAlumno == null)
            {
                return NotFound();
            }

            return campPlanAlumno;
        }

        // PUT: api/CampPlanAlumno/{idAlumno}/{idCarrera}/{idMateria}/{idPl}/{lu}
        // Para actualizar un registro existente usando la clave compuesta.
        [HttpPut("{idAlumno}/{idCarrera}/{idMateria}/{idPl}/{lu}")]
        public async Task<IActionResult> PutCampPlanAlumno(string idAlumno, int idCarrera, int idMateria, int idPl, int lu, CampPlanAlumno campPlanAlumno)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campPlanAlumno.CpalIdAlumno ||
                idCarrera != campPlanAlumno.CpalIdCarrera ||
                idMateria != campPlanAlumno.CpalIdMateria ||
                idPl != campPlanAlumno.CpalIdPl ||
                lu != campPlanAlumno.CpalLu)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampPlanAlumnos == null)
            {
                 return NotFound("La entidad 'CampPlanesAlumnos' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campPlanAlumno).State = EntityState.Modified;
            _context.Entry(campPlanAlumno).Property(x => x.CpalIdAlumno).IsModified = false; // Previene modificación PK
            _context.Entry(campPlanAlumno).Property(x => x.CpalIdCarrera).IsModified = false; // Previene modificación PK
            _context.Entry(campPlanAlumno).Property(x => x.CpalIdMateria).IsModified = false; // Previene modificación PK
            _context.Entry(campPlanAlumno).Property(x => x.CpalIdPl).IsModified = false; // Previene modificación PK
            _context.Entry(campPlanAlumno).Property(x => x.CpalLu).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampPlanAlumnoExists(idAlumno, idCarrera, idMateria, idPl, lu))
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
                 return BadRequest($"No se puede modificar la clave primaria. Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampPlanAlumno
        // Para crear un nuevo registro de plan de alumno.
        [HttpPost]
        public async Task<ActionResult<CampPlanAlumno>> PostCampPlanAlumno(CampPlanAlumno campPlanAlumno)
        {
             if (_context.CampPlanAlumnos == null)
            {
                 return Problem("La entidad 'CampPlanesAlumnos' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(campPlanAlumno.CpalIdAlumno))
             {
                 return BadRequest("El IdAlumno es requerido.");
             }

            _context.CampPlanAlumnos.Add(campPlanAlumno);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampPlanAlumnoExists(campPlanAlumno.CpalIdAlumno, campPlanAlumno.CpalIdCarrera, campPlanAlumno.CpalIdMateria, campPlanAlumno.CpalIdPl, campPlanAlumno.CpalLu))
                {
                    return Conflict("Ya existe un registro en el plan para este alumno/materia/plan.");
                }
                else
                {
                    // Podría ser un error de FK si el alumno, carrera, materia o plan no existen
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampPlanAlumno), new {
                 idAlumno = campPlanAlumno.CpalIdAlumno,
                 idCarrera = campPlanAlumno.CpalIdCarrera,
                 idMateria = campPlanAlumno.CpalIdMateria,
                 idPl = campPlanAlumno.CpalIdPl,
                 lu = campPlanAlumno.CpalLu
                 }, campPlanAlumno);
        }

        // DELETE: api/CampPlanAlumno/{idAlumno}/{idCarrera}/{idMateria}/{idPl}/{lu}
        // Elimina un registro de plan de alumno.
        [HttpDelete("{idAlumno}/{idCarrera}/{idMateria}/{idPl}/{lu}")]
        public async Task<IActionResult> DeleteCampPlanAlumno(string idAlumno, int idCarrera, int idMateria, int idPl, int lu)
        {
             if (_context.CampPlanAlumnos == null)
            {
                 return NotFound("La entidad 'CampPlanesAlumnos' no está configurada en el DbContext.");
            }

            var campPlanAlumno = await _context.CampPlanAlumnos.FindAsync(idAlumno, idCarrera, idMateria, idPl, lu);
            if (campPlanAlumno == null)
            {
                return NotFound();
            }

            _context.CampPlanAlumnos.Remove(campPlanAlumno);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampPlanAlumnoExists(string idAlumno, int idCarrera, int idMateria, int idPl, int lu)
        {
            return (_context.CampPlanAlumnos?.Any(e =>
                e.CpalIdAlumno == idAlumno &&
                e.CpalIdCarrera == idCarrera &&
                e.CpalIdMateria == idMateria &&
                e.CpalIdPl == idPl &&
                e.CpalLu == lu)).GetValueOrDefault();
        }
    }
}