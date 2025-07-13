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
    public class CampDeudaPrevController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampDeudaPrevController> _logger;

        public CampDeudaPrevController(ExtranetContext context, IConfiguration configuration, ILogger<CampDeudaPrevController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampDeudaPrev
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampDeudaPrev>>> GetCampDeudaPrevs() // Asumiendo DbSet se llama CampDeudaPrevs
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampDeudaPrevs == null)
            {
                return NotFound("La entidad 'CampDeudaPrevs' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los registros puede ser ineficiente y exponer datos.
            // Considera aplicar filtros o paginación obligatoria.
            return await _context.CampDeudaPrevs.ToListAsync();
        }

        // GET: api/CampDeudaPrev/{id}
        // Busca por la clave primaria IdFactura (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampDeudaPrev>> GetCampDeudaPrev(int id)
        {
            if (_context.CampDeudaPrevs == null)
            {
                 return NotFound("La entidad 'CampDeudaPrevs' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campDeudaPrev = await _context.CampDeudaPrevs.FindAsync(id);

            if (campDeudaPrev == null)
            {
                return NotFound();
            }

            return campDeudaPrev;
        }

        // PUT: api/CampDeudaPrev/{id}
        // Para actualizar un registro de deuda "previa" existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampDeudaPrev(int id, CampDeudaPrev campDeudaPrev)
        {
            // Valida que el ID de la ruta coincida con el ID del objeto
            if (id != campDeudaPrev.IdFactura)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la factura.");
            }

             if (_context.CampDeudaPrevs == null)
            {
                 return NotFound("La entidad 'CampDeudaPrevs' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (IdFactura)
            _context.Entry(campDeudaPrev).State = EntityState.Modified;
            _context.Entry(campDeudaPrev).Property(x => x.IdFactura).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampDeudaPrevExists(id))
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

        // POST: api/CampDeudaPrev
        // Para crear un nuevo registro de deuda "previa"
        [HttpPost]
        public async Task<ActionResult<CampDeudaPrev>> PostCampDeudaPrev(CampDeudaPrev campDeudaPrev)
        {
             if (_context.CampDeudaPrevs == null)
            {
                 return Problem("La entidad 'CampDeudaPrevs' no está configurada en el DbContext.");
            }

            _context.CampDeudaPrevs.Add(campDeudaPrev);
            try
            {
                 // Asume que IdFactura es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si IdFactura NO es autogenerado y ya existe
                 if (CampDeudaPrevExists(campDeudaPrev.IdFactura))
                 {
                     return Conflict($"Ya existe un registro de deuda 'prev' con IdFactura {campDeudaPrev.IdFactura}");
                 }
                 else
                 {
                    // O podría ser otro error de constraint, etc.
                    return Problem($"Error al guardar el registro de deuda 'prev': {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampDeudaPrev), new { id = campDeudaPrev.IdFactura }, campDeudaPrev);
        }

        // DELETE: api/CampDeudaPrev/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampDeudaPrev(int id)
        {
             if (_context.CampDeudaPrevs == null)
            {
                 return NotFound("La entidad 'CampDeudaPrevs' no está configurada en el DbContext.");
            }

            var campDeudaPrev = await _context.CampDeudaPrevs.FindAsync(id);
            if (campDeudaPrev == null)
            {
                return NotFound();
            }

            _context.CampDeudaPrevs.Remove(campDeudaPrev);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su ID
        private bool CampDeudaPrevExists(int id)
        {
            // Asegúrate que CampDeudaPrevs no sea null antes de usarlo
            return (_context.CampDeudaPrevs?.Any(e => e.IdFactura == id)).GetValueOrDefault();
        }
    }
}