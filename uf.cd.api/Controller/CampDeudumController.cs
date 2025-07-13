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
    public class CampDeudumController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampDeudumController> _logger;

        public CampDeudumController(ExtranetContext context, IConfiguration configuration, ILogger<CampDeudumController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampDeudum
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampDeudum>>> GetCampDeuda()
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampDeuda == null)
            {
                return NotFound("La entidad 'CampDeuda' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los registros de deuda puede ser ineficiente y exponer datos.
            // Considera aplicar filtros o paginación obligatoria.
            return await _context.CampDeuda.ToListAsync();
        }

        // GET: api/CampDeudum/{id}
        // Busca por la clave primaria IdFactura (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampDeudum>> GetCampDeudum(int id)
        {
            if (_context.CampDeuda == null)
            {
                 return NotFound("La entidad 'CampDeuda' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campDeudum = await _context.CampDeuda.FindAsync(id);

            if (campDeudum == null)
            {
                return NotFound();
            }

            return campDeudum;
        }

        // PUT: api/CampDeudum/{id}
        // Para actualizar un registro de deuda existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampDeudum(int id, CampDeudum campDeudum)
        {
            // Valida que el ID de la ruta coincida con el ID del objeto
            if (id != campDeudum.IdFactura)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la factura.");
            }

             if (_context.CampDeuda == null)
            {
                 return NotFound("La entidad 'CampDeuda' no está configurada en el DbContext.");
            }

             // Solo permite modificar campos que no son parte de la clave primaria (IdFactura)
            _context.Entry(campDeudum).State = EntityState.Modified;
            _context.Entry(campDeudum).Property(x => x.IdFactura).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampDeudumExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (IdFactura). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampDeudum
        // Para crear un nuevo registro de deuda
        [HttpPost]
        public async Task<ActionResult<CampDeudum>> PostCampDeudum(CampDeudum campDeudum)
        {
             if (_context.CampDeuda == null)
            {
                 return Problem("La entidad 'CampDeuda' no está configurada en el DbContext.");
            }

            _context.CampDeuda.Add(campDeudum);
            try
            {
                 // Asume que IdFactura es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si IdFactura NO es autogenerado y ya existe
                 if (CampDeudumExists(campDeudum.IdFactura))
                 {
                     return Conflict($"Ya existe un registro de deuda con IdFactura {campDeudum.IdFactura}");
                 }
                 else
                 {
                    // O podría ser otro error de constraint, etc.
                    return Problem($"Error al guardar el registro de deuda: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampDeudum), new { id = campDeudum.IdFactura }, campDeudum);
        }

        // DELETE: api/CampDeudum/{id}
        // Elimina un registro de deuda. Considera usar Soft Delete (marcar como inactivo) en lugar de borrar físicamente.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampDeudum(int id)
        {
             if (_context.CampDeuda == null)
            {
                 return NotFound("La entidad 'CampDeuda' no está configurada en el DbContext.");
            }

            var campDeudum = await _context.CampDeuda.FindAsync(id);
            if (campDeudum == null)
            {
                return NotFound();
            }

            _context.CampDeuda.Remove(campDeudum);
            // Alternativa Soft Delete:
            // campDeudum.Estado = "BAJA"; // O un campo booleano IsDeleted = true;
            // _context.Entry(campDeudum).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su ID
        private bool CampDeudumExists(int id)
        {
            // Asegúrate que CampDeuda no sea null antes de usarlo
            return (_context.CampDeuda?.Any(e => e.IdFactura == id)).GetValueOrDefault();
        }
    }
}