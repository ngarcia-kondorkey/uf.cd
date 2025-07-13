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
    public class CampSexoController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampSexoController> _logger;

        public CampSexoController(ExtranetContext context, IConfiguration configuration, ILogger<CampSexoController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampSexo
        // Obtiene todos los registros de sexo/género.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampSexo>>> GetCampSexos()
        {
            if (_context.CampSexos == null)
            {
                return NotFound("La entidad 'CampSexos' no está configurada en el DbContext.");
            }
            return await _context.CampSexos.OrderBy(s => s.CsexSexo).ToListAsync();
        }

        // GET: api/CampSexo/{id}
        // Busca un registro de sexo/género por su clave primaria CsexIdSexo (string)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampSexo>> GetCampSexo(string id)
        {
            if (_context.CampSexos == null)
            {
                 return NotFound("La entidad 'CampSexos' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campSexo = await _context.CampSexos.FindAsync(id);

            if (campSexo == null)
            {
                return NotFound();
            }

            return campSexo;
        }

        // PUT: api/CampSexo/{id}
        // Para actualizar un registro de sexo/género existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampSexo(string id, CampSexo campSexo)
        {
            if (id != campSexo.CsexIdSexo)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del registro de sexo.");
            }

             if (_context.CampSexos == null)
            {
                 return NotFound("La entidad 'CampSexos' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (CsexIdSexo)
            _context.Entry(campSexo).State = EntityState.Modified;
             _context.Entry(campSexo).Property(x => x.CsexIdSexo).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampSexoExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (CsexIdSexo). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampSexo
        // Para crear un nuevo registro de sexo/género.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampSexo>> PostCampSexo(CampSexo campSexo)
        {
             if (_context.CampSexos == null)
            {
                 return Problem("La entidad 'CampSexos' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(campSexo.CsexIdSexo) || string.IsNullOrWhiteSpace(campSexo.CsexSexo))
             {
                 return BadRequest("Tanto CsexIdSexo como CsexSexo son requeridos.");
             }

            _context.CampSexos.Add(campSexo);
            try
            {
                 // Asume que CsexIdSexo debe ser único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CsexIdSexo ya existe
                 if (CampSexoExists(campSexo.CsexIdSexo))
                 {
                     return Conflict($"Ya existe un registro de sexo con ID {campSexo.CsexIdSexo}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el registro de sexo: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampSexo), new { id = campSexo.CsexIdSexo }, campSexo);
        }

        // DELETE: api/CampSexo/{id}
        // Elimina un registro de sexo/género.
        // Nota: Podría estar restringido o fallar si existen alumnos (u otras entidades) con este sexo/género.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampSexo(string id)
        {
             if (_context.CampSexos == null)
            {
                 return NotFound("La entidad 'CampSexos' no está configurada en el DbContext.");
            }

            var campSexo = await _context.CampSexos.FindAsync(id);
            if (campSexo == null)
            {
                return NotFound();
            }

            _context.CampSexos.Remove(campSexo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (ej. si la tabla CampAlumno tiene una FK hacia CsexIdSexo)
                  return Problem($"No se pudo eliminar el registro de sexo. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro de sexo/género existe por su ID
        private bool CampSexoExists(string id)
        {
            // Considerar comparación insensible a mayúsculas/minúsculas si aplica para el ID
            return (_context.CampSexos?.Any(e => e.CsexIdSexo == id)).GetValueOrDefault();
        }
    }
}