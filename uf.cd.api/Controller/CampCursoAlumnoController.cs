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
    public class CampCursoAlumnoController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampCursoAlumnoController> _logger;

        public CampCursoAlumnoController(ExtranetContext context, IConfiguration configuration, ILogger<CampCursoAlumnoController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampCursoAlumno
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampCursoAlumno>>> GetCampCursoAlumnos() // Asumiendo DbSet se llama CampCursoAlumnos
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampCursoAlumnos == null)
            {
                return NotFound("La entidad 'CampCursoAlumnos' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los registros puede ser ineficiente. Considera paginación/filtrado.
            return await _context.CampCursoAlumnos.ToListAsync();
        }

        // GET: api/CampCursoAlumno/{idAlumno}/{idCarrera}/{lu}/{clMateria}/{pl}
        // Busca por la clave primaria compuesta asumida.
        // Nota: Considera codificar 'clMateria' si puede contener caracteres especiales para URL.
        [HttpGet("{idAlumno}/{idCarrera}/{lu}/{clMateria}/{pl}")]
        public async Task<ActionResult<CampCursoAlumno>> GetCampCursoAlumno(string idAlumno, int idCarrera, int lu, string clMateria, int pl)
        {
            if (_context.CampCursoAlumnos == null)
            {
                 return NotFound("La entidad 'CampCursoAlumnos' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campCursoAlumno = await _context.CampCursoAlumnos.FindAsync(idAlumno, idCarrera, lu, clMateria, pl);

            if (campCursoAlumno == null)
            {
                return NotFound();
            }

            return campCursoAlumno;
        }

        // PUT: api/CampCursoAlumno/{idAlumno}/{idCarrera}/{lu}/{clMateria}/{pl}
        // Para actualizar un registro existente usando la clave compuesta
        [HttpPut("{idAlumno}/{idCarrera}/{lu}/{clMateria}/{pl}")]
        public async Task<IActionResult> PutCampCursoAlumno(string idAlumno, int idCarrera, int lu, string clMateria, int pl, CampCursoAlumno campCursoAlumno)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campCursoAlumno.CualIdAlumno ||
                idCarrera != campCursoAlumno.CualIdCarrera ||
                lu != campCursoAlumno.CualLu ||
                clMateria != campCursoAlumno.CualClMateria ||
                pl != campCursoAlumno.CualPl)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampCursoAlumnos == null)
            {
                 return NotFound("La entidad 'CampCursoAlumnos' no está configurada en el DbContext.");
            }

             // Solo permite modificar campos que no son parte de la clave primaria
             _context.Entry(campCursoAlumno).State = EntityState.Modified;
             // Previene la modificación de la clave primaria si se intenta
             _context.Entry(campCursoAlumno).Property(x => x.CualIdAlumno).IsModified = false;
             _context.Entry(campCursoAlumno).Property(x => x.CualIdCarrera).IsModified = false;
             _context.Entry(campCursoAlumno).Property(x => x.CualLu).IsModified = false;
             _context.Entry(campCursoAlumno).Property(x => x.CualClMateria).IsModified = false;
             _context.Entry(campCursoAlumno).Property(x => x.CualPl).IsModified = false;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampCursoAlumnoExists(idAlumno, idCarrera, lu, clMateria, pl))
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
                 // Captura el error si se intentó modificar la PK (ya prevenido arriba, pero como defensa)
                 return BadRequest($"No se puede modificar la clave primaria. Error: {ex.Message}");
            }


            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampCursoAlumno
        // Para crear un nuevo registro de curso-alumno
        [HttpPost]
        public async Task<ActionResult<CampCursoAlumno>> PostCampCursoAlumno(CampCursoAlumno campCursoAlumno)
        {
             if (_context.CampCursoAlumnos == null)
            {
                 return Problem("La entidad 'CampCursoAlumnos' no está configurada en el DbContext.");
            }

            _context.CampCursoAlumnos.Add(campCursoAlumno);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampCursoAlumnoExists(campCursoAlumno.CualIdAlumno, campCursoAlumno.CualIdCarrera, campCursoAlumno.CualLu, campCursoAlumno.CualClMateria, campCursoAlumno.CualPl))
                {
                    return Conflict("Ya existe un registro para este alumno en esta materia/plan.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampCursoAlumno), new {
                idAlumno = campCursoAlumno.CualIdAlumno,
                idCarrera = campCursoAlumno.CualIdCarrera,
                lu = campCursoAlumno.CualLu,
                clMateria = campCursoAlumno.CualClMateria, // Considera URL encoding si es necesario
                pl = campCursoAlumno.CualPl
                }, campCursoAlumno);
        }

        // DELETE: api/CampCursoAlumno/{idAlumno}/{idCarrera}/{lu}/{clMateria}/{pl}
        // Elimina por clave primaria compuesta asumida
        [HttpDelete("{idAlumno}/{idCarrera}/{lu}/{clMateria}/{pl}")]
        public async Task<IActionResult> DeleteCampCursoAlumno(string idAlumno, int idCarrera, int lu, string clMateria, int pl)
        {
             if (_context.CampCursoAlumnos == null)
            {
                 return NotFound("La entidad 'CampCursoAlumnos' no está configurada en el DbContext.");
            }

            var campCursoAlumno = await _context.CampCursoAlumnos.FindAsync(idAlumno, idCarrera, lu, clMateria, pl);
            if (campCursoAlumno == null)
            {
                return NotFound();
            }

            _context.CampCursoAlumnos.Remove(campCursoAlumno);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampCursoAlumnoExists(string idAlumno, int idCarrera, int lu, string clMateria, int pl)
        {
            // Asegúrate que CampCursoAlumnos no sea null antes de usarlo
            return (_context.CampCursoAlumnos?.Any(e =>
                e.CualIdAlumno == idAlumno &&
                e.CualIdCarrera == idCarrera &&
                e.CualLu == lu &&
                e.CualClMateria == clMateria &&
                e.CualPl == pl)).GetValueOrDefault();
        }
    }
}