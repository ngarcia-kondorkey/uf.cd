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
    public class CampFinalesIncripcionController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampFinalesIncripcionController> _logger;

        public CampFinalesIncripcionController(ExtranetContext context, IConfiguration configuration, ILogger<CampFinalesIncripcionController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampFinalesIncripcion
        // Obtiene todas las inscripciones a finales.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampFinalesIncripcion>>> GetCampFinalesIncripciones()
        {
            if (_context.CampFinalesIncripcions == null)
            {
                return NotFound("La entidad 'CampFinalesIncripciones' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todas las inscripciones puede ser ineficiente.
            // Considera paginación y filtrado (ej: por alumno, por examen).
            return await _context.CampFinalesIncripcions.ToListAsync();
        }

        // GET: api/CampFinalesIncripcion/{idExamen}/{idPeriodo}/{idLlamado}/{idAlumno}/{idLu}
        // Busca una inscripción específica por su clave primaria compuesta asumida.
        [HttpGet("{idExamen}/{idPeriodo}/{idLlamado}/{idAlumno}/{idLu}")]
        public async Task<ActionResult<CampFinalesIncripcion>> GetCampFinalesIncripcion(int idExamen, int idPeriodo, int idLlamado, string idAlumno, int idLu)
        {
            if (_context.CampFinalesIncripcions == null)
            {
                 return NotFound("La entidad 'CampFinalesIncripciones' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campFinalesIncripcion = await _context.CampFinalesIncripcions.FindAsync(idExamen, idPeriodo, idLlamado, idAlumno, idLu);

            if (campFinalesIncripcion == null)
            {
                return NotFound();
            }

            return campFinalesIncripcion;
        }

        // PUT: api/CampFinalesIncripcion/{idExamen}/{idPeriodo}/{idLlamado}/{idAlumno}/{idLu}
        // Para actualizar una inscripción existente (ej: cambiar estado administrativo).
        [HttpPut("{idExamen}/{idPeriodo}/{idLlamado}/{idAlumno}/{idLu}")]
        public async Task<IActionResult> PutCampFinalesIncripcion(int idExamen, int idPeriodo, int idLlamado, string idAlumno, int idLu, CampFinalesIncripcion campFinalesIncripcion)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idExamen != campFinalesIncripcion.CfiiIdExamen ||
                idPeriodo != campFinalesIncripcion.CfiiIdPeriodo ||
                idLlamado != campFinalesIncripcion.CfiiIdLlamado ||
                idAlumno != campFinalesIncripcion.CfiiIdAlumno ||
                idLu != campFinalesIncripcion.CfiiIdLu)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampFinalesIncripcions == null)
            {
                 return NotFound("La entidad 'CampFinalesIncripciones' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campFinalesIncripcion).State = EntityState.Modified;
            _context.Entry(campFinalesIncripcion).Property(x => x.CfiiIdExamen).IsModified = false; // Previene modificación PK
            _context.Entry(campFinalesIncripcion).Property(x => x.CfiiIdPeriodo).IsModified = false; // Previene modificación PK
            _context.Entry(campFinalesIncripcion).Property(x => x.CfiiIdLlamado).IsModified = false; // Previene modificación PK
            _context.Entry(campFinalesIncripcion).Property(x => x.CfiiIdAlumno).IsModified = false; // Previene modificación PK
            _context.Entry(campFinalesIncripcion).Property(x => x.CfiiIdLu).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampFinalesIncripcionExists(idExamen, idPeriodo, idLlamado, idAlumno, idLu))
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

        // POST: api/CampFinalesIncripcion
        // Para crear una nueva inscripción a final.
        [HttpPost]
        public async Task<ActionResult<CampFinalesIncripcion>> PostCampFinalesIncripcion(CampFinalesIncripcion campFinalesIncripcion)
        {
             if (_context.CampFinalesIncripcions == null)
            {
                 return Problem("La entidad 'CampFinalesIncripciones' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales (ej: el alumno existe, el final existe, está en período válido)
             // if(!AlumnoExists(campFinalesIncripcion.CfiiIdAlumno, campFinalesIncripcion.CfiiIdLu)) return BadRequest("Alumno no válido");
             // if(!FinalExiste(campFinalesIncripcion.CfiiIdExamen, ...)) return BadRequest("Final no válido");

            // Establecer fecha de inscripción si no viene
             if(!campFinalesIncripcion.CfiiFechaIncripcion.HasValue)
             {
                 campFinalesIncripcion.CfiiFechaIncripcion = DateTime.UtcNow; // O DateTime.Now dependiendo del requerimiento de zona horaria
             }

            _context.CampFinalesIncripcions.Add(campFinalesIncripcion);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampFinalesIncripcionExists(campFinalesIncripcion.CfiiIdExamen, campFinalesIncripcion.CfiiIdPeriodo, campFinalesIncripcion.CfiiIdLlamado, campFinalesIncripcion.CfiiIdAlumno, campFinalesIncripcion.CfiiIdLu))
                {
                    return Conflict("El alumno ya está inscripto a este final/llamado.");
                }
                else
                {
                    // Podría ser un error de FK si el alumno o el final no existen
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampFinalesIncripcion), new {
                idExamen = campFinalesIncripcion.CfiiIdExamen,
                idPeriodo = campFinalesIncripcion.CfiiIdPeriodo,
                idLlamado = campFinalesIncripcion.CfiiIdLlamado,
                idAlumno = campFinalesIncripcion.CfiiIdAlumno,
                idLu = campFinalesIncripcion.CfiiIdLu
                }, campFinalesIncripcion);
        }

        // DELETE: api/CampFinalesIncripcion/{idExamen}/{idPeriodo}/{idLlamado}/{idAlumno}/{idLu}
        // Elimina (cancela) una inscripción a final.
        [HttpDelete("{idExamen}/{idPeriodo}/{idLlamado}/{idAlumno}/{idLu}")]
        public async Task<IActionResult> DeleteCampFinalesIncripcion(int idExamen, int idPeriodo, int idLlamado, string idAlumno, int idLu)
        {
             if (_context.CampFinalesIncripcions == null)
            {
                 return NotFound("La entidad 'CampFinalesIncripciones' no está configurada en el DbContext.");
            }

            var campFinalesIncripcion = await _context.CampFinalesIncripcions.FindAsync(idExamen, idPeriodo, idLlamado, idAlumno, idLu);
            if (campFinalesIncripcion == null)
            {
                return NotFound();
            }

            _context.CampFinalesIncripcions.Remove(campFinalesIncripcion);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampFinalesIncripcionExists(int idExamen, int idPeriodo, int idLlamado, string idAlumno, int idLu)
        {
            return (_context.CampFinalesIncripcions?.Any(e =>
                e.CfiiIdExamen == idExamen &&
                e.CfiiIdPeriodo == idPeriodo &&
                e.CfiiIdLlamado == idLlamado &&
                e.CfiiIdAlumno == idAlumno &&
                e.CfiiIdLu == idLu)).GetValueOrDefault();
        }
    }
}