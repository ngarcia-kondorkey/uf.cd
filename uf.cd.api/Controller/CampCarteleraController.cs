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
    public class CampCarteleraController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampCarteleraController> _logger;

        public CampCarteleraController(ExtranetContext context, IConfiguration configuration, ILogger<CampCarteleraController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampCartelera
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampCartelera>>> GetCampCarteleras() // Asumiendo DbSet se llama CampCarteleras
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampCarteleras == null)
            {
                return NotFound("La entidad 'CampCarteleras' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver toda la cartelera puede ser ineficiente. Considera paginación/filtrado.
            return await _context.CampCarteleras.ToListAsync();
        }

        // GET: api/CampCartelera/{idCarrera}/{idMateria}/{fecha}/{cohorte}/{asignatura}
        // Busca por la clave primaria compuesta asumida.
        // Nota: Pasar DateTime en la ruta puede requerir un formato específico (ej: ISO 8601 YYYY-MM-DD).
        // Considera también codificar 'asignatura' si puede contener caracteres especiales para URL.
        [HttpGet("{idCarrera}/{idMateria}/{fecha}/{cohorte}/{asignatura}")]
        public async Task<ActionResult<CampCartelera>> GetCampCartelera(int idCarrera, int idMateria, DateTime fecha, string cohorte, string asignatura)
        {
            if (_context.CampCarteleras == null)
            {
                 return NotFound("La entidad 'CampCarteleras' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campCartelera = await _context.CampCarteleras.FindAsync(idCarrera, idMateria, fecha, cohorte, asignatura);

            if (campCartelera == null)
            {
                return NotFound();
            }

            return campCartelera;
        }

        // PUT: api/CampCartelera/{idCarrera}/{idMateria}/{fecha}/{cohorte}/{asignatura}
        // Para actualizar un registro existente usando la clave compuesta
        [HttpPut("{idCarrera}/{idMateria}/{fecha}/{cohorte}/{asignatura}")]
        public async Task<IActionResult> PutCampCartelera(int idCarrera, int idMateria, DateTime fecha, string cohorte, string asignatura, CampCartelera campCartelera)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idCarrera != campCartelera.CcarIdCarrera ||
                idMateria != campCartelera.CcarIdMateria ||
                fecha != campCartelera.CcarFecha ||
                cohorte != campCartelera.CcarCohorte ||
                asignatura != campCartelera.CcarAsignatura)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampCarteleras == null)
            {
                 return NotFound("La entidad 'CampCarteleras' no está configurada en el DbContext.");
            }

            _context.Entry(campCartelera).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampCarteleraExists(idCarrera, idMateria, fecha, cohorte, asignatura))
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

        // POST: api/CampCartelera
        // Para crear un nuevo registro en la cartelera
        [HttpPost]
        public async Task<ActionResult<CampCartelera>> PostCampCartelera(CampCartelera campCartelera)
        {
             if (_context.CampCarteleras == null)
            {
                 return Problem("La entidad 'CampCarteleras' no está configurada en el DbContext.");
            }

            _context.CampCarteleras.Add(campCartelera);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampCarteleraExists(campCartelera.CcarIdCarrera, campCartelera.CcarIdMateria, campCartelera.CcarFecha, campCartelera.CcarCohorte, campCartelera.CcarAsignatura))
                {
                    return Conflict("Ya existe un registro en la cartelera con esta clave compuesta.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampCartelera), new {
                idCarrera = campCartelera.CcarIdCarrera,
                idMateria = campCartelera.CcarIdMateria,
                fecha = campCartelera.CcarFecha, // Asegúrate que el formato sea compatible con la ruta
                cohorte = campCartelera.CcarCohorte,
                asignatura = campCartelera.CcarAsignatura
                }, campCartelera);
        }

        // DELETE: api/CampCartelera/{idCarrera}/{idMateria}/{fecha}/{cohorte}/{asignatura}
        // Elimina por clave primaria compuesta asumida
        [HttpDelete("{idCarrera}/{idMateria}/{fecha}/{cohorte}/{asignatura}")]
        public async Task<IActionResult> DeleteCampCartelera(int idCarrera, int idMateria, DateTime fecha, string cohorte, string asignatura)
        {
             if (_context.CampCarteleras == null)
            {
                 return NotFound("La entidad 'CampCarteleras' no está configurada en el DbContext.");
            }

            var campCartelera = await _context.CampCarteleras.FindAsync(idCarrera, idMateria, fecha, cohorte, asignatura);
            if (campCartelera == null)
            {
                return NotFound();
            }

            _context.CampCarteleras.Remove(campCartelera);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampCarteleraExists(int idCarrera, int idMateria, DateTime fecha, string cohorte, string asignatura)
        {
            // Asegúrate que CampCarteleras no sea null antes de usarlo
            return (_context.CampCarteleras?.Any(e =>
                e.CcarIdCarrera == idCarrera &&
                e.CcarIdMateria == idMateria &&
                e.CcarFecha == fecha &&
                e.CcarCohorte == cohorte &&
                e.CcarAsignatura == asignatura)).GetValueOrDefault();
        }
    }
}