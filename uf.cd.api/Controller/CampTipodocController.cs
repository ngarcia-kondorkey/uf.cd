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
    public class CampTipodocController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampTipodocController> _logger;

        public CampTipodocController(ExtranetContext context, IConfiguration configuration, ILogger<CampTipodocController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampTipodoc
        // Obtiene todos los tipos de documento.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampTipodoc>>> GetCampTipodocs()
        {
            if (_context.CampTipodocs == null)
            {
                return NotFound("La entidad 'CampTipodocs' no está configurada en el DbContext.");
            }
            return await _context.CampTipodocs.OrderBy(t => t.CtdcTipodoc).ToListAsync();
        }

        // GET: api/CampTipodoc/{id}
        // Busca un tipo de documento por su clave primaria CtdcIdTipodoc (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampTipodoc>> GetCampTipodoc(int id)
        {
            if (_context.CampTipodocs == null)
            {
                 return NotFound("La entidad 'CampTipodocs' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campTipodoc = await _context.CampTipodocs.FindAsync(id);

            if (campTipodoc == null)
            {
                return NotFound();
            }

            return campTipodoc;
        }

        // PUT: api/CampTipodoc/{id}
        // Para actualizar un tipo de documento existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampTipodoc(int id, CampTipodoc campTipodoc)
        {
            if (id != campTipodoc.CtdcIdTipodoc)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del tipo de documento.");
            }

             if (_context.CampTipodocs == null)
            {
                 return NotFound("La entidad 'CampTipodocs' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (CtdcIdTipodoc)
            _context.Entry(campTipodoc).State = EntityState.Modified;
             _context.Entry(campTipodoc).Property(x => x.CtdcIdTipodoc).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampTipodocExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (CtdcIdTipodoc). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampTipodoc
        // Para crear un nuevo tipo de documento.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampTipodoc>> PostCampTipodoc(CampTipodoc campTipodoc)
        {
             if (_context.CampTipodocs == null)
            {
                 return Problem("La entidad 'CampTipodocs' no está configurada en el DbContext.");
            }

            _context.CampTipodocs.Add(campTipodoc);
            try
            {
                 // Asume que CtdcIdTipodoc es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CtdcIdTipodoc NO es autogenerado y ya existe
                 if (CampTipodocExists(campTipodoc.CtdcIdTipodoc))
                 {
                     return Conflict($"Ya existe un tipo de documento con ID {campTipodoc.CtdcIdTipodoc}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el tipo de documento: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampTipodoc), new { id = campTipodoc.CtdcIdTipodoc }, campTipodoc);
        }

        // DELETE: api/CampTipodoc/{id}
        // Elimina un tipo de documento.
        // Nota: Podría estar restringido o fallar si existen alumnos (u otras entidades) con este tipo de documento.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampTipodoc(int id)
        {
             if (_context.CampTipodocs == null)
            {
                 return NotFound("La entidad 'CampTipodocs' no está configurada en el DbContext.");
            }

            var campTipodoc = await _context.CampTipodocs.FindAsync(id);
            if (campTipodoc == null)
            {
                return NotFound();
            }

            _context.CampTipodocs.Remove(campTipodoc);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (ej. si la tabla CampAlumno tiene una FK hacia CtdcIdTipodoc)
                  return Problem($"No se pudo eliminar el tipo de documento. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un tipo de documento existe por su ID
        private bool CampTipodocExists(int id)
        {
            return (_context.CampTipodocs?.Any(e => e.CtdcIdTipodoc == id)).GetValueOrDefault();
        }
    }
}