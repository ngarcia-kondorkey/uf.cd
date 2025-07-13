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
    public class TotalVisitorController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TotalVisitorController> _logger;

        public TotalVisitorController(ExtranetContext context, IConfiguration configuration, ILogger<TotalVisitorController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/TotalVisitor
        // Obtiene todos los registros de visitantes totales.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TotalVisitor>>> GetTotalVisitors()
        {
            if (_context.TotalVisitors == null)
            {
                return NotFound("La entidad 'TotalVisitors' no está configurada en el DbContext.");
            }
            // Considera ordenar por fecha si es relevante.
            return await _context.TotalVisitors.OrderByDescending(tv => tv.DateVisit).ToListAsync();
        }

        // GET: api/TotalVisitor/{id}
        // Busca un registro específico por su clave primaria Id (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<TotalVisitor>> GetTotalVisitor(int id)
        {
            if (_context.TotalVisitors == null)
            {
                 return NotFound("La entidad 'TotalVisitors' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var totalVisitor = await _context.TotalVisitors.FindAsync(id);

            if (totalVisitor == null)
            {
                return NotFound();
            }

            return totalVisitor;
        }

        // PUT: api/TotalVisitor/{id}
        // Para actualizar un registro de visitante total existente.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTotalVisitor(int id, TotalVisitor totalVisitor)
        {
            if (id != totalVisitor.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del registro.");
            }

             if (_context.TotalVisitors == null)
            {
                 return NotFound("La entidad 'TotalVisitors' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (Id)
            _context.Entry(totalVisitor).State = EntityState.Modified;
            _context.Entry(totalVisitor).Property(x => x.Id).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TotalVisitorExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (Id). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/TotalVisitor
        // Para crear un nuevo registro de visitante total.
        [HttpPost]
        public async Task<ActionResult<TotalVisitor>> PostTotalVisitor(TotalVisitor totalVisitor)
        {
             if (_context.TotalVisitors == null)
            {
                 return Problem("La entidad 'TotalVisitors' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(totalVisitor.NVisit < 0)
             {
                return BadRequest("El número de visitas (NVisit) no puede ser negativo.");
             }
             // Asegura que DateVisit tenga un valor si es requerido lógicamente, aunque el modelo lo permita nulo
             // if(totalVisitor.DateVisit == default(DateTime)) {
             //     totalVisitor.DateVisit = DateTime.UtcNow; // O DateTime.Now
             // }

             // Asegura que Id no se envíe o sea 0 si es autogenerado por la BD
             // totalVisitor.Id = 0; // Descomentar si Id es Identity y quieres asegurarte

            _context.TotalVisitors.Add(totalVisitor);
            try
            {
                 // El Id probablemente será generado por la base de datos al guardar.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar otros posibles errores de base de datos
                 return Problem($"Error al guardar el registro de visitante total: {ex.InnerException?.Message ?? ex.Message}");
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetTotalVisitor), new { id = totalVisitor.Id }, totalVisitor);
        }

        // DELETE: api/TotalVisitor/{id}
        // Elimina un registro de visitante total.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTotalVisitor(int id)
        {
             if (_context.TotalVisitors == null)
            {
                 return NotFound("La entidad 'TotalVisitors' no está configurada en el DbContext.");
            }

            var totalVisitor = await _context.TotalVisitors.FindAsync(id);
            if (totalVisitor == null)
            {
                return NotFound();
            }

            _context.TotalVisitors.Remove(totalVisitor);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro de visitante total existe por su ID
        private bool TotalVisitorExists(int id)
        {
            return (_context.TotalVisitors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}