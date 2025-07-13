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
    public class EntidadesBancariaController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<EntidadesBancariaController> _logger;

        public EntidadesBancariaController(ExtranetContext context, IConfiguration configuration, ILogger<EntidadesBancariaController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/EntidadesBancaria
        // Obtiene todas las entidades bancarias.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EntidadesBancaria>>> GetEntidadesBancarias()
        {
            if (_context.EntidadesBancarias == null)
            {
                return NotFound("La entidad 'EntidadesBancarias' no está configurada en el DbContext.");
            }
             // Considera ordenar por nombre de entidad.
            return await _context.EntidadesBancarias.OrderBy(e => e.Entidad).ToListAsync();
        }

        // GET: api/EntidadesBancaria/{id}
        // Busca una entidad bancaria específica por su clave primaria Id (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<EntidadesBancaria>> GetEntidadBancaria(int id)
        {
            if (_context.EntidadesBancarias == null)
            {
                 return NotFound("La entidad 'EntidadesBancarias' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var entidadBancaria = await _context.EntidadesBancarias.FindAsync(id);

            if (entidadBancaria == null)
            {
                return NotFound();
            }

            return entidadBancaria;
        }

        // PUT: api/EntidadesBancaria/{id}
        // Para actualizar una entidad bancaria existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEntidadBancaria(int id, EntidadesBancaria entidadBancaria)
        {
            if (id != entidadBancaria.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la entidad bancaria.");
            }

             if (_context.EntidadesBancarias == null)
            {
                 return NotFound("La entidad 'EntidadesBancarias' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (Id)
            _context.Entry(entidadBancaria).State = EntityState.Modified;
            _context.Entry(entidadBancaria).Property(x => x.Id).IsModified = false; // Previene modificación PK

            // Opcional: Actualizar CreatedAt si se desea registrar la modificación
            // entidadBancaria.CreatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntidadBancariaExists(id))
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

        // POST: api/EntidadesBancaria
        // Para crear una nueva entidad bancaria.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<EntidadesBancaria>> PostEntidadBancaria(EntidadesBancaria entidadBancaria)
        {
             if (_context.EntidadesBancarias == null)
            {
                 return Problem("La entidad 'EntidadesBancarias' no está configurada en el DbContext.");
            }

            // Opcional: Establecer CreatedAt al crear
            if (!entidadBancaria.CreatedAt.HasValue)
            {
                entidadBancaria.CreatedAt = DateTime.UtcNow; // O DateTime.Now según zona horaria
            }
            // Opcional: Asegurar que Id no se envíe o sea 0 si es autogenerado por la BD
            // entidadBancaria.Id = 0;

            _context.EntidadesBancarias.Add(entidadBancaria);
            try
            {
                 // El Id probablemente será generado por la base de datos al guardar.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar otros posibles errores de base de datos
                 return Problem($"Error al guardar la entidad bancaria: {ex.InnerException?.Message ?? ex.Message}");
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetEntidadBancaria), new { id = entidadBancaria.Id }, entidadBancaria);
        }

        // DELETE: api/EntidadesBancaria/{id}
        // Elimina una entidad bancaria.
        // Nota: Podría fallar si existen cuentas bancarias (u otros registros) asociados.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEntidadBancaria(int id)
        {
             if (_context.EntidadesBancarias == null)
            {
                 return NotFound("La entidad 'EntidadesBancarias' no está configurada en el DbContext.");
            }

            var entidadBancaria = await _context.EntidadesBancarias.FindAsync(id);
            if (entidadBancaria == null)
            {
                return NotFound();
            }

            _context.EntidadesBancarias.Remove(entidadBancaria);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                 // (ej. si la tabla CampCbu tiene una FK hacia EntidadesBancaria.Id)
                  return Problem($"No se pudo eliminar la entidad bancaria. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }


            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si una entidad bancaria existe por su ID
        private bool EntidadBancariaExists(int id)
        {
            return (_context.EntidadesBancarias?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}