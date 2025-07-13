using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uf.cd.db.Model; // Namespace del modelo

namespace uf.cd.api.Controllers // Namespace especificado para el controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampAlumnosNovController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampAlumnosNovController> _logger;

        public CampAlumnosNovController(ExtranetContext context, IConfiguration configuration, ILogger<CampAlumnosNovController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampAlumnosNov
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampAlumnosNov>>> GetCampAlumnosNovs() // Asumiendo DbSet se llama CampAlumnosNovs
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampAlumnosNovs == null)
            {
                return NotFound("La entidad 'CampAlumnosNovs' no está configurada en el DbContext.");
            }
            // Nota: Devolver toda la tabla puede ser ineficiente. Considera paginación.
            return await _context.CampAlumnosNovs.ToListAsync();
        }

        // GET: api/CampAlumnosNov/{idAlumno}/{idCarrera}/{lu}
        // Busca por la clave primaria compuesta asumida
        [HttpGet("{idAlumno}/{idCarrera}/{lu}")]
        public async Task<ActionResult<CampAlumnosNov>> GetCampAlumnosNov(string idAlumno, int idCarrera, int lu)
        {
            if (_context.CampAlumnosNovs == null)
            {
                 return NotFound("La entidad 'CampAlumnosNovs' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // Ajusta el orden si la definición de la clave en OnModelCreating es diferente
            var campAlumnosNov = await _context.CampAlumnosNovs.FindAsync(idAlumno, idCarrera, lu);

            if (campAlumnosNov == null)
            {
                return NotFound();
            }

            return campAlumnosNov;
        }

        // PUT: api/CampAlumnosNov/{idAlumno}/{idCarrera}/{lu}
        // Para actualizar un registro existente usando la clave compuesta
        [HttpPut("{idAlumno}/{idCarrera}/{lu}")]
        public async Task<IActionResult> PutCampAlumnosNov(string idAlumno, int idCarrera, int lu, CampAlumnosNov campAlumnosNov)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campAlumnosNov.CanvIdAlumno ||
                idCarrera != campAlumnosNov.CanvIdCarrera ||
                lu != campAlumnosNov.CanvLu)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampAlumnosNovs == null)
            {
                 return NotFound("La entidad 'CampAlumnosNovs' no está configurada en el DbContext.");
            }

            _context.Entry(campAlumnosNov).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampAlumnosNovExists(idAlumno, idCarrera, lu))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampAlumnosNov
        // Para crear un nuevo registro
        [HttpPost]
        public async Task<ActionResult<CampAlumnosNov>> PostCampAlumnosNov(CampAlumnosNov campAlumnosNov)
        {
             if (_context.CampAlumnosNovs == null)
            {
                 return Problem("La entidad 'CampAlumnosNovs' no está configurada en el DbContext.");
            }

            _context.CampAlumnosNovs.Add(campAlumnosNov);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampAlumnosNovExists(campAlumnosNov.CanvIdAlumno, campAlumnosNov.CanvIdCarrera, campAlumnosNov.CanvLu))
                {
                    return Conflict("Ya existe un registro con esta clave compuesta.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampAlumnosNov), new {
                idAlumno = campAlumnosNov.CanvIdAlumno,
                idCarrera = campAlumnosNov.CanvIdCarrera,
                lu = campAlumnosNov.CanvLu
                }, campAlumnosNov);
        }

        // DELETE: api/CampAlumnosNov/{idAlumno}/{idCarrera}/{lu}
        // Elimina por clave primaria compuesta asumida
        [HttpDelete("{idAlumno}/{idCarrera}/{lu}")]
        public async Task<IActionResult> DeleteCampAlumnosNov(string idAlumno, int idCarrera, int lu)
        {
             if (_context.CampAlumnosNovs == null)
            {
                 return NotFound("La entidad 'CampAlumnosNovs' no está configurada en el DbContext.");
            }

            var campAlumnosNov = await _context.CampAlumnosNovs.FindAsync(idAlumno, idCarrera, lu);
            if (campAlumnosNov == null)
            {
                return NotFound();
            }

            _context.CampAlumnosNovs.Remove(campAlumnosNov);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampAlumnosNovExists(string idAlumno, int idCarrera, int lu)
        {
            // Asegúrate que CampAlumnosNovs no sea null antes de usarlo
            return (_context.CampAlumnosNovs?.Any(e =>
                e.CanvIdAlumno == idAlumno &&
                e.CanvIdCarrera == idCarrera &&
                e.CanvLu == lu)).GetValueOrDefault();
        }
    }
}