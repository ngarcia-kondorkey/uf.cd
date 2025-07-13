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
    public class CampMensajesCarreraController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampMensajesCarreraController> _logger;

        public CampMensajesCarreraController(ExtranetContext context, IConfiguration configuration, ILogger<CampMensajesCarreraController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampMensajesCarrera
        // Obtiene todos los mensajes de carrera.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampMensajesCarrera>>> GetCampMensajesCarreras()
        {
            if (_context.CampMensajesCarreras == null)
            {
                return NotFound("La entidad 'CampMensajesCarreras' no está configurada en el DbContext.");
            }
            // Considera aplicar filtros por defecto (ej: mensajes activos por fecha) o paginación.
            return await _context.CampMensajesCarreras.ToListAsync();
        }

        // GET: api/CampMensajesCarrera/{idMensaje}/{idCarrera}
        // Busca un mensaje específico por su clave primaria compuesta asumida.
        [HttpGet("{idMensaje}/{idCarrera}")]
        public async Task<ActionResult<CampMensajesCarrera>> GetCampMensajesCarrera(int idMensaje, int idCarrera)
        {
            if (_context.CampMensajesCarreras == null)
            {
                 return NotFound("La entidad 'CampMensajesCarreras' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campMensajesCarrera = await _context.CampMensajesCarreras.FindAsync(idMensaje, idCarrera);

            if (campMensajesCarrera == null)
            {
                return NotFound();
            }

            return campMensajesCarrera;
        }

        // PUT: api/CampMensajesCarrera/{idMensaje}/{idCarrera}
        // Para actualizar un mensaje existente usando la clave compuesta.
        [HttpPut("{idMensaje}/{idCarrera}")]
        public async Task<IActionResult> PutCampMensajesCarrera(int idMensaje, int idCarrera, CampMensajesCarrera campMensajesCarrera)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idMensaje != campMensajesCarrera.CmecIdMensaje || idCarrera != campMensajesCarrera.CmecIdCarrera)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampMensajesCarreras == null)
            {
                 return NotFound("La entidad 'CampMensajesCarreras' no está configurada en el DbContext.");
            }

             // Validar fechas si existen
             if (campMensajesCarrera.CmecFechaDesde.HasValue && campMensajesCarrera.CmecFechaHasta.HasValue &&
                 campMensajesCarrera.CmecFechaDesde.Value > campMensajesCarrera.CmecFechaHasta.Value)
             {
                 return BadRequest("La fecha 'Desde' no puede ser posterior a la fecha 'Hasta'.");
             }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campMensajesCarrera).State = EntityState.Modified;
            _context.Entry(campMensajesCarrera).Property(x => x.CmecIdMensaje).IsModified = false; // Previene modificación PK
            _context.Entry(campMensajesCarrera).Property(x => x.CmecIdCarrera).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampMensajesCarreraExists(idMensaje, idCarrera))
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

        // POST: api/CampMensajesCarrera
        // Para crear un nuevo mensaje de carrera.
        [HttpPost]
        public async Task<ActionResult<CampMensajesCarrera>> PostCampMensajesCarrera(CampMensajesCarrera campMensajesCarrera)
        {
             if (_context.CampMensajesCarreras == null)
            {
                 return Problem("La entidad 'CampMensajesCarreras' no está configurada en el DbContext.");
            }

             // Validar fechas si existen
             if (campMensajesCarrera.CmecFechaDesde.HasValue && campMensajesCarrera.CmecFechaHasta.HasValue &&
                 campMensajesCarrera.CmecFechaDesde.Value > campMensajesCarrera.CmecFechaHasta.Value)
             {
                 return BadRequest("La fecha 'Desde' no puede ser posterior a la fecha 'Hasta'.");
             }

            _context.CampMensajesCarreras.Add(campMensajesCarrera);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                 // Si CmecIdMensaje es Identity, la BD lo generará. Si no, debe venir en el request.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampMensajesCarreraExists(campMensajesCarrera.CmecIdMensaje, campMensajesCarrera.CmecIdCarrera))
                {
                    return Conflict("Ya existe un mensaje con esta clave (IdMensaje, IdCarrera).");
                }
                else
                {
                     // Podría ser un error de FK si la carrera, materia, etc., no existen
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampMensajesCarrera), new {
                 idMensaje = campMensajesCarrera.CmecIdMensaje,
                 idCarrera = campMensajesCarrera.CmecIdCarrera
                 }, campMensajesCarrera);
        }

        // DELETE: api/CampMensajesCarrera/{idMensaje}/{idCarrera}
        // Elimina un mensaje de carrera.
        [HttpDelete("{idMensaje}/{idCarrera}")]
        public async Task<IActionResult> DeleteCampMensajesCarrera(int idMensaje, int idCarrera)
        {
             if (_context.CampMensajesCarreras == null)
            {
                 return NotFound("La entidad 'CampMensajesCarreras' no está configurada en el DbContext.");
            }

            var campMensajesCarrera = await _context.CampMensajesCarreras.FindAsync(idMensaje, idCarrera);
            if (campMensajesCarrera == null)
            {
                return NotFound();
            }

            _context.CampMensajesCarreras.Remove(campMensajesCarrera);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampMensajesCarreraExists(int idMensaje, int idCarrera)
        {
            return (_context.CampMensajesCarreras?.Any(e =>
                e.CmecIdMensaje == idMensaje &&
                e.CmecIdCarrera == idCarrera)).GetValueOrDefault();
        }
    }
}