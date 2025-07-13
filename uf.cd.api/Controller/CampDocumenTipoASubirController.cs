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
    public class CampDocumenTipoASubirController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampDocumenTipoASubirController> _logger;

        public CampDocumenTipoASubirController(ExtranetContext context, IConfiguration configuration, ILogger<CampDocumenTipoASubirController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampDocumenTipoASubir
        // Obtiene todos los tipos de documentos que se pueden subir.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampDocumenTipoASubir>>> GetCampDocumenTiposASubir()
        {
            if (_context.CampDocumenTipoASubirs == null)
            {
                return NotFound("La entidad 'CampDocumenTiposASubir' no está configurada en el DbContext.");
            }
            return await _context.CampDocumenTipoASubirs.ToListAsync();
        }

        // GET: api/CampDocumenTipoASubir/{id}
        // Busca un tipo de documento por su clave primaria CdocIdTipoadjunto (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampDocumenTipoASubir>> GetCampDocumenTipoASubir(int id)
        {
            if (_context.CampDocumenTipoASubirs == null)
            {
                 return NotFound("La entidad 'CampDocumenTiposASubir' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campDocumenTipoASubir = await _context.CampDocumenTipoASubirs.FindAsync(id);

            if (campDocumenTipoASubir == null)
            {
                return NotFound();
            }

            return campDocumenTipoASubir;
        }

        // PUT: api/CampDocumenTipoASubir/{id}
        // Para actualizar un tipo de documento existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampDocumenTipoASubir(int id, CampDocumenTipoASubir campDocumenTipoASubir)
        {
            if (id != campDocumenTipoASubir.CdocIdTipoadjunto)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del tipo de adjunto.");
            }

             if (_context.CampDocumenTipoASubirs == null)
            {
                 return NotFound("La entidad 'CampDocumenTiposASubir' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (CdocIdTipoadjunto)
            _context.Entry(campDocumenTipoASubir).State = EntityState.Modified;
             _context.Entry(campDocumenTipoASubir).Property(x => x.CdocIdTipoadjunto).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampDocumenTipoASubirExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (CdocIdTipoadjunto). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampDocumenTipoASubir
        // Para crear un nuevo tipo de documento.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampDocumenTipoASubir>> PostCampDocumenTipoASubir(CampDocumenTipoASubir campDocumenTipoASubir)
        {
             if (_context.CampDocumenTipoASubirs == null)
            {
                 return Problem("La entidad 'CampDocumenTiposASubir' no está configurada en el DbContext.");
            }

            _context.CampDocumenTipoASubirs.Add(campDocumenTipoASubir);
            try
            {
                 // Asume que CdocIdTipoadjunto es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CdocIdTipoadjunto NO es autogenerado y ya existe
                 if (CampDocumenTipoASubirExists(campDocumenTipoASubir.CdocIdTipoadjunto))
                 {
                     return Conflict($"Ya existe un tipo de adjunto con ID {campDocumenTipoASubir.CdocIdTipoadjunto}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el tipo de adjunto: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampDocumenTipoASubir), new { id = campDocumenTipoASubir.CdocIdTipoadjunto }, campDocumenTipoASubir);
        }

        // DELETE: api/CampDocumenTipoASubir/{id}
        // Elimina un tipo de documento.
        // Nota: Podría estar restringido o fallar si existen documentos asociados a este tipo.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampDocumenTipoASubir(int id)
        {
             if (_context.CampDocumenTipoASubirs == null)
            {
                 return NotFound("La entidad 'CampDocumenTiposASubir' no está configurada en el DbContext.");
            }

            var campDocumenTipoASubir = await _context.CampDocumenTipoASubirs.FindAsync(id);
            if (campDocumenTipoASubir == null)
            {
                return NotFound();
            }

            _context.CampDocumenTipoASubirs.Remove(campDocumenTipoASubir);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (La excepción específica puede variar según el motor de BD)
                  return Problem($"No se pudo eliminar el tipo de adjunto. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }


            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un tipo de documento existe por su ID
        private bool CampDocumenTipoASubirExists(int id)
        {
            return (_context.CampDocumenTipoASubirs?.Any(e => e.CdocIdTipoadjunto == id)).GetValueOrDefault();
        }
    }
}