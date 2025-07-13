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
    public class DayVisitorController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<DayVisitorController> _logger;

        public DayVisitorController(ExtranetContext context, IConfiguration configuration, ILogger<DayVisitorController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/DayVisitor
        // Obtiene todos los registros de visitantes por día.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DayVisitor>>> GetDayVisitors()
        {
            if (_context.DayVisitors == null)
            {
                return NotFound("La entidad 'DayVisitors' no está configurada en el DbContext.");
            }
            // Considera ordenar por fecha si es relevante.
            return await _context.DayVisitors.OrderByDescending(dv => dv.DateVisit).ToListAsync();
        }

        // GET: api/DayVisitor/{id}
        // Busca un registro específico por su clave primaria Id (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<DayVisitor>> GetDayVisitor(int id)
        {
            if (_context.DayVisitors == null)
            {
                 return NotFound("La entidad 'DayVisitors' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var dayVisitor = await _context.DayVisitors.FindAsync(id);

            if (dayVisitor == null)
            {
                return NotFound();
            }

            return dayVisitor;
        }

        // PUT: api/DayVisitor/{id}
        // Para actualizar un registro de visitante existente.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDayVisitor(int id, DayVisitor dayVisitor)
        {
            if (id != dayVisitor.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del registro.");
            }

             if (_context.DayVisitors == null)
            {
                 return NotFound("La entidad 'DayVisitors' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (Id)
            _context.Entry(dayVisitor).State = EntityState.Modified;
            _context.Entry(dayVisitor).Property(x => x.Id).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DayVisitorExists(id))
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

        // POST: api/DayVisitor
        // Para crear un nuevo registro de visitante.
        [HttpPost]
        public async Task<ActionResult<DayVisitor>> PostDayVisitor(DayVisitor dayVisitor)
        {
             if (_context.DayVisitors == null)
            {
                 return Problem("La entidad 'DayVisitors' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(dayVisitor.NVisit < 0)
             {
                return BadRequest("El número de visitas (NVisit) no puede ser negativo.");
             }
             // Asegura que Id no se envíe o sea 0 si es autogenerado por la BD
             // dayVisitor.Id = 0; // Descomentar si Id es Identity y quieres asegurarte

            _context.DayVisitors.Add(dayVisitor);
            try
            {
                 // El Id probablemente será generado por la base de datos al guardar.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar otros posibles errores de base de datos
                 return Problem($"Error al guardar el registro de visitante: {ex.InnerException?.Message ?? ex.Message}");
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetDayVisitor), new { id = dayVisitor.Id }, dayVisitor);
        }

        // DELETE: api/DayVisitor/{id}
        // Elimina un registro de visitante.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDayVisitor(int id)
        {
             if (_context.DayVisitors == null)
            {
                 return NotFound("La entidad 'DayVisitors' no está configurada en el DbContext.");
            }

            var dayVisitor = await _context.DayVisitors.FindAsync(id);
            if (dayVisitor == null)
            {
                return NotFound();
            }

            _context.DayVisitors.Remove(dayVisitor);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro de visitante existe por su ID
        private bool DayVisitorExists(int id)
        {
            return (_context.DayVisitors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}