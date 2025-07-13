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
    public class CampEncuestasPadresNovController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampEncuestasPadresNovController> _logger;

        public CampEncuestasPadresNovController(ExtranetContext context, IConfiguration configuration, ILogger<CampEncuestasPadresNovController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampEncuestasPadresNov
        // Obtiene todos los registros de encuestas de padres.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampEncuestasPadresNov>>> GetCampEncuestasPadresNovs()
        {
            if (_context.CampEncuestasPadresNovs == null)
            {
                return NotFound("La entidad 'CampEncuestasPadresNovs' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todos los registros puede ser ineficiente y exponer datos personales.
             // Considera paginación y filtrado (ej: por alumno).
            return await _context.CampEncuestasPadresNovs.ToListAsync();
        }

        // GET: api/CampEncuestasPadresNov/{idAlumno}/{padreMadre}
        // Busca un registro específico por la clave primaria compuesta asumida (IdAlumno, PadreMadre)
        // Nota: Considera codificar 'padreMadre' si puede contener caracteres especiales para URL.
        [HttpGet("{idAlumno}/{padreMadre}")]
        public async Task<ActionResult<CampEncuestasPadresNov>> GetCampEncuestasPadresNov(string idAlumno, string padreMadre)
        {
            if (_context.CampEncuestasPadresNovs == null)
            {
                 return NotFound("La entidad 'CampEncuestasPadresNovs' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campEncuestasPadresNov = await _context.CampEncuestasPadresNovs.FindAsync(idAlumno, padreMadre);

            if (campEncuestasPadresNov == null)
            {
                return NotFound();
            }

            return campEncuestasPadresNov;
        }

        // PUT: api/CampEncuestasPadresNov/{idAlumno}/{padreMadre}
        // Para actualizar un registro de padre/madre existente.
        [HttpPut("{idAlumno}/{padreMadre}")]
        public async Task<IActionResult> PutCampEncuestasPadresNov(string idAlumno, string padreMadre, CampEncuestasPadresNov campEncuestasPadresNov)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campEncuestasPadresNov.CepaIdAlumno || padreMadre != campEncuestasPadresNov.CepaPadreMadre)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampEncuestasPadresNovs == null)
            {
                 return NotFound("La entidad 'CampEncuestasPadresNovs' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campEncuestasPadresNov).State = EntityState.Modified;
            _context.Entry(campEncuestasPadresNov).Property(x => x.CepaIdAlumno).IsModified = false; // Previene modificación PK
            _context.Entry(campEncuestasPadresNov).Property(x => x.CepaPadreMadre).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampEncuestasPadresNovExists(idAlumno, padreMadre))
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

        // POST: api/CampEncuestasPadresNov
        // Para crear un nuevo registro de encuesta de padre/madre.
        [HttpPost]
        public async Task<ActionResult<CampEncuestasPadresNov>> PostCampEncuestasPadresNov(CampEncuestasPadresNov campEncuestasPadresNov)
        {
             if (_context.CampEncuestasPadresNovs == null)
            {
                 return Problem("La entidad 'CampEncuestasPadresNovs' no está configurada en el DbContext.");
            }

            // Opcional: Validaciones adicionales
            if (string.IsNullOrEmpty(campEncuestasPadresNov.CepaIdAlumno) || string.IsNullOrEmpty(campEncuestasPadresNov.CepaPadreMadre))
            {
                return BadRequest("CepaIdAlumno y CepaPadreMadre son requeridos.");
            }

            _context.CampEncuestasPadresNovs.Add(campEncuestasPadresNov);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampEncuestasPadresNovExists(campEncuestasPadresNov.CepaIdAlumno, campEncuestasPadresNov.CepaPadreMadre))
                {
                    return Conflict($"Ya existe un registro para este padre/madre del alumno {campEncuestasPadresNov.CepaIdAlumno}.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampEncuestasPadresNov), new {
                 idAlumno = campEncuestasPadresNov.CepaIdAlumno,
                 padreMadre = campEncuestasPadresNov.CepaPadreMadre // Considera URL encoding si es necesario
                 }, campEncuestasPadresNov);
        }

        // DELETE: api/CampEncuestasPadresNov/{idAlumno}/{padreMadre}
        // Elimina un registro de encuesta de padre/madre.
        [HttpDelete("{idAlumno}/{padreMadre}")]
        public async Task<IActionResult> DeleteCampEncuestasPadresNov(string idAlumno, string padreMadre)
        {
             if (_context.CampEncuestasPadresNovs == null)
            {
                 return NotFound("La entidad 'CampEncuestasPadresNovs' no está configurada en el DbContext.");
            }

            var campEncuestasPadresNov = await _context.CampEncuestasPadresNovs.FindAsync(idAlumno, padreMadre);
            if (campEncuestasPadresNov == null)
            {
                return NotFound();
            }

            _context.CampEncuestasPadresNovs.Remove(campEncuestasPadresNov);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampEncuestasPadresNovExists(string idAlumno, string padreMadre)
        {
            return (_context.CampEncuestasPadresNovs?.Any(e => e.CepaIdAlumno == idAlumno && e.CepaPadreMadre == padreMadre)).GetValueOrDefault();
        }
    }
}