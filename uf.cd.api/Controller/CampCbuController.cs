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
    public class CampCbuController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampCbuController> _logger;

        public CampCbuController(ExtranetContext context, IConfiguration configuration, ILogger<CampCbuController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampCbu
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampCbu>>> GetCampCbus() // Asumiendo DbSet se llama CampCbus
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampCbus == null)
            {
                return NotFound("La entidad 'CampCbus' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los registros CBU puede ser ineficiente y exponer datos sensibles.
            // Considera aplicar filtros o devolver sólo datos necesarios.
            return await _context.CampCbus.ToListAsync();
        }

        // GET: api/CampCbu/{idCarrera}/{lu}/{idAlumno}
        // Busca por la clave primaria compuesta asumida.
        [HttpGet("{idCarrera}/{lu}/{idAlumno}")]
        public async Task<ActionResult<CampCbu>> GetCampCbu(int idCarrera, int lu, string idAlumno)
        {
            if (_context.CampCbus == null)
            {
                 return NotFound("La entidad 'CampCbus' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campCbu = await _context.CampCbus.FindAsync(idCarrera, lu, idAlumno);

            if (campCbu == null)
            {
                return NotFound();
            }

            return campCbu;
        }

        // PUT: api/CampCbu/{idCarrera}/{lu}/{idAlumno}
        // Para actualizar un registro CBU existente usando la clave compuesta
        [HttpPut("{idCarrera}/{lu}/{idAlumno}")]
        public async Task<IActionResult> PutCampCbu(int idCarrera, int lu, string idAlumno, CampCbu campCbu)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idCarrera != campCbu.Idcarrera ||
                lu != campCbu.Lu ||
                idAlumno != campCbu.Idalumno)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto CBU.");
            }

             if (_context.CampCbus == null)
            {
                 return NotFound("La entidad 'CampCbus' no está configurada en el DbContext.");
            }

            _context.Entry(campCbu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampCbuExists(idCarrera, lu, idAlumno))
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

        // POST: api/CampCbu
        // Para crear un nuevo registro CBU
        [HttpPost]
        public async Task<ActionResult<CampCbu>> PostCampCbu(CampCbu campCbu)
        {
             if (_context.CampCbus == null)
            {
                 return Problem("La entidad 'CampCbus' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales antes de agregar
             // if(string.IsNullOrWhiteSpace(campCbu.Cbu) || campCbu.Cbu.Length != 22) {
             //     return BadRequest("El CBU proporcionado no es válido.");
             // }

            _context.CampCbus.Add(campCbu);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampCbuExists(campCbu.Idcarrera, campCbu.Lu, campCbu.Idalumno))
                {
                    return Conflict("Ya existe un registro CBU para este alumno en esta carrera.");
                }
                else
                {
                    throw; // Relanza si es otro error
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampCbu), new {
                idCarrera = campCbu.Idcarrera,
                lu = campCbu.Lu,
                idAlumno = campCbu.Idalumno
                }, campCbu);
        }

        // DELETE: api/CampCbu/{idCarrera}/{lu}/{idAlumno}
        // Elimina por clave primaria compuesta asumida
        [HttpDelete("{idCarrera}/{lu}/{idAlumno}")]
        public async Task<IActionResult> DeleteCampCbu(int idCarrera, int lu, string idAlumno)
        {
             if (_context.CampCbus == null)
            {
                 return NotFound("La entidad 'CampCbus' no está configurada en el DbContext.");
            }

            var campCbu = await _context.CampCbus.FindAsync(idCarrera, lu, idAlumno);
            if (campCbu == null)
            {
                return NotFound();
            }

            _context.CampCbus.Remove(campCbu);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampCbuExists(int idCarrera, int lu, string idAlumno)
        {
            // Asegúrate que CampCbus no sea null antes de usarlo
            return (_context.CampCbus?.Any(e =>
                e.Idcarrera == idCarrera &&
                e.Lu == lu &&
                e.Idalumno == idAlumno)).GetValueOrDefault();
        }
    }
}