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
    public class CampParcialNotaController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampParcialNotaController> _logger;

        public CampParcialNotaController(ExtranetContext context, IConfiguration configuration, ILogger<CampParcialNotaController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampParcialNota
        // Obtiene todas las notas de parciales.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampParcialNota>>> GetCampParcialesNotas()
        {
            if (_context.CampParcialNotas == null)
            {
                return NotFound("La entidad 'CampParcialesNotas' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todas las notas puede ser ineficiente.
             // Considera paginación y filtrado (ej: por alumno, por materia).
            return await _context.CampParcialNotas.ToListAsync();
        }

        // GET: api/CampParcialNota/{idExamen}/{lu}/{idCarrera}
        // Busca una nota específica por su clave primaria compuesta asumida.
        [HttpGet("{idExamen}/{lu}/{idCarrera}")]
        public async Task<ActionResult<CampParcialNota>> GetCampParcialNota(int idExamen, int lu, int idCarrera)
        {
            if (_context.CampParcialNotas == null)
            {
                 return NotFound("La entidad 'CampParcialesNotas' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campParcialNota = await _context.CampParcialNotas.FindAsync(idExamen, lu, idCarrera);

            if (campParcialNota == null)
            {
                return NotFound();
            }

            return campParcialNota;
        }

        // PUT: api/CampParcialNota/{idExamen}/{lu}/{idCarrera}
        // Para actualizar una nota de parcial existente usando la clave compuesta.
        [HttpPut("{idExamen}/{lu}/{idCarrera}")]
        public async Task<IActionResult> PutCampParcialNota(int idExamen, int lu, int idCarrera, CampParcialNota campParcialNota)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idExamen != campParcialNota.CpnoIdExamen ||
                lu != campParcialNota.CpnoLu ||
                idCarrera != campParcialNota.CpnoIdCarrera)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampParcialNotas == null)
            {
                 return NotFound("La entidad 'CampParcialesNotas' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campParcialNota).State = EntityState.Modified;
            _context.Entry(campParcialNota).Property(x => x.CpnoIdExamen).IsModified = false; // Previene modificación PK
            _context.Entry(campParcialNota).Property(x => x.CpnoLu).IsModified = false; // Previene modificación PK
            _context.Entry(campParcialNota).Property(x => x.CpnoIdCarrera).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampParcialNotaExists(idExamen, lu, idCarrera))
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

        // POST: api/CampParcialNota
        // Para crear un nuevo registro de nota de parcial.
        [HttpPost]
        public async Task<ActionResult<CampParcialNota>> PostCampParcialNota(CampParcialNota campParcialNota)
        {
             if (_context.CampParcialNotas == null)
            {
                 return Problem("La entidad 'CampParcialesNotas' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales (ej: nota dentro de rango válido)
             // if(campParcialNota.CpnoNota < 0 || campParcialNota.CpnoNota > 10) { // Ajustar rango según corresponda
             //     return BadRequest("La nota no es válida.");
             // }

            _context.CampParcialNotas.Add(campParcialNota);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampParcialNotaExists(campParcialNota.CpnoIdExamen, campParcialNota.CpnoLu, campParcialNota.CpnoIdCarrera))
                {
                    return Conflict("Ya existe una nota para este alumno en este examen.");
                }
                else
                {
                    // Podría ser un error de FK si el alumno, carrera, examen, etc., no existen
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampParcialNota), new {
                 idExamen = campParcialNota.CpnoIdExamen,
                 lu = campParcialNota.CpnoLu,
                 idCarrera = campParcialNota.CpnoIdCarrera
                 }, campParcialNota);
        }

        // DELETE: api/CampParcialNota/{idExamen}/{lu}/{idCarrera}
        // Elimina una nota de parcial.
        [HttpDelete("{idExamen}/{lu}/{idCarrera}")]
        public async Task<IActionResult> DeleteCampParcialNota(int idExamen, int lu, int idCarrera)
        {
             if (_context.CampParcialNotas == null)
            {
                 return NotFound("La entidad 'CampParcialesNotas' no está configurada en el DbContext.");
            }

            var campParcialNota = await _context.CampParcialNotas.FindAsync(idExamen, lu, idCarrera);
            if (campParcialNota == null)
            {
                return NotFound();
            }

            _context.CampParcialNotas.Remove(campParcialNota);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampParcialNotaExists(int idExamen, int lu, int idCarrera)
        {
            return (_context.CampParcialNotas?.Any(e =>
                e.CpnoIdExamen == idExamen &&
                e.CpnoLu == lu &&
                e.CpnoIdCarrera == idCarrera)).GetValueOrDefault();
        }
    }
}