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
    public class CampEncuestasEstudioItemController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampEncuestasEstudioItemController> _logger;

        public CampEncuestasEstudioItemController(ExtranetContext context, IConfiguration configuration, ILogger<CampEncuestasEstudioItemController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampEncuestasEstudioItem
        // Obtiene todos los niveles de instrucción para encuestas.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampEncuestasEstudioItem>>> GetCampEncuestasEstudioItems()
        {
            if (_context.CampEncuestasEstudioItems == null)
            {
                return NotFound("La entidad 'CampEncuestasEstudioItems' no está configurada en el DbContext.");
            }
            return await _context.CampEncuestasEstudioItems.ToListAsync();
        }

        // GET: api/CampEncuestasEstudioItem/{id}
        // Busca un nivel de instrucción por su clave primaria CeeiIdNivelinstruccion (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampEncuestasEstudioItem>> GetCampEncuestasEstudioItem(int id)
        {
            if (_context.CampEncuestasEstudioItems == null)
            {
                 return NotFound("La entidad 'CampEncuestasEstudioItems' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campEncuestasEstudioItem = await _context.CampEncuestasEstudioItems.FindAsync(id);

            if (campEncuestasEstudioItem == null)
            {
                return NotFound();
            }

            return campEncuestasEstudioItem;
        }

        // PUT: api/CampEncuestasEstudioItem/{id}
        // Para actualizar un nivel de instrucción existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampEncuestasEstudioItem(int id, CampEncuestasEstudioItem campEncuestasEstudioItem)
        {
            if (id != campEncuestasEstudioItem.CeeiIdNivelinstruccion)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del nivel de instrucción.");
            }

             if (_context.CampEncuestasEstudioItems == null)
            {
                 return NotFound("La entidad 'CampEncuestasEstudioItems' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (CeeiIdNivelinstruccion)
            _context.Entry(campEncuestasEstudioItem).State = EntityState.Modified;
            _context.Entry(campEncuestasEstudioItem).Property(x => x.CeeiIdNivelinstruccion).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampEncuestasEstudioItemExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (CeeiIdNivelinstruccion). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampEncuestasEstudioItem
        // Para crear un nuevo nivel de instrucción.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampEncuestasEstudioItem>> PostCampEncuestasEstudioItem(CampEncuestasEstudioItem campEncuestasEstudioItem)
        {
             if (_context.CampEncuestasEstudioItems == null)
            {
                 return Problem("La entidad 'CampEncuestasEstudioItems' no está configurada en el DbContext.");
            }

            _context.CampEncuestasEstudioItems.Add(campEncuestasEstudioItem);
            try
            {
                 // Asume que CeeiIdNivelinstruccion es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CeeiIdNivelinstruccion NO es autogenerado y ya existe
                 if (CampEncuestasEstudioItemExists(campEncuestasEstudioItem.CeeiIdNivelinstruccion))
                 {
                     return Conflict($"Ya existe un nivel de instrucción con ID {campEncuestasEstudioItem.CeeiIdNivelinstruccion}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el nivel de instrucción: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampEncuestasEstudioItem), new { id = campEncuestasEstudioItem.CeeiIdNivelinstruccion }, campEncuestasEstudioItem);
        }

        // DELETE: api/CampEncuestasEstudioItem/{id}
        // Elimina un nivel de instrucción.
        // Nota: Podría estar restringido o fallar si existen encuestas que referencien este nivel.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampEncuestasEstudioItem(int id)
        {
             if (_context.CampEncuestasEstudioItems == null)
            {
                 return NotFound("La entidad 'CampEncuestasEstudioItems' no está configurada en el DbContext.");
            }

            var campEncuestasEstudioItem = await _context.CampEncuestasEstudioItems.FindAsync(id);
            if (campEncuestasEstudioItem == null)
            {
                return NotFound();
            }

            _context.CampEncuestasEstudioItems.Remove(campEncuestasEstudioItem);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (La excepción específica puede variar según el motor de BD)
                  return Problem($"No se pudo eliminar el nivel de instrucción. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un nivel de instrucción existe por su ID
        private bool CampEncuestasEstudioItemExists(int id)
        {
            return (_context.CampEncuestasEstudioItems?.Any(e => e.CeeiIdNivelinstruccion == id)).GetValueOrDefault();
        }
    }
}