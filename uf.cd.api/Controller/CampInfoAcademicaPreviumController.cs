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
    public class CampInfoAcademicaPreviumController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampInfoAcademicaPreviumController> _logger;

        public CampInfoAcademicaPreviumController(ExtranetContext context, IConfiguration configuration, ILogger<CampInfoAcademicaPreviumController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampInfoAcademicaPrevium
        // Obtiene todos los registros de información académica previa.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampInfoAcademicaPrevium>>> GetCampInfoAcademicaPrevia()
        {
            if (_context.CampInfoAcademicaPrevia == null)
            {
                return NotFound("La entidad 'CampInfoAcademicaPrevia' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todos los registros puede ser ineficiente. Considera paginación y filtrado.
            return await _context.CampInfoAcademicaPrevia.ToListAsync();
        }

        // GET: api/CampInfoAcademicaPrevium/{idAlumno}/{idCarrera}/{nroTitulo}
        // Busca un registro específico por la clave primaria compuesta asumida.
        [HttpGet("{idAlumno}/{idCarrera}/{nroTitulo}")]
        public async Task<ActionResult<CampInfoAcademicaPrevium>> GetCampInfoAcademicaPrevium(string idAlumno, int idCarrera, short nroTitulo)
        {
            if (_context.CampInfoAcademicaPrevia == null)
            {
                 return NotFound("La entidad 'CampInfoAcademicaPrevia' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campInfoAcademicaPrevium = await _context.CampInfoAcademicaPrevia.FindAsync(idAlumno, idCarrera, nroTitulo);

            if (campInfoAcademicaPrevium == null)
            {
                return NotFound();
            }

            return campInfoAcademicaPrevium;
        }

        // PUT: api/CampInfoAcademicaPrevium/{idAlumno}/{idCarrera}/{nroTitulo}
        // Para actualizar un registro existente usando la clave compuesta.
        [HttpPut("{idAlumno}/{idCarrera}/{nroTitulo}")]
        public async Task<IActionResult> PutCampInfoAcademicaPrevium(string idAlumno, int idCarrera, short nroTitulo, CampInfoAcademicaPrevium campInfoAcademicaPrevium)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campInfoAcademicaPrevium.CiapIdAlumno ||
                idCarrera != campInfoAcademicaPrevium.CiapIdCarrera ||
                nroTitulo != campInfoAcademicaPrevium.CiapNroTitulo)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampInfoAcademicaPrevia == null)
            {
                 return NotFound("La entidad 'CampInfoAcademicaPrevia' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campInfoAcademicaPrevium).State = EntityState.Modified;
            _context.Entry(campInfoAcademicaPrevium).Property(x => x.CiapIdAlumno).IsModified = false; // Previene modificación PK
            _context.Entry(campInfoAcademicaPrevium).Property(x => x.CiapIdCarrera).IsModified = false; // Previene modificación PK
            _context.Entry(campInfoAcademicaPrevium).Property(x => x.CiapNroTitulo).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampInfoAcademicaPreviumExists(idAlumno, idCarrera, nroTitulo))
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

        // POST: api/CampInfoAcademicaPrevium
        // Para crear un nuevo registro de información académica previa.
        [HttpPost]
        public async Task<ActionResult<CampInfoAcademicaPrevium>> PostCampInfoAcademicaPrevium(CampInfoAcademicaPrevium campInfoAcademicaPrevium)
        {
             if (_context.CampInfoAcademicaPrevia == null)
            {
                 return Problem("La entidad 'CampInfoAcademicaPrevia' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(campInfoAcademicaPrevium.CiapIdAlumno))
             {
                 return BadRequest("El IdAlumno es requerido.");
             }
             // Podrían necesitarse validaciones para CiapNroTitulo si no debe repetirse para el mismo alumno/carrera

            _context.CampInfoAcademicaPrevia.Add(campInfoAcademicaPrevium);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampInfoAcademicaPreviumExists(campInfoAcademicaPrevium.CiapIdAlumno, campInfoAcademicaPrevium.CiapIdCarrera, campInfoAcademicaPrevium.CiapNroTitulo))
                {
                    return Conflict("Ya existe un registro de información académica previa con esta clave.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampInfoAcademicaPrevium), new {
                 idAlumno = campInfoAcademicaPrevium.CiapIdAlumno,
                 idCarrera = campInfoAcademicaPrevium.CiapIdCarrera,
                 nroTitulo = campInfoAcademicaPrevium.CiapNroTitulo
                 }, campInfoAcademicaPrevium);
        }

        // DELETE: api/CampInfoAcademicaPrevium/{idAlumno}/{idCarrera}/{nroTitulo}
        // Elimina un registro de información académica previa.
        [HttpDelete("{idAlumno}/{idCarrera}/{nroTitulo}")]
        public async Task<IActionResult> DeleteCampInfoAcademicaPrevium(string idAlumno, int idCarrera, short nroTitulo)
        {
             if (_context.CampInfoAcademicaPrevia == null)
            {
                 return NotFound("La entidad 'CampInfoAcademicaPrevia' no está configurada en el DbContext.");
            }

            var campInfoAcademicaPrevium = await _context.CampInfoAcademicaPrevia.FindAsync(idAlumno, idCarrera, nroTitulo);
            if (campInfoAcademicaPrevium == null)
            {
                return NotFound();
            }

            _context.CampInfoAcademicaPrevia.Remove(campInfoAcademicaPrevium);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampInfoAcademicaPreviumExists(string idAlumno, int idCarrera, short nroTitulo)
        {
            return (_context.CampInfoAcademicaPrevia?.Any(e =>
                e.CiapIdAlumno == idAlumno &&
                e.CiapIdCarrera == idCarrera &&
                e.CiapNroTitulo == nroTitulo)).GetValueOrDefault();
        }
    }
}