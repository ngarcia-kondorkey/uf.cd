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
    public class CampPresentismoController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampPresentismoController> _logger;

        public CampPresentismoController(ExtranetContext context, IConfiguration configuration, ILogger<CampPresentismoController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampPresentismo
        // Obtiene todos los registros de presentismo.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampPresentismo>>> GetCampPresentismos()
        {
            if (_context.CampPresentismos == null)
            {
                return NotFound("La entidad 'CampPresentismos' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todos los registros puede ser ineficiente.
             // Considera paginación y filtrado (ej: por alumno, ciclo, asignatura).
            return await _context.CampPresentismos.ToListAsync();
        }

        // GET: api/CampPresentismo/{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}
        // Busca un registro específico por la clave primaria compuesta asumida.
        // Nota: Considera codificar 'asignatura' y 'ciclo' si pueden contener caracteres especiales para URL.
        [HttpGet("{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}")]
        public async Task<ActionResult<CampPresentismo>> GetCampPresentismo(string idAlumno, int idCarrera, int lu, string asignatura, string ciclo)
        {
            if (_context.CampPresentismos == null)
            {
                 return NotFound("La entidad 'CampPresentismos' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campPresentismo = await _context.CampPresentismos.FindAsync(idAlumno, idCarrera, lu, asignatura, ciclo);

            if (campPresentismo == null)
            {
                return NotFound();
            }

            return campPresentismo;
        }

        // PUT: api/CampPresentismo/{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}
        // Para actualizar un registro de presentismo existente usando la clave compuesta.
        [HttpPut("{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}")]
        public async Task<IActionResult> PutCampPresentismo(string idAlumno, int idCarrera, int lu, string asignatura, string ciclo, CampPresentismo campPresentismo)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campPresentismo.CpreIdAlumno ||
                idCarrera != campPresentismo.CpreIdCarrera ||
                lu != campPresentismo.CpreLu ||
                asignatura != campPresentismo.CpreAsignatura ||
                ciclo != campPresentismo.CpreCiclo)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampPresentismos == null)
            {
                 return NotFound("La entidad 'CampPresentismos' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campPresentismo).State = EntityState.Modified;
            _context.Entry(campPresentismo).Property(x => x.CpreIdAlumno).IsModified = false; // Previene modificación PK
            _context.Entry(campPresentismo).Property(x => x.CpreIdCarrera).IsModified = false; // Previene modificación PK
            _context.Entry(campPresentismo).Property(x => x.CpreLu).IsModified = false; // Previene modificación PK
            _context.Entry(campPresentismo).Property(x => x.CpreAsignatura).IsModified = false; // Previene modificación PK
            _context.Entry(campPresentismo).Property(x => x.CpreCiclo).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampPresentismoExists(idAlumno, idCarrera, lu, asignatura, ciclo))
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

        // POST: api/CampPresentismo
        // Para crear un nuevo registro de presentismo.
        // Nota: Usualmente estos datos son calculados o generados internamente.
        [HttpPost]
        public async Task<ActionResult<CampPresentismo>> PostCampPresentismo(CampPresentismo campPresentismo)
        {
             if (_context.CampPresentismos == null)
            {
                 return Problem("La entidad 'CampPresentismos' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(campPresentismo.CpreIdAlumno) ||
                string.IsNullOrWhiteSpace(campPresentismo.CpreAsignatura) ||
                string.IsNullOrWhiteSpace(campPresentismo.CpreCiclo))
             {
                 return BadRequest("IdAlumno, Asignatura y Ciclo son requeridos.");
             }
             // Validar que los contadores no sean negativos, etc.

            _context.CampPresentismos.Add(campPresentismo);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampPresentismoExists(campPresentismo.CpreIdAlumno, campPresentismo.CpreIdCarrera, campPresentismo.CpreLu, campPresentismo.CpreAsignatura, campPresentismo.CpreCiclo))
                {
                    return Conflict("Ya existe un registro de presentismo para este alumno/asignatura/ciclo.");
                }
                else
                {
                    // Podría ser un error de FK si el alumno, carrera, asignatura, etc., no existen
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampPresentismo), new {
                 idAlumno = campPresentismo.CpreIdAlumno,
                 idCarrera = campPresentismo.CpreIdCarrera,
                 lu = campPresentismo.CpreLu,
                 asignatura = campPresentismo.CpreAsignatura, // Considera URL encoding
                 ciclo = campPresentismo.CpreCiclo // Considera URL encoding
                 }, campPresentismo);
        }

        // DELETE: api/CampPresentismo/{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}
        // Elimina un registro de presentismo.
        // Nota: Puede no ser una operación común si los datos se recalculan.
        [HttpDelete("{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}")]
        public async Task<IActionResult> DeleteCampPresentismo(string idAlumno, int idCarrera, int lu, string asignatura, string ciclo)
        {
             if (_context.CampPresentismos == null)
            {
                 return NotFound("La entidad 'CampPresentismos' no está configurada en el DbContext.");
            }

            var campPresentismo = await _context.CampPresentismos.FindAsync(idAlumno, idCarrera, lu, asignatura, ciclo);
            if (campPresentismo == null)
            {
                return NotFound();
            }

            _context.CampPresentismos.Remove(campPresentismo);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampPresentismoExists(string idAlumno, int idCarrera, int lu, string asignatura, string ciclo)
        {
            return (_context.CampPresentismos?.Any(e =>
                e.CpreIdAlumno == idAlumno &&
                e.CpreIdCarrera == idCarrera &&
                e.CpreLu == lu &&
                e.CpreAsignatura == asignatura &&
                e.CpreCiclo == ciclo)).GetValueOrDefault();
        }
    }
}