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
    public class CampDeudaHistController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampDeudaHistController> _logger;

        public CampDeudaHistController(ExtranetContext context, IConfiguration configuration, ILogger<CampDeudaHistController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampDeudaHist
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampDeudaHist>>> GetCampDeudaHists() // Asumiendo DbSet se llama CampDeudaHists
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampDeudaHists == null)
            {
                return NotFound("La entidad 'CampDeudaHists' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todo el historial de deuda puede ser ineficiente.
            // Considera aplicar filtros obligatorios (por fecha, por cliente, etc.) o paginación.
            return await _context.CampDeudaHists.OrderByDescending(h => h.FechaReg).ToListAsync(); // Ejemplo: Ordenar por fecha de registro
        }

        // GET: api/CampDeudaHist/{id}
        // Busca por la clave primaria IdFactura (int)
        // Nota: Esto asume que IdFactura es único incluso en la tabla histórica.
        // Si puede haber múltiples entradas históricas para la misma IdFactura, este método
        // podría devolver solo una de ellas o requerir una clave diferente (ej: un Id de historial).
        [HttpGet("{id}")]
        public async Task<ActionResult<CampDeudaHist>> GetCampDeudaHist(int id)
        {
            if (_context.CampDeudaHists == null)
            {
                 return NotFound("La entidad 'CampDeudaHists' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria asumida.
            var campDeudaHist = await _context.CampDeudaHists.FindAsync(id);

            if (campDeudaHist == null)
            {
                return NotFound($"No se encontró historial para IdFactura {id}");
            }

            return campDeudaHist;
        }

        // PUT: api/CampDeudaHist/{id}
        // Para actualizar un registro histórico.
        // ADVERTENCIA: Actualizar registros históricos generalmente NO es una buena práctica.
        // La historia debería ser inmutable. Considera si realmente necesitas esta operación.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampDeudaHist(int id, CampDeudaHist campDeudaHist)
        {
            // Valida que el ID de la ruta coincida con el ID del objeto
            if (id != campDeudaHist.IdFactura)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la factura en el historial.");
            }

             if (_context.CampDeudaHists == null)
            {
                 return NotFound("La entidad 'CampDeudaHists' no está configurada en el DbContext.");
            }

             // Previene la modificación de la clave primaria (IdFactura)
            _context.Entry(campDeudaHist).State = EntityState.Modified;
            _context.Entry(campDeudaHist).Property(x => x.IdFactura).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampDeudaHistExists(id))
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


            // Considera devolver Ok() o el objeto actualizado en lugar de NoContent()
            // si es útil para el cliente saber qué se actualizó.
            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampDeudaHist
        // Para crear un nuevo registro histórico.
        // Nota: Usualmente los registros históricos se crean mediante procesos internos (triggers, lógica de negocio)
        // y no directamente vía API, pero se incluye la posibilidad.
        [HttpPost]
        public async Task<ActionResult<CampDeudaHist>> PostCampDeudaHist(CampDeudaHist campDeudaHist)
        {
             if (_context.CampDeudaHists == null)
            {
                 return Problem("La entidad 'CampDeudaHists' no está configurada en el DbContext.");
            }

             // Opcional: Establecer FechaReg si no viene en el request
             // if(!campDeudaHist.FechaReg.HasValue)
             // {
             //     campDeudaHist.FechaReg = DateTime.UtcNow;
             // }

            _context.CampDeudaHists.Add(campDeudaHist);
            try
            {
                // Asume que IdFactura se provee correctamente. Si puede haber duplicados
                // (porque no es PK única aquí), este save funcionará, pero GetCampDeudaHist(id) será ambiguo.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Verifica si el registro ya existe (si IdFactura DEBE ser único aquí)
                 if (CampDeudaHistExists(campDeudaHist.IdFactura))
                 {
                     // O quizás aquí no debería dar conflicto si se permite múltiple historia...
                     // return Conflict($"Ya existe un registro de historial con IdFactura {campDeudaHist.IdFactura}");
                      return Problem($"Error de base de datos al guardar, posible duplicado si IdFactura debe ser único. {ex.InnerException?.Message ?? ex.Message}");
                 }
                 else
                 {
                    return Problem($"Error al guardar el registro de historial: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            // Advertencia: Esto asume que GetCampDeudaHist(id) puede recuperar este registro específico.
            return CreatedAtAction(nameof(GetCampDeudaHist), new { id = campDeudaHist.IdFactura }, campDeudaHist);
        }

        // DELETE: api/CampDeudaHist/{id}
        // Para eliminar un registro histórico.
        // ADVERTENCIA: Eliminar registros históricos generalmente NO es una buena práctica.
        // Considera si realmente necesitas esta operación. Podría ser mejor marcarlo como inválido.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampDeudaHist(int id)
        {
             if (_context.CampDeudaHists == null)
            {
                 return NotFound("La entidad 'CampDeudaHists' no está configurada en el DbContext.");
            }

            // Asume que IdFactura identifica unívocamente el registro a eliminar
            var campDeudaHist = await _context.CampDeudaHists.FindAsync(id);
            if (campDeudaHist == null)
            {
                return NotFound($"No se encontró historial para IdFactura {id}");
            }

            _context.CampDeudaHists.Remove(campDeudaHist);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su ID (asumiendo IdFactura es único aquí)
        private bool CampDeudaHistExists(int id)
        {
            // Asegúrate que CampDeudaHists no sea null antes de usarlo
            return (_context.CampDeudaHists?.Any(e => e.IdFactura == id)).GetValueOrDefault();
        }
    }
}