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
    public class CampSponsorController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampSponsorController> _logger;

        public CampSponsorController(ExtranetContext context, IConfiguration configuration, ILogger<CampSponsorController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampSponsor
        // Obtiene todos los sponsors.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampSponsor>>> GetCampSponsors()
        {
            if (_context.CampSponsors == null)
            {
                return NotFound("La entidad 'CampSponsors' no está configurada en el DbContext.");
            }
            // Considera ordenar o paginar si la lista es larga.
            return await _context.CampSponsors.ToListAsync();
        }

        // GET: api/CampSponsor/{id}
        // Busca un sponsor específico por su clave primaria IdSponsor (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampSponsor>> GetCampSponsor(int id)
        {
            if (_context.CampSponsors == null)
            {
                 return NotFound("La entidad 'CampSponsors' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campSponsor = await _context.CampSponsors.FindAsync(id);

            if (campSponsor == null)
            {
                return NotFound();
            }

            return campSponsor;
        }

        // PUT: api/CampSponsor/{id}
        // Para actualizar un sponsor existente.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampSponsor(int id, CampSponsor campSponsor)
        {
            if (id != campSponsor.IdSponsor)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del sponsor.");
            }

             if (_context.CampSponsors == null)
            {
                 return NotFound("La entidad 'CampSponsors' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (IdSponsor)
            _context.Entry(campSponsor).State = EntityState.Modified;
            _context.Entry(campSponsor).Property(x => x.IdSponsor).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampSponsorExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (IdSponsor). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampSponsor
        // Para crear un nuevo sponsor.
        [HttpPost]
        public async Task<ActionResult<CampSponsor>> PostCampSponsor(CampSponsor campSponsor)
        {
             if (_context.CampSponsors == null)
            {
                 return Problem("La entidad 'CampSponsors' no está configurada en el DbContext.");
            }

             // Asegura que IdSponsor no se envíe o sea 0 si es autogenerado por la BD
             // campSponsor.IdSponsor = 0; // Descomentar si IdSponsor es Identity y quieres asegurarte

            _context.CampSponsors.Add(campSponsor);
            try
            {
                 // El IdSponsor probablemente será generado por la base de datos al guardar.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar otros posibles errores de base de datos
                 return Problem($"Error al guardar el sponsor: {ex.InnerException?.Message ?? ex.Message}");
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampSponsor), new { id = campSponsor.IdSponsor }, campSponsor);
        }

        // DELETE: api/CampSponsor/{id}
        // Elimina un sponsor.
        // Nota: Podría fallar si existen pagos u otros registros asociados a este sponsor.
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampSponsor(int id)
        {
             if (_context.CampSponsors == null)
            {
                 return NotFound("La entidad 'CampSponsors' no está configurada en el DbContext.");
            }

            var campSponsor = await _context.CampSponsors.FindAsync(id);
            if (campSponsor == null)
            {
                return NotFound();
            }

            _context.CampSponsors.Remove(campSponsor);
             try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                  return Problem($"No se pudo eliminar el sponsor. Es posible que tenga registros asociados. Error: {ex.InnerException?.Message ?? ex.Message}");
            }


            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un sponsor existe por su ID
        private bool CampSponsorExists(int id)
        {
            return (_context.CampSponsors?.Any(e => e.IdSponsor == id)).GetValueOrDefault();
        }
    }
}