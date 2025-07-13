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
    public class CampExamenNovController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampExamenNovController> _logger;

        public CampExamenNovController(ExtranetContext context, IConfiguration configuration, ILogger<CampExamenNovController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampExamenNov
        // Obtiene todos los registros de novedades de examen (inscripciones).
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampExamenNov>>> GetCampExamenNovs()
        {
            if (_context.CampExamenNovs == null)
            {
                return NotFound("La entidad 'CampExamenNovs' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los registros puede ser ineficiente.
            // Considera paginación y filtrado (ej: por alumno, por fecha).
            return await _context.CampExamenNovs.ToListAsync();
        }

        // GET: api/CampExamenNov/{idAlumno}/{idExamen}/{parcialFinal}/{fecha}
        // Busca un registro específico por la clave primaria compuesta asumida.
        // Nota: Pasar DateTime en la ruta puede requerir un formato específico (ej: ISO 8601 YYYY-MM-DD).
        // Considera también codificar 'parcialFinal' si puede contener caracteres especiales para URL.
        [HttpGet("{idAlumno}/{idExamen}/{parcialFinal}/{fecha}")]
        public async Task<ActionResult<CampExamenNov>> GetCampExamenNov(string idAlumno, int idExamen, string parcialFinal, DateTime fecha)
        {
            if (_context.CampExamenNovs == null)
            {
                 return NotFound("La entidad 'CampExamenNovs' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campExamenNov = await _context.CampExamenNovs.FindAsync(idAlumno, idExamen, parcialFinal, fecha);

            if (campExamenNov == null)
            {
                return NotFound();
            }

            return campExamenNov;
        }

        // PUT: api/CampExamenNov/{idAlumno}/{idExamen}/{parcialFinal}/{fecha}
        // Para actualizar un registro existente usando la clave compuesta (ej: confirmar asistencia CexnSiNo).
        [HttpPut("{idAlumno}/{idExamen}/{parcialFinal}/{fecha}")]
        public async Task<IActionResult> PutCampExamenNov(string idAlumno, int idExamen, string parcialFinal, DateTime fecha, CampExamenNov campExamenNov)
        {
            DateOnly fechaAComparar = DateOnly.FromDateTime(fecha);

            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campExamenNov.CexnIdAlumno ||
                idExamen != campExamenNov.CexnIdExamen ||
                parcialFinal != campExamenNov.CexnParcialFinal ||
                fechaAComparar != campExamenNov.CexnFecha)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampExamenNovs == null)
            {
                 return NotFound("La entidad 'CampExamenNovs' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campExamenNov).State = EntityState.Modified;
            _context.Entry(campExamenNov).Property(x => x.CexnIdAlumno).IsModified = false; // Previene modificación PK
            _context.Entry(campExamenNov).Property(x => x.CexnIdExamen).IsModified = false; // Previene modificación PK
            _context.Entry(campExamenNov).Property(x => x.CexnParcialFinal).IsModified = false; // Previene modificación PK
            _context.Entry(campExamenNov).Property(x => x.CexnFecha).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampExamenNovExists(idAlumno, idExamen, parcialFinal, fechaAComparar))
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

        // POST: api/CampExamenNov
        // Para crear un nuevo registro de novedad de examen (inscripción).
        [HttpPost]
        public async Task<ActionResult<CampExamenNov>> PostCampExamenNov(CampExamenNov campExamenNov)
        {
             if (_context.CampExamenNovs == null)
            {
                 return Problem("La entidad 'CampExamenNovs' no está configurada en el DbContext.");
            }

            // Opcional: Validaciones adicionales
            if (string.IsNullOrEmpty(campExamenNov.CexnIdAlumno) ||
                string.IsNullOrEmpty(campExamenNov.CexnParcialFinal))
            {
                 return BadRequest("IdAlumno y ParcialFinal son requeridos.");
            }

            _context.CampExamenNovs.Add(campExamenNov);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampExamenNovExists(campExamenNov.CexnIdAlumno, campExamenNov.CexnIdExamen, campExamenNov.CexnParcialFinal, campExamenNov.CexnFecha))
                {
                    return Conflict("Ya existe una inscripción para este alumno en este examen/fecha.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampExamenNov), new {
                 idAlumno = campExamenNov.CexnIdAlumno,
                 idExamen = campExamenNov.CexnIdExamen,
                 parcialFinal = campExamenNov.CexnParcialFinal, // Considera URL encoding
                 fecha = campExamenNov.CexnFecha // Asegúrate que el formato sea compatible con la ruta
                 }, campExamenNov);
        }

        // DELETE: api/CampExamenNov/{idAlumno}/{idExamen}/{parcialFinal}/{fecha}
        // Elimina una inscripción a examen (novedad).
        [HttpDelete("{idAlumno}/{idExamen}/{parcialFinal}/{fecha}")]
        public async Task<IActionResult> DeleteCampExamenNov(string idAlumno, int idExamen, string parcialFinal, DateTime fecha)
        {
             if (_context.CampExamenNovs == null)
            {
                 return NotFound("La entidad 'CampExamenNovs' no está configurada en el DbContext.");
            }

            var campExamenNov = await _context.CampExamenNovs.FindAsync(idAlumno, idExamen, parcialFinal, fecha);
            if (campExamenNov == null)
            {
                return NotFound();
            }

            _context.CampExamenNovs.Remove(campExamenNov);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampExamenNovExists(string idAlumno, int idExamen, string parcialFinal, DateOnly fecha)
        {
            //DateOnly fechaAComparar = DateOnly.FromDateTime(fecha);

            return (_context.CampExamenNovs?.Any(e =>
                e.CexnIdAlumno == idAlumno &&
                e.CexnIdExamen == idExamen &&
                e.CexnParcialFinal == parcialFinal &&
                e.CexnFecha == fecha)).GetValueOrDefault();
        }
    }
}