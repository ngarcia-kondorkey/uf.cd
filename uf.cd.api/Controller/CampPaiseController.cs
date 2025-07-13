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
    public class CampPaiseController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampPaiseController> _logger;

        public CampPaiseController(ExtranetContext context, IConfiguration configuration, ILogger<CampPaiseController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampPaise
        // Obtiene todos los países.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampPaise>>> GetCampPaises()
        {
            if (_context.CampPaises == null)
            {
                return NotFound("La entidad 'CampPaises' no está configurada en el DbContext.");
            }
            // Considera ordenar los países alfabéticamente.
            return await _context.CampPaises.OrderBy(p => p.CpaiPais).ToListAsync();
        }

        // GET: api/CampPaise/{id}
        // Busca un país por su clave primaria CpaiIdPais (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampPaise>> GetCampPaise(int id)
        {
            if (_context.CampPaises == null)
            {
                 return NotFound("La entidad 'CampPaises' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campPaise = await _context.CampPaises.FindAsync(id);

            if (campPaise == null)
            {
                return NotFound();
            }

            return campPaise;
        }

        // PUT: api/CampPaise/{id}
        // Para actualizar un país existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampPaise(int id, CampPaise campPaise)
        {
            if (id != campPaise.CpaiIdPais)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del país.");
            }

             if (_context.CampPaises == null)
            {
                 return NotFound("La entidad 'CampPaises' no está configurada en el DbContext.");
            }

             // Solo permite modificar campos que no son parte de la clave primaria (CpaiIdPais)
            _context.Entry(campPaise).State = EntityState.Modified;
             _context.Entry(campPaise).Property(x => x.CpaiIdPais).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampPaiseExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (CpaiIdPais). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampPaise
        // Para crear un nuevo país.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampPaise>> PostCampPaise(CampPaise campPaise)
        {
             if (_context.CampPaises == null)
            {
                 return Problem("La entidad 'CampPaises' no está configurada en el DbContext.");
            }

            _context.CampPaises.Add(campPaise);
            try
            {
                 // Asume que CpaiIdPais es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CpaiIdPais NO es autogenerado y ya existe
                 if (CampPaiseExists(campPaise.CpaiIdPais))
                 {
                     return Conflict($"Ya existe un país con ID {campPaise.CpaiIdPais}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el país: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampPaise), new { id = campPaise.CpaiIdPais }, campPaise);
        }

        // DELETE: api/CampPaise/{id}
        // Elimina un país.
        // Nota: Podría estar restringido o fallar si existen provincias, localidades, alumnos, etc., asociados.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampPaise(int id)
        {
             if (_context.CampPaises == null)
            {
                 return NotFound("La entidad 'CampPaises' no está configurada en el DbContext.");
            }

            var campPaise = await _context.CampPaises.FindAsync(id);
            if (campPaise == null)
            {
                return NotFound();
            }

            _context.CampPaises.Remove(campPaise);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                  return Problem($"No se pudo eliminar el país. Es posible que esté en uso por provincias, localidades u otros registros. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un país existe por su ID
        private bool CampPaiseExists(int id)
        {
            return (_context.CampPaises?.Any(e => e.CpaiIdPais == id)).GetValueOrDefault();
        }
    }
}