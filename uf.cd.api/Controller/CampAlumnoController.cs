using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uf.cd.db.Model; // Asegúrate que este namespace sea correcto

namespace uf.cd.api.Controllers // Reemplaza YourApiProject con el namespace de tu proyecto
{
    [Route("api/[controller]")]
    [ApiController]
    public class CampAlumnoController : ControllerBase
    {

        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampAlumnoController> _logger;

        public CampAlumnoController(ExtranetContext context, IConfiguration configuration, ILogger<CampAlumnoController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }
        // --- Fin Reemplazo ---

        // GET: api/CampAlumno
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampAlumno>>> GetCampAlumnos()
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampAlumnos == null)
            {
                return NotFound("La entidad 'CampAlumnos' no está configurada en el DbContext.");
            }
            return await _context.CampAlumnos.ToListAsync();
        }

        // GET: api/CampAlumno/{id}
        // Asumiendo que CaluIdAlumno es la clave primaria (PK) y es string
        [HttpGet("{id}")]
        public async Task<ActionResult<CampAlumno>> GetCampAlumno(string id)
        {
            if (_context.CampAlumnos == null)
            {
                 return NotFound("La entidad 'CampAlumnos' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria. Ajusta si la PK es diferente o compuesta.
            var campAlumno = await _context.CampAlumnos.Where(c => c.CaluIdAlumno == id).FirstOrDefaultAsync();

            if (campAlumno == null)
            {
                return NotFound();
            }

            return campAlumno;
        }

        // PUT: api/CampAlumno/{id}
        // Para actualizar un registro existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampAlumno(string id, CampAlumno campAlumno)
        {
            // Valida que el ID de la ruta coincida con el ID del objeto
            if (id != campAlumno.CaluIdAlumno)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del alumno.");
            }

             if (_context.CampAlumnos == null)
            {
                 return NotFound("La entidad 'CampAlumnos' no está configurada en el DbContext.");
            }

            _context.Entry(campAlumno).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampAlumnoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Relanza la excepción si ocurrió otro error de concurrencia
                }
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampAlumno
        // Para crear un nuevo registro
        [HttpPost]
        public async Task<ActionResult<CampAlumno>> PostCampAlumno(CampAlumno campAlumno)
        {
             if (_context.CampAlumnos == null)
            {
                 return Problem("La entidad 'CampAlumnos' no está configurada en el DbContext.");
            }

            _context.CampAlumnos.Add(campAlumno);
            try
            {
                // Asumiendo que CaluIdAlumno podría no ser autogenerado y viene en el request
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Podría ser una violación de PK si el ID ya existe
                if (CampAlumnoExists(campAlumno.CaluIdAlumno))
                {
                    return Conflict($"Ya existe un alumno con el ID {campAlumno.CaluIdAlumno}");
                }
                else
                {
                    throw; // Relanza si es otro tipo de error de base de datos
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampAlumno), new { id = campAlumno.CaluIdAlumno }, campAlumno);
        }

        // DELETE: api/CampAlumno/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampAlumno(string id)
        {
             if (_context.CampAlumnos == null)
            {
                 return NotFound("La entidad 'CampAlumnos' no está configurada en el DbContext.");
            }

            var campAlumno = await _context.CampAlumnos.Where(c => c.CaluIdAlumno == id).FirstOrDefaultAsync();
            if (campAlumno == null)
            {
                return NotFound();
            }

            _context.CampAlumnos.Remove(campAlumno);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un alumno existe por su ID
        private bool CampAlumnoExists(string id)
        {
            // Asegúrate que CampAlumnos no sea null antes de usarlo
            return (_context.CampAlumnos?.Any(e => e.CaluIdAlumno == id)).GetValueOrDefault();
        }
    }
}