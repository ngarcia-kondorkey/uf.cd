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
    public class CampAusenciaController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampAusenciaController> _logger;

        public CampAusenciaController(ExtranetContext context, IConfiguration configuration, ILogger<CampAusenciaController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampAusencia
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampAusencia>>> GetCampAusencias() // Asumiendo DbSet se llama CampAusencias
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampAusencias == null)
            {
                return NotFound("La entidad 'CampAusencias' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todas las ausencias puede ser ineficiente. Considera paginación/filtrado.
            return await _context.CampAusencias.ToListAsync();
        }

        // GET: api/CampAusencia/{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}/{fecha}/{tipo}
        // Busca por la clave primaria compuesta asumida.
        // Nota: Pasar DateTime en la ruta puede requerir un formato específico (ej: ISO 8601 YYYY-MM-DD).
        [HttpGet("{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}/{fecha}/{tipo}")]
        public async Task<ActionResult<CampAusencia>> GetCampAusencia(string idAlumno, int idCarrera, int lu, string asignatura, string ciclo, DateTime fecha, string tipo)
        {
            if (_context.CampAusencias == null)
            {
                 return NotFound("La entidad 'CampAusencias' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campAusencia = await _context.CampAusencias.FindAsync(idAlumno, idCarrera, lu, asignatura, ciclo, fecha, tipo);

            if (campAusencia == null)
            {
                return NotFound();
            }

            return campAusencia;
        }

        // PUT: api/CampAusencia/{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}/{fecha}/{tipo}
        // Para actualizar un registro existente usando la clave compuesta
        [HttpPut("{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}/{fecha}/{tipo}")]
        public async Task<IActionResult> PutCampAusencia(string idAlumno, int idCarrera, int lu, string asignatura, string ciclo, DateTime fecha, string tipo, CampAusencia campAusencia)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campAusencia.CausIdAlumno ||
                idCarrera != campAusencia.CausIdCarrera ||
                lu != campAusencia.CausLu ||
                asignatura != campAusencia.CausAsignatura ||
                ciclo != campAusencia.CausCiclo ||
                fecha != campAusencia.CausFecha ||
                tipo != campAusencia.CausTipo)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampAusencias == null)
            {
                 return NotFound("La entidad 'CampAusencias' no está configurada en el DbContext.");
            }

            _context.Entry(campAusencia).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampAusenciaExists(idAlumno, idCarrera, lu, asignatura, ciclo, fecha, tipo))
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

        // POST: api/CampAusencia
        // Para crear un nuevo registro de ausencia
        [HttpPost]
        public async Task<ActionResult<CampAusencia>> PostCampAusencia(CampAusencia campAusencia)
        {
             if (_context.CampAusencias == null)
            {
                 return Problem("La entidad 'CampAusencias' no está configurada en el DbContext.");
            }

            _context.CampAusencias.Add(campAusencia);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampAusenciaExists(campAusencia.CausIdAlumno, campAusencia.CausIdCarrera, campAusencia.CausLu, campAusencia.CausAsignatura, campAusencia.CausCiclo, campAusencia.CausFecha, campAusencia.CausTipo))
                {
                    return Conflict("Ya existe un registro de ausencia con esta clave compuesta.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampAusencia), new {
                idAlumno = campAusencia.CausIdAlumno,
                idCarrera = campAusencia.CausIdCarrera,
                lu = campAusencia.CausLu,
                asignatura = campAusencia.CausAsignatura,
                ciclo = campAusencia.CausCiclo,
                fecha = campAusencia.CausFecha, // Asegúrate que el formato sea compatible con la ruta
                tipo = campAusencia.CausTipo
                }, campAusencia);
        }

        // DELETE: api/CampAusencia/{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}/{fecha}/{tipo}
        // Elimina por clave primaria compuesta asumida
        [HttpDelete("{idAlumno}/{idCarrera}/{lu}/{asignatura}/{ciclo}/{fecha}/{tipo}")]
        public async Task<IActionResult> DeleteCampAusencia(string idAlumno, int idCarrera, int lu, string asignatura, string ciclo, DateTime fecha, string tipo)
        {
             if (_context.CampAusencias == null)
            {
                 return NotFound("La entidad 'CampAusencias' no está configurada en el DbContext.");
            }

            var campAusencia = await _context.CampAusencias.FindAsync(idAlumno, idCarrera, lu, asignatura, ciclo, fecha, tipo);
            if (campAusencia == null)
            {
                return NotFound();
            }

            _context.CampAusencias.Remove(campAusencia);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampAusenciaExists(string idAlumno, int idCarrera, int lu, string asignatura, string ciclo, DateTime fecha, string tipo)
        {
            // Asegúrate que CampAusencias no sea null antes de usarlo
            return (_context.CampAusencias?.Any(e =>
                e.CausIdAlumno == idAlumno &&
                e.CausIdCarrera == idCarrera &&
                e.CausLu == lu &&
                e.CausAsignatura == asignatura &&
                e.CausCiclo == ciclo &&
                e.CausFecha == fecha &&
                e.CausTipo == tipo)).GetValueOrDefault();
        }
    }
}