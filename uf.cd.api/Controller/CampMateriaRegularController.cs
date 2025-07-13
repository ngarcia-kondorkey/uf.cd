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
    public class CampMateriaRegularController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampMateriaRegularController> _logger;

        public CampMateriaRegularController(ExtranetContext context, IConfiguration configuration, ILogger<CampMateriaRegularController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampMateriaRegular
        // Obtiene todos los registros de regularidad de materias por alumno.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampMateriaRegular>>> GetCampMateriasRegulares()
        {
            if (_context.CampMateriaRegulars == null)
            {
                return NotFound("La entidad 'CampMateriasRegulares' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todos los registros puede ser ineficiente.
             // Considera paginación y filtrado (ej: por alumno).
            return await _context.CampMateriaRegulars.ToListAsync();
        }

        // GET: api/CampMateriaRegular/{idAlumno}/{idCarrera}/{lu}/{clMateria}
        // Busca un registro específico por la clave primaria compuesta asumida.
        // Nota: Considera codificar 'clMateria' si puede contener caracteres especiales para URL.
        [HttpGet("{idAlumno}/{idCarrera}/{lu}/{clMateria}")]
        public async Task<ActionResult<CampMateriaRegular>> GetCampMateriaRegular(string idAlumno, int idCarrera, int lu, string clMateria)
        {
            if (_context.CampMateriaRegulars == null)
            {
                 return NotFound("La entidad 'CampMateriasRegulares' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campMateriaRegular = await _context.CampMateriaRegulars.FindAsync(idAlumno, idCarrera, lu, clMateria);

            if (campMateriaRegular == null)
            {
                return NotFound();
            }

            return campMateriaRegular;
        }

        // PUT: api/CampMateriaRegular/{idAlumno}/{idCarrera}/{lu}/{clMateria}
        // Para actualizar un registro existente usando la clave compuesta (ej: cambiar estado).
        [HttpPut("{idAlumno}/{idCarrera}/{lu}/{clMateria}")]
        public async Task<IActionResult> PutCampMateriaRegular(string idAlumno, int idCarrera, int lu, string clMateria, CampMateriaRegular campMateriaRegular)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campMateriaRegular.CmarIdAlumno ||
                idCarrera != campMateriaRegular.CmarIdCarrera ||
                lu != campMateriaRegular.CmarLu ||
                clMateria != campMateriaRegular.CmarClMateria)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampMateriaRegulars == null)
            {
                 return NotFound("La entidad 'CampMateriasRegulares' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campMateriaRegular).State = EntityState.Modified;
            _context.Entry(campMateriaRegular).Property(x => x.CmarIdAlumno).IsModified = false; // Previene modificación PK
            _context.Entry(campMateriaRegular).Property(x => x.CmarIdCarrera).IsModified = false; // Previene modificación PK
            _context.Entry(campMateriaRegular).Property(x => x.CmarLu).IsModified = false; // Previene modificación PK
            _context.Entry(campMateriaRegular).Property(x => x.CmarClMateria).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampMateriaRegularExists(idAlumno, idCarrera, lu, clMateria))
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
                 return BadRequest($"No se puede modificar la clave primaria. Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampMateriaRegular
        // Para crear un nuevo registro de regularidad de materia-alumno.
        [HttpPost]
        public async Task<ActionResult<CampMateriaRegular>> PostCampMateriaRegular(CampMateriaRegular campMateriaRegular)
        {
             if (_context.CampMateriaRegulars == null)
            {
                 return Problem("La entidad 'CampMateriasRegulares' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(campMateriaRegular.CmarIdAlumno) || string.IsNullOrWhiteSpace(campMateriaRegular.CmarClMateria))
             {
                 return BadRequest("El IdAlumno y ClMateria son requeridos.");
             }

            _context.CampMateriaRegulars.Add(campMateriaRegular);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampMateriaRegularExists(campMateriaRegular.CmarIdAlumno, campMateriaRegular.CmarIdCarrera, campMateriaRegular.CmarLu, campMateriaRegular.CmarClMateria))
                {
                    return Conflict("Ya existe un registro de regularidad para este alumno/materia.");
                }
                else
                {
                     // Podría ser un error de FK si el alumno o materia no existen
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampMateriaRegular), new {
                 idAlumno = campMateriaRegular.CmarIdAlumno,
                 idCarrera = campMateriaRegular.CmarIdCarrera,
                 lu = campMateriaRegular.CmarLu,
                 clMateria = campMateriaRegular.CmarClMateria // Considera URL encoding si es necesario
                 }, campMateriaRegular);
        }

        // DELETE: api/CampMateriaRegular/{idAlumno}/{idCarrera}/{lu}/{clMateria}
        // Elimina un registro de regularidad de materia-alumno.
        [HttpDelete("{idAlumno}/{idCarrera}/{lu}/{clMateria}")]
        public async Task<IActionResult> DeleteCampMateriaRegular(string idAlumno, int idCarrera, int lu, string clMateria)
        {
             if (_context.CampMateriaRegulars == null)
            {
                 return NotFound("La entidad 'CampMateriaRegulars' no está configurada en el DbContext.");
            }

            var campMateriaRegular = await _context.CampMateriaRegulars.FindAsync(idAlumno, idCarrera, lu, clMateria);
            if (campMateriaRegular == null)
            {
                return NotFound();
            }

            _context.CampMateriaRegulars.Remove(campMateriaRegular);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampMateriaRegularExists(string idAlumno, int idCarrera, int lu, string clMateria)
        {
            return (_context.CampMateriaRegulars?.Any(e =>
                e.CmarIdAlumno == idAlumno &&
                e.CmarIdCarrera == idCarrera &&
                e.CmarLu == lu &&
                e.CmarClMateria == clMateria)).GetValueOrDefault();
        }
    }
}