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
    public class CampInfoProfesionalPreviaNovController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampInfoProfesionalPreviaNovController> _logger;

        public CampInfoProfesionalPreviaNovController(ExtranetContext context, IConfiguration configuration, ILogger<CampInfoProfesionalPreviaNovController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampInfoProfesionalPreviaNov
        // Obtiene todos los registros de información profesional previa.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampInfoProfesionalPreviaNov>>> GetCampInfoProfesionalesPreviasNov()
        {
            if (_context.CampInfoProfesionalPreviaNovs == null)
            {
                return NotFound("La entidad 'CampInfoProfesionalesPreviasNov' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todos los registros puede ser ineficiente. Considera paginación y filtrado.
            return await _context.CampInfoProfesionalPreviaNovs.ToListAsync();
        }

        // GET: api/CampInfoProfesionalPreviaNov/{idAlumno}/{idCarrera}/{nroProfesion}
        // Busca un registro específico por la clave primaria compuesta asumida.
        [HttpGet("{idAlumno}/{idCarrera}/{nroProfesion}")]
        public async Task<ActionResult<CampInfoProfesionalPreviaNov>> GetCampInfoProfesionalPreviaNov(string idAlumno, int idCarrera, short nroProfesion)
        {
            if (_context.CampInfoProfesionalPreviaNovs == null)
            {
                 return NotFound("La entidad 'CampInfoProfesionalesPreviasNov' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campInfoProfesionalPreviaNov = await _context.CampInfoProfesionalPreviaNovs.FindAsync(idAlumno, idCarrera, nroProfesion);

            if (campInfoProfesionalPreviaNov == null)
            {
                return NotFound();
            }

            return campInfoProfesionalPreviaNov;
        }

        // PUT: api/CampInfoProfesionalPreviaNov/{idAlumno}/{idCarrera}/{nroProfesion}
        // Para actualizar un registro existente usando la clave compuesta.
        [HttpPut("{idAlumno}/{idCarrera}/{nroProfesion}")]
        public async Task<IActionResult> PutCampInfoProfesionalPreviaNov(string idAlumno, int idCarrera, short nroProfesion, CampInfoProfesionalPreviaNov campInfoProfesionalPreviaNov)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campInfoProfesionalPreviaNov.CipnIdAlumno ||
                idCarrera != campInfoProfesionalPreviaNov.CipnIdCarrera ||
                nroProfesion != campInfoProfesionalPreviaNov.CipnNroProfesion)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampInfoProfesionalPreviaNovs == null)
            {
                 return NotFound("La entidad 'CampInfoProfesionalesPreviasNov' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campInfoProfesionalPreviaNov).State = EntityState.Modified;
            _context.Entry(campInfoProfesionalPreviaNov).Property(x => x.CipnIdAlumno).IsModified = false; // Previene modificación PK
            _context.Entry(campInfoProfesionalPreviaNov).Property(x => x.CipnIdCarrera).IsModified = false; // Previene modificación PK
            _context.Entry(campInfoProfesionalPreviaNov).Property(x => x.CipnNroProfesion).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampInfoProfesionalPreviaNovExists(idAlumno, idCarrera, nroProfesion))
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

        // POST: api/CampInfoProfesionalPreviaNov
        // Para crear un nuevo registro de información profesional previa.
        [HttpPost]
        public async Task<ActionResult<CampInfoProfesionalPreviaNov>> PostCampInfoProfesionalPreviaNov(CampInfoProfesionalPreviaNov campInfoProfesionalPreviaNov)
        {
             if (_context.CampInfoProfesionalPreviaNovs == null)
            {
                 return Problem("La entidad 'CampInfoProfesionalesPreviasNov' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(campInfoProfesionalPreviaNov.CipnIdAlumno))
             {
                 return BadRequest("El IdAlumno es requerido.");
             }
             // Podrían necesitarse validaciones para CipnNroProfesion si no debe repetirse para el mismo alumno/carrera

            _context.CampInfoProfesionalPreviaNovs.Add(campInfoProfesionalPreviaNov);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampInfoProfesionalPreviaNovExists(campInfoProfesionalPreviaNov.CipnIdAlumno, campInfoProfesionalPreviaNov.CipnIdCarrera, campInfoProfesionalPreviaNov.CipnNroProfesion))
                {
                    return Conflict("Ya existe un registro de información profesional previa con esta clave.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampInfoProfesionalPreviaNov), new {
                 idAlumno = campInfoProfesionalPreviaNov.CipnIdAlumno,
                 idCarrera = campInfoProfesionalPreviaNov.CipnIdCarrera,
                 nroProfesion = campInfoProfesionalPreviaNov.CipnNroProfesion
                 }, campInfoProfesionalPreviaNov);
        }

        // DELETE: api/CampInfoProfesionalPreviaNov/{idAlumno}/{idCarrera}/{nroProfesion}
        // Elimina un registro de información profesional previa.
        [HttpDelete("{idAlumno}/{idCarrera}/{nroProfesion}")]
        public async Task<IActionResult> DeleteCampInfoProfesionalPreviaNov(string idAlumno, int idCarrera, short nroProfesion)
        {
             if (_context.CampInfoProfesionalPreviaNovs == null)
            {
                 return NotFound("La entidad 'CampInfoProfesionalesPreviasNov' no está configurada en el DbContext.");
            }

            var campInfoProfesionalPreviaNov = await _context.CampInfoProfesionalPreviaNovs.FindAsync(idAlumno, idCarrera, nroProfesion);
            if (campInfoProfesionalPreviaNov == null)
            {
                return NotFound();
            }

            _context.CampInfoProfesionalPreviaNovs.Remove(campInfoProfesionalPreviaNov);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampInfoProfesionalPreviaNovExists(string idAlumno, int idCarrera, short nroProfesion)
        {
            return (_context.CampInfoProfesionalPreviaNovs?.Any(e =>
                e.CipnIdAlumno == idAlumno &&
                e.CipnIdCarrera == idCarrera &&
                e.CipnNroProfesion == nroProfesion)).GetValueOrDefault();
        }
    }
}