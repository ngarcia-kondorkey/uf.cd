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
    public class CampProvinciaController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampProvinciaController> _logger;

        public CampProvinciaController(ExtranetContext context, IConfiguration configuration, ILogger<CampProvinciaController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampProvincia
        // Obtiene todas las provincias.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampProvincia>>> GetCampProvincias()
        {
            if (_context.CampProvincias == null)
            {
                return NotFound("La entidad 'CampProvincias' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todas las provincias puede ser ineficiente.
             // Considera filtrar por país o implementar paginación.
            return await _context.CampProvincias.OrderBy(p => p.CprvProvincia).ToListAsync();
        }

        // GET: api/CampProvincia/{idPais}/{idProvincia}
        // Busca una provincia específica por su clave primaria compuesta asumida.
        [HttpGet("{idPais}/{idProvincia}")]
        public async Task<ActionResult<CampProvincia>> GetCampProvincia(int idPais, int idProvincia)
        {
            if (_context.CampProvincias == null)
            {
                 return NotFound("La entidad 'CampProvincias' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campProvincia = await _context.CampProvincias.FindAsync(idPais, idProvincia);

            if (campProvincia == null)
            {
                return NotFound();
            }

            return campProvincia;
        }

        // PUT: api/CampProvincia/{idPais}/{idProvincia}
        // Para actualizar una provincia existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{idPais}/{idProvincia}")]
        public async Task<IActionResult> PutCampProvincia(int idPais, int idProvincia, CampProvincia campProvincia)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idPais != campProvincia.CprvIdPais || idProvincia != campProvincia.CprvIdProvincia)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampProvincias == null)
            {
                 return NotFound("La entidad 'CampProvincias' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campProvincia).State = EntityState.Modified;
            _context.Entry(campProvincia).Property(x => x.CprvIdPais).IsModified = false; // Previene modificación PK
            _context.Entry(campProvincia).Property(x => x.CprvIdProvincia).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampProvinciaExists(idPais, idProvincia))
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

        // POST: api/CampProvincia
        // Para crear una nueva provincia.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampProvincia>> PostCampProvincia(CampProvincia campProvincia)
        {
             if (_context.CampProvincias == null)
            {
                 return Problem("La entidad 'CampProvincias' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(campProvincia.CprvProvincia))
             {
                 return BadRequest("El nombre de la provincia es requerido.");
             }

            _context.CampProvincias.Add(campProvincia);
            try
            {
                 // Asume que la combinación de claves primarias es única y se proveen valores válidos.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampProvinciaExists(campProvincia.CprvIdPais, campProvincia.CprvIdProvincia))
                {
                    return Conflict("Ya existe una provincia con esta clave (País, Provincia).");
                }
                else
                {
                     // Podría ser un error de FK si el país no existe
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampProvincia), new {
                 idPais = campProvincia.CprvIdPais,
                 idProvincia = campProvincia.CprvIdProvincia
                 }, campProvincia);
        }

        // DELETE: api/CampProvincia/{idPais}/{idProvincia}
        // Elimina una provincia.
        // Nota: Podría estar restringido o fallar si existen localidades (u otros registros) asociados.
        [HttpDelete("{idPais}/{idProvincia}")]
        public async Task<IActionResult> DeleteCampProvincia(int idPais, int idProvincia)
        {
             if (_context.CampProvincias == null)
            {
                 return NotFound("La entidad 'CampProvincias' no está configurada en el DbContext.");
            }

            var campProvincia = await _context.CampProvincias.FindAsync(idPais, idProvincia);
            if (campProvincia == null)
            {
                return NotFound();
            }

            _context.CampProvincias.Remove(campProvincia);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                  return Problem($"No se pudo eliminar la provincia. Es posible que esté en uso por localidades u otros registros. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampProvinciaExists(int idPais, int idProvincia)
        {
            return (_context.CampProvincias?.Any(e =>
                e.CprvIdPais == idPais &&
                e.CprvIdProvincia == idProvincia)).GetValueOrDefault();
        }
    }
}