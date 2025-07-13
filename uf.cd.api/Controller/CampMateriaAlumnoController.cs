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
    public class CampMateriaAlumnoController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampMateriaAlumnoController> _logger;

        public CampMateriaAlumnoController(ExtranetContext context, IConfiguration configuration, ILogger<CampMateriaAlumnoController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampMateriaAlumno
        // Obtiene todos los registros de materia por alumno.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampMateriaAlumno>>> GetCampMateriasAlumnos()
        {
            if (_context.CampMateriaAlumnos == null)
            {
                return NotFound("La entidad 'CampMateriasAlumnos' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todos los registros puede ser ineficiente.
             // Considera paginación y filtrado (ej: por alumno).
            return await _context.CampMateriaAlumnos.ToListAsync();
        }

        // GET: api/CampMateriaAlumno/{idAlumno}/{idCarrera}/{lu}/{idMateria}
        // Busca un registro específico por la clave primaria compuesta asumida.
        [HttpGet("{idAlumno}/{idCarrera}/{lu}/{idMateria}")]
        public async Task<ActionResult<CampMateriaAlumno>> GetCampMateriaAlumno(string idAlumno, int idCarrera, int lu, int idMateria)
        {
            if (_context.CampMateriaAlumnos == null)
            {
                 return NotFound("La entidad 'CampMateriasAlumnos' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campMateriaAlumno = await _context.CampMateriaAlumnos.FindAsync(idAlumno, idCarrera, lu, idMateria);

            if (campMateriaAlumno == null)
            {
                return NotFound();
            }

            return campMateriaAlumno;
        }

        // PUT: api/CampMateriaAlumno/{idAlumno}/{idCarrera}/{lu}/{idMateria}
        // Para actualizar un registro existente usando la clave compuesta (ej: cargar nota, resultado).
        [HttpPut("{idAlumno}/{idCarrera}/{lu}/{idMateria}")]
        public async Task<IActionResult> PutCampMateriaAlumno(string idAlumno, int idCarrera, int lu, int idMateria, CampMateriaAlumno campMateriaAlumno)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campMateriaAlumno.CmalIdAlumno ||
                idCarrera != campMateriaAlumno.CmalIdCarrera ||
                lu != campMateriaAlumno.CmalLu ||
                idMateria != campMateriaAlumno.CmalIdMateria)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampMateriaAlumnos == null)
            {
                 return NotFound("La entidad 'CampMateriasAlumnos' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campMateriaAlumno).State = EntityState.Modified;
            _context.Entry(campMateriaAlumno).Property(x => x.CmalIdAlumno).IsModified = false; // Previene modificación PK
            _context.Entry(campMateriaAlumno).Property(x => x.CmalIdCarrera).IsModified = false; // Previene modificación PK
            _context.Entry(campMateriaAlumno).Property(x => x.CmalLu).IsModified = false; // Previene modificación PK
            _context.Entry(campMateriaAlumno).Property(x => x.CmalIdMateria).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampMateriaAlumnoExists(idAlumno, idCarrera, lu, idMateria))
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

        // POST: api/CampMateriaAlumno
        // Para crear un nuevo registro de materia-alumno.
        [HttpPost]
        public async Task<ActionResult<CampMateriaAlumno>> PostCampMateriaAlumno(CampMateriaAlumno campMateriaAlumno)
        {
             if (_context.CampMateriaAlumnos == null)
            {
                 return Problem("La entidad 'CampMateriasAlumnos' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(campMateriaAlumno.CmalIdAlumno))
             {
                 return BadRequest("El IdAlumno es requerido.");
             }

            _context.CampMateriaAlumnos.Add(campMateriaAlumno);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampMateriaAlumnoExists(campMateriaAlumno.CmalIdAlumno, campMateriaAlumno.CmalIdCarrera, campMateriaAlumno.CmalLu, campMateriaAlumno.CmalIdMateria))
                {
                    return Conflict("Ya existe un registro para este alumno/materia.");
                }
                else
                {
                    // Podría ser un error de FK si el alumno o materia no existen
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampMateriaAlumno), new {
                 idAlumno = campMateriaAlumno.CmalIdAlumno,
                 idCarrera = campMateriaAlumno.CmalIdCarrera,
                 lu = campMateriaAlumno.CmalLu,
                 idMateria = campMateriaAlumno.CmalIdMateria
                 }, campMateriaAlumno);
        }

        // DELETE: api/CampMateriaAlumno/{idAlumno}/{idCarrera}/{lu}/{idMateria}
        // Elimina un registro de materia-alumno.
        [HttpDelete("{idAlumno}/{idCarrera}/{lu}/{idMateria}")]
        public async Task<IActionResult> DeleteCampMateriaAlumno(string idAlumno, int idCarrera, int lu, int idMateria)
        {
             if (_context.CampMateriaAlumnos == null)
            {
                 return NotFound("La entidad 'CampMateriasAlumnos' no está configurada en el DbContext.");
            }

            var campMateriaAlumno = await _context.CampMateriaAlumnos.FindAsync(idAlumno, idCarrera, lu, idMateria);
            if (campMateriaAlumno == null)
            {
                return NotFound();
            }

            _context.CampMateriaAlumnos.Remove(campMateriaAlumno);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampMateriaAlumnoExists(string idAlumno, int idCarrera, int lu, int idMateria)
        {
            return (_context.CampMateriaAlumnos?.Any(e =>
                e.CmalIdAlumno == idAlumno &&
                e.CmalIdCarrera == idCarrera &&
                e.CmalLu == lu &&
                e.CmalIdMateria == idMateria)).GetValueOrDefault();
        }
    }
}