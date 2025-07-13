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
    public class CampNacionalidadeController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampNacionalidadeController> _logger;

        public CampNacionalidadeController(ExtranetContext context, IConfiguration configuration, ILogger<CampNacionalidadeController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampNacionalidade
        // Obtiene todas las nacionalidades.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampNacionalidade>>> GetCampNacionalidades()
        {
            if (_context.CampNacionalidades == null)
            {
                return NotFound("La entidad 'CampNacionalidades' no está configurada en el DbContext.");
            }
            // Considera ordenar las nacionalidades alfabéticamente.
            return await _context.CampNacionalidades.OrderBy(n => n.CnacNacionalidad).ToListAsync();
        }

        // GET: api/CampNacionalidade/{id}
        // Busca una nacionalidad por su clave primaria CnacIdNacionalidad (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampNacionalidade>> GetCampNacionalidade(int id)
        {
            if (_context.CampNacionalidades == null)
            {
                 return NotFound("La entidad 'CampNacionalidades' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campNacionalidade = await _context.CampNacionalidades.FindAsync(id);

            if (campNacionalidade == null)
            {
                return NotFound();
            }

            return campNacionalidade;
        }

        // PUT: api/CampNacionalidade/{id}
        // Para actualizar una nacionalidad existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampNacionalidade(int id, CampNacionalidade campNacionalidade)
        {
            if (id != campNacionalidade.CnacIdNacionalidad)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la nacionalidad.");
            }

             if (_context.CampNacionalidades == null)
            {
                 return NotFound("La entidad 'CampNacionalidades' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (CnacIdNacionalidad)
            _context.Entry(campNacionalidade).State = EntityState.Modified;
             _context.Entry(campNacionalidade).Property(x => x.CnacIdNacionalidad).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampNacionalidadeExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (CnacIdNacionalidad). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampNacionalidade
        // Para crear una nueva nacionalidad.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampNacionalidade>> PostCampNacionalidade(CampNacionalidade campNacionalidade)
        {
             if (_context.CampNacionalidades == null)
            {
                 return Problem("La entidad 'CampNacionalidades' no está configurada en el DbContext.");
            }

            _context.CampNacionalidades.Add(campNacionalidade);
            try
            {
                 // Asume que CnacIdNacionalidad es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CnacIdNacionalidad NO es autogenerado y ya existe
                 if (CampNacionalidadeExists(campNacionalidade.CnacIdNacionalidad))
                 {
                     return Conflict($"Ya existe una nacionalidad con ID {campNacionalidade.CnacIdNacionalidad}");
                 }
                 else
                 {
                     return Problem($"Error al guardar la nacionalidad: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampNacionalidade), new { id = campNacionalidade.CnacIdNacionalidad }, campNacionalidade);
        }

        // DELETE: api/CampNacionalidade/{id}
        // Elimina una nacionalidad.
        // Nota: Podría estar restringido o fallar si existen alumnos (u otras entidades) con esta nacionalidad.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampNacionalidade(int id)
        {
             if (_context.CampNacionalidades == null)
            {
                 return NotFound("La entidad 'CampNacionalidades' no está configurada en el DbContext.");
            }

            var campNacionalidade = await _context.CampNacionalidades.FindAsync(id);
            if (campNacionalidade == null)
            {
                return NotFound();
            }

            _context.CampNacionalidades.Remove(campNacionalidade);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (ej. si la tabla CampAlumno tiene una FK hacia CnacIdNacionalidad)
                  return Problem($"No se pudo eliminar la nacionalidad. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si una nacionalidad existe por su ID
        private bool CampNacionalidadeExists(int id)
        {
            return (_context.CampNacionalidades?.Any(e => e.CnacIdNacionalidad == id)).GetValueOrDefault();
        }
    }
}