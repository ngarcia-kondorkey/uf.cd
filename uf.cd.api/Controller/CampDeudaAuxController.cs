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
    public class CampDeudaAuxController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampDeudaAuxController> _logger;

        public CampDeudaAuxController(ExtranetContext context, IConfiguration configuration, ILogger<CampDeudaAuxController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampDeudaAux
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampDeudaAux>>> GetCampDeudaAuxes() // Asumiendo DbSet se llama CampDeudaAuxes
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampDeudaAuxes == null)
            {
                return NotFound("La entidad 'CampDeudaAuxes' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los registros de deuda puede ser ineficiente y exponer datos.
            // Considera aplicar filtros o paginación obligatoria.
            return await _context.CampDeudaAuxes.ToListAsync();
        }

        // GET: api/CampDeudaAux/{id}
        // Busca por la clave primaria IdFactura (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampDeudaAux>> GetCampDeudaAux(int id)
        {
            if (_context.CampDeudaAuxes == null)
            {
                 return NotFound("La entidad 'CampDeudaAuxes' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campDeudaAux = await _context.CampDeudaAuxes.FindAsync(id);

            if (campDeudaAux == null)
            {
                return NotFound();
            }

            return campDeudaAux;
        }

        // PUT: api/CampDeudaAux/{id}
        // Para actualizar un registro de deuda existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampDeudaAux(int id, CampDeudaAux campDeudaAux)
        {
            // Valida que el ID de la ruta coincida con el ID del objeto
            if (id != campDeudaAux.IdFactura)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la factura.");
            }

             if (_context.CampDeudaAuxes == null)
            {
                 return NotFound("La entidad 'CampDeudaAuxes' no está configurada en el DbContext.");
            }

             // Solo permite modificar campos que no son parte de la clave primaria (IdFactura)
            _context.Entry(campDeudaAux).State = EntityState.Modified;
            _context.Entry(campDeudaAux).Property(x => x.IdFactura).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampDeudaAuxExists(id))
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

        // POST: api/CampDeudaAux
        // Para crear un nuevo registro de deuda auxiliar
        [HttpPost]
        public async Task<ActionResult<CampDeudaAux>> PostCampDeudaAux(CampDeudaAux campDeudaAux)
        {
             if (_context.CampDeudaAuxes == null)
            {
                 return Problem("La entidad 'CampDeudaAuxes' no está configurada en el DbContext.");
            }

            _context.CampDeudaAuxes.Add(campDeudaAux);
            try
            {
                 // Asume que IdFactura es autoincremental o se provee un valor único.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si IdFactura NO es autogenerado y ya existe
                 if (CampDeudaAuxExists(campDeudaAux.IdFactura))
                 {
                     return Conflict($"Ya existe un registro de deuda con IdFactura {campDeudaAux.IdFactura}");
                 }
                 else
                 {
                    // O podría ser otro error de constraint, etc.
                    return Problem($"Error al guardar el registro de deuda: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampDeudaAux), new { id = campDeudaAux.IdFactura }, campDeudaAux);
        }

        // DELETE: api/CampDeudaAux/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampDeudaAux(int id)
        {
             if (_context.CampDeudaAuxes == null)
            {
                 return NotFound("La entidad 'CampDeudaAuxes' no está configurada en el DbContext.");
            }

            var campDeudaAux = await _context.CampDeudaAuxes.FindAsync(id);
            if (campDeudaAux == null)
            {
                return NotFound();
            }

            _context.CampDeudaAuxes.Remove(campDeudaAux);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su ID
        private bool CampDeudaAuxExists(int id)
        {
            // Asegúrate que CampDeudaAuxes no sea null antes de usarlo
            return (_context.CampDeudaAuxes?.Any(e => e.IdFactura == id)).GetValueOrDefault();
        }
    }
}