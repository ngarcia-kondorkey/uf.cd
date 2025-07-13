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
    public class CampInfoProfesionalPreviumController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampInfoProfesionalPreviumController> _logger;

        public CampInfoProfesionalPreviumController(ExtranetContext context, IConfiguration configuration, ILogger<CampInfoProfesionalPreviumController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampInfoProfesionalPrevium
        // Obtiene todos los registros de información profesional previa.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampInfoProfesionalPrevium>>> GetCampInfoProfesionalesPrevia()
        {
            if (_context.CampInfoProfesionalPrevia == null)
            {
                return NotFound("La entidad 'CampInfoProfesionalesPrevia' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todos los registros puede ser ineficiente. Considera paginación y filtrado.
            return await _context.CampInfoProfesionalPrevia.ToListAsync();
        }

        // GET: api/CampInfoProfesionalPrevium/{idAlumno}/{idCarrera}/{nroProfesion}
        // Busca un registro específico por la clave primaria compuesta asumida.
        [HttpGet("{idAlumno}/{idCarrera}/{nroProfesion}")]
        public async Task<ActionResult<CampInfoProfesionalPrevium>> GetCampInfoProfesionalPrevium(string idAlumno, int idCarrera, short nroProfesion)
        {
            if (_context.CampInfoProfesionalPrevia == null)
            {
                 return NotFound("La entidad 'CampInfoProfesionalesPrevia' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campInfoProfesionalPrevium = await _context.CampInfoProfesionalPrevia.FindAsync(idAlumno, idCarrera, nroProfesion);

            if (campInfoProfesionalPrevium == null)
            {
                return NotFound();
            }

            return campInfoProfesionalPrevium;
        }

        // PUT: api/CampInfoProfesionalPrevium/{idAlumno}/{idCarrera}/{nroProfesion}
        // Para actualizar un registro existente usando la clave compuesta.
        [HttpPut("{idAlumno}/{idCarrera}/{nroProfesion}")]
        public async Task<IActionResult> PutCampInfoProfesionalPrevium(string idAlumno, int idCarrera, short nroProfesion, CampInfoProfesionalPrevium campInfoProfesionalPrevium)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campInfoProfesionalPrevium.CippIdAlumno ||
                idCarrera != campInfoProfesionalPrevium.CippIdCarrera ||
                nroProfesion != campInfoProfesionalPrevium.CippNroProfesion)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampInfoProfesionalPrevia == null)
            {
                 return NotFound("La entidad 'CampInfoProfesionalesPrevia' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campInfoProfesionalPrevium).State = EntityState.Modified;
            _context.Entry(campInfoProfesionalPrevium).Property(x => x.CippIdAlumno).IsModified = false; // Previene modificación PK
            _context.Entry(campInfoProfesionalPrevium).Property(x => x.CippIdCarrera).IsModified = false; // Previene modificación PK
            _context.Entry(campInfoProfesionalPrevium).Property(x => x.CippNroProfesion).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampInfoProfesionalPreviumExists(idAlumno, idCarrera, nroProfesion))
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

        // POST: api/CampInfoProfesionalPrevium
        // Para crear un nuevo registro de información profesional previa.
        [HttpPost]
        public async Task<ActionResult<CampInfoProfesionalPrevium>> PostCampInfoProfesionalPrevium(CampInfoProfesionalPrevium campInfoProfesionalPrevium)
        {
             if (_context.CampInfoProfesionalPrevia == null)
            {
                 return Problem("La entidad 'CampInfoProfesionalesPrevia' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(campInfoProfesionalPrevium.CippIdAlumno))
             {
                 return BadRequest("El IdAlumno es requerido.");
             }
             // Podrían necesitarse validaciones para CippNroProfesion si no debe repetirse para el mismo alumno/carrera

            _context.CampInfoProfesionalPrevia.Add(campInfoProfesionalPrevium);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampInfoProfesionalPreviumExists(campInfoProfesionalPrevium.CippIdAlumno, campInfoProfesionalPrevium.CippIdCarrera, campInfoProfesionalPrevium.CippNroProfesion))
                {
                    return Conflict("Ya existe un registro de información profesional previa con esta clave.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampInfoProfesionalPrevium), new {
                 idAlumno = campInfoProfesionalPrevium.CippIdAlumno,
                 idCarrera = campInfoProfesionalPrevium.CippIdCarrera,
                 nroProfesion = campInfoProfesionalPrevium.CippNroProfesion
                 }, campInfoProfesionalPrevium);
        }

        // DELETE: api/CampInfoProfesionalPrevium/{idAlumno}/{idCarrera}/{nroProfesion}
        // Elimina un registro de información profesional previa.
        [HttpDelete("{idAlumno}/{idCarrera}/{nroProfesion}")]
        public async Task<IActionResult> DeleteCampInfoProfesionalPrevium(string idAlumno, int idCarrera, short nroProfesion)
        {
             if (_context.CampInfoProfesionalPrevia == null)
            {
                 return NotFound("La entidad 'CampInfoProfesionalesPrevia' no está configurada en el DbContext.");
            }

            var campInfoProfesionalPrevium = await _context.CampInfoProfesionalPrevia.FindAsync(idAlumno, idCarrera, nroProfesion);
            if (campInfoProfesionalPrevium == null)
            {
                return NotFound();
            }

            _context.CampInfoProfesionalPrevia.Remove(campInfoProfesionalPrevium);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampInfoProfesionalPreviumExists(string idAlumno, int idCarrera, short nroProfesion)
        {
            return (_context.CampInfoProfesionalPrevia?.Any(e =>
                e.CippIdAlumno == idAlumno &&
                e.CippIdCarrera == idCarrera &&
                e.CippNroProfesion == nroProfesion)).GetValueOrDefault();
        }
    }
}