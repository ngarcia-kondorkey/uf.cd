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
    public class CompensacionesAgostoController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CompensacionesAgostoController> _logger;

        public CompensacionesAgostoController(ExtranetContext context, IConfiguration configuration, ILogger<CompensacionesAgostoController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CompensacionesAgosto
        // Obtiene todos los registros de compensaciones de agosto.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompensacionesAgosto>>> GetCompensacionesAgosto()
        {
            if (_context.CompensacionesAgostos == null)
            {
                return NotFound("La entidad 'CompensacionesAgosto' no está configurada en el DbContext.");
            }
            return await _context.CompensacionesAgostos.ToListAsync();
        }

        // GET: api/CompensacionesAgosto/{fact}
        // Busca un registro por su clave primaria Fact (string)
        [HttpGet("{fact}")]
        public async Task<ActionResult<CompensacionesAgosto>> GetCompensacionAgosto(string fact)
        {
            if (_context.CompensacionesAgostos == null)
            {
                 return NotFound("La entidad 'CompensacionesAgosto' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            // Considera normalizar/validar el formato de 'fact' si es necesario.
            var compensacionAgosto = await _context.CompensacionesAgostos.FindAsync(fact);

            if (compensacionAgosto == null)
            {
                return NotFound();
            }

            return compensacionAgosto;
        }

        // PUT: api/CompensacionesAgosto/{fact}
        // Actualiza un registro.
        // Nota: Dado que 'Fact' es la única propiedad y la clave primaria,
        // esta operación tiene una utilidad limitada más allá de verificar la existencia
        // y potencialmente manejar concurrencia si hubiera más campos ocultos o versionado.
        [HttpPut("{fact}")]
        public async Task<IActionResult> PutCompensacionAgosto(string fact, CompensacionesAgosto compensacionAgosto)
        {
            // Valida que el ID de la ruta coincida con el ID del objeto
            if (fact != compensacionAgosto.Fact)
            {
                return BadRequest("El 'Fact' de la ruta no coincide con el 'Fact' del objeto.");
            }

             if (_context.CompensacionesAgostos == null)
            {
                 return NotFound("La entidad 'CompensacionesAgosto' no está configurada en el DbContext.");
            }

            // Marca el estado como modificado para disparar la lógica de concurrencia si existe,
            // aunque no haya otros campos que cambiar.
            _context.Entry(compensacionAgosto).State = EntityState.Modified;
             _context.Entry(compensacionAgosto).Property(x => x.Fact).IsModified = false; // Previene modificación PK explícitamente

            try
            {
                // Intenta guardar para detectar problemas de concurrencia o si fue eliminado.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompensacionAgostoExists(fact))
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
                 return BadRequest($"No se puede modificar la clave primaria (Fact). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver (ya que no hubo cambios reales de datos)
        }

        // POST: api/CompensacionesAgosto
        // Para crear un nuevo registro de compensación.
        [HttpPost]
        public async Task<ActionResult<CompensacionesAgosto>> PostCompensacionAgosto(CompensacionesAgosto compensacionAgosto)
        {
             if (_context.CompensacionesAgostos == null)
            {
                 return Problem("La entidad 'CompensacionesAgosto' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(compensacionAgosto.Fact))
             {
                 return BadRequest("El campo 'Fact' es requerido.");
             }

            _context.CompensacionesAgostos.Add(compensacionAgosto);
            try
            {
                 // Asume que Fact debe ser único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si Fact ya existe
                 if (CompensacionAgostoExists(compensacionAgosto.Fact))
                 {
                     return Conflict($"Ya existe un registro con Fact {compensacionAgosto.Fact}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el registro: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            // Nota: El objeto devuelto solo contiene 'Fact'.
            return CreatedAtAction(nameof(GetCompensacionAgosto), new { fact = compensacionAgosto.Fact }, compensacionAgosto);
        }

        // DELETE: api/CompensacionesAgosto/{fact}
        // Elimina un registro de compensación.
        [HttpDelete("{fact}")]
        public async Task<IActionResult> DeleteCompensacionAgosto(string fact)
        {
             if (_context.CompensacionesAgostos == null)
            {
                 return NotFound("La entidad 'CompensacionesAgosto' no está configurada en el DbContext.");
            }

            var compensacionAgosto = await _context.CompensacionesAgostos.FindAsync(fact);
            if (compensacionAgosto == null)
            {
                return NotFound();
            }

            _context.CompensacionesAgostos.Remove(compensacionAgosto);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su ID (Fact)
        private bool CompensacionAgostoExists(string fact)
        {
            // Considerar comparación insensible a mayúsculas/minúsculas si aplica para 'Fact'
            return (_context.CompensacionesAgostos?.Any(e => e.Fact == fact)).GetValueOrDefault();
        }
    }
}