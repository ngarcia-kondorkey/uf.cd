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
    public class CampFinalesFechaController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampFinalesFechaController> _logger;

        public CampFinalesFechaController(ExtranetContext context, IConfiguration configuration, ILogger<CampFinalesFechaController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampFinalesFecha
        // Obtiene todas las fechas de finales.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampFinalesFecha>>> GetCampFinalesFechas()
        {
            if (_context.CampFinalesFechas == null)
            {
                return NotFound("La entidad 'CampFinalesFechas' no está configurada en el DbContext.");
            }
            // Considera ordenar o filtrar si la lista es muy grande.
            return await _context.CampFinalesFechas.ToListAsync();
        }

        // GET: api/CampFinalesFecha/{idExamen}/{idPeriodo}/{idLlamado}
        // Busca una fecha de final específica por su clave primaria compuesta asumida.
        [HttpGet("{idExamen}/{idPeriodo}/{idLlamado}")]
        public async Task<ActionResult<CampFinalesFecha>> GetCampFinalesFecha(int idExamen, int idPeriodo, int idLlamado)
        {
            if (_context.CampFinalesFechas == null)
            {
                 return NotFound("La entidad 'CampFinalesFechas' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campFinalesFecha = await _context.CampFinalesFechas.FindAsync(idExamen, idPeriodo, idLlamado);

            if (campFinalesFecha == null)
            {
                return NotFound();
            }

            return campFinalesFecha;
        }

        // PUT: api/CampFinalesFecha/{idExamen}/{idPeriodo}/{idLlamado}
        // Para actualizar una fecha de final existente.
        [HttpPut("{idExamen}/{idPeriodo}/{idLlamado}")]
        public async Task<IActionResult> PutCampFinalesFecha(int idExamen, int idPeriodo, int idLlamado, CampFinalesFecha campFinalesFecha)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idExamen != campFinalesFecha.CfifIdExamen ||
                idPeriodo != campFinalesFecha.CfifIdPeriodo ||
                idLlamado != campFinalesFecha.CfifIdLlamado)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampFinalesFechas == null)
            {
                 return NotFound("La entidad 'CampFinalesFechas' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campFinalesFecha).State = EntityState.Modified;
            _context.Entry(campFinalesFecha).Property(x => x.CfifIdExamen).IsModified = false; // Previene modificación PK
            _context.Entry(campFinalesFecha).Property(x => x.CfifIdPeriodo).IsModified = false; // Previene modificación PK
            _context.Entry(campFinalesFecha).Property(x => x.CfifIdLlamado).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampFinalesFechaExists(idExamen, idPeriodo, idLlamado))
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

        // POST: api/CampFinalesFecha
        // Para crear una nueva fecha de final.
        [HttpPost]
        public async Task<ActionResult<CampFinalesFecha>> PostCampFinalesFecha(CampFinalesFecha campFinalesFecha)
        {
             if (_context.CampFinalesFechas == null)
            {
                 return Problem("La entidad 'CampFinalesFechas' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales (ej: fecha no nula si es requerida por lógica)
             // if(!campFinalesFecha.CfifFecha.HasValue) {
             //     return BadRequest("La fecha del final es requerida.");
             // }

            _context.CampFinalesFechas.Add(campFinalesFecha);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampFinalesFechaExists(campFinalesFecha.CfifIdExamen, campFinalesFecha.CfifIdPeriodo, campFinalesFecha.CfifIdLlamado))
                {
                    return Conflict("Ya existe una fecha de final con esta clave (Examen, Periodo, Llamado).");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampFinalesFecha), new {
                idExamen = campFinalesFecha.CfifIdExamen,
                idPeriodo = campFinalesFecha.CfifIdPeriodo,
                idLlamado = campFinalesFecha.CfifIdLlamado
                }, campFinalesFecha);
        }

        // DELETE: api/CampFinalesFecha/{idExamen}/{idPeriodo}/{idLlamado}
        // Elimina una fecha de final.
        // Nota: Podría fallar si existen inscripciones de alumnos (u otros datos) asociados a esta fecha.
        [HttpDelete("{idExamen}/{idPeriodo}/{idLlamado}")]
        public async Task<IActionResult> DeleteCampFinalesFecha(int idExamen, int idPeriodo, int idLlamado)
        {
             if (_context.CampFinalesFechas == null)
            {
                 return NotFound("La entidad 'CampFinalesFechas' no está configurada en el DbContext.");
            }

            var campFinalesFecha = await _context.CampFinalesFechas.FindAsync(idExamen, idPeriodo, idLlamado);
            if (campFinalesFecha == null)
            {
                return NotFound();
            }

            _context.CampFinalesFechas.Remove(campFinalesFecha);
             try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                  return Problem($"No se pudo eliminar la fecha de final. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampFinalesFechaExists(int idExamen, int idPeriodo, int idLlamado)
        {
            return (_context.CampFinalesFechas?.Any(e =>
                e.CfifIdExamen == idExamen &&
                e.CfifIdPeriodo == idPeriodo &&
                e.CfifIdLlamado == idLlamado)).GetValueOrDefault();
        }
    }
}