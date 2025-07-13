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
    public class CampInscripMateriaNovController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampInscripMateriaNovController> _logger;

        public CampInscripMateriaNovController(ExtranetContext context, IConfiguration configuration, ILogger<CampInscripMateriaNovController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampInscripMateriaNov
        // Obtiene todas las novedades de inscripción a materias.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampInscripMateriaNov>>> GetCampInscripMateriasNov()
        {
            if (_context.CampInscripMateriaNovs == null)
            {
                return NotFound("La entidad 'CampInscripMateriasNov' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todos los registros puede ser ineficiente.
             // Considera paginación y filtrado (ej: por alumno, por fecha).
            return await _context.CampInscripMateriaNovs.ToListAsync();
        }

        // GET: api/CampInscripMateriaNov/{idAlumno}/{idMateria}/{fecha}
        // Busca un registro específico por la clave primaria compuesta asumida.
        // Nota: Pasar DateTime en la ruta puede requerir un formato específico (ej: ISO 8601 YYYY-MM-DD).
        [HttpGet("{idAlumno}/{idMateria}/{fecha}")]
        public async Task<ActionResult<CampInscripMateriaNov>> GetCampInscripMateriaNov(string idAlumno, int idMateria, DateTime fecha)
        {
            if (_context.CampInscripMateriaNovs == null)
            {
                 return NotFound("La entidad 'CampInscripMateriasNov' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campInscripMateriaNov = await _context.CampInscripMateriaNovs.FindAsync(idAlumno, idMateria, fecha);

            if (campInscripMateriaNov == null)
            {
                return NotFound();
            }

            return campInscripMateriaNov;
        }

        // PUT: api/CampInscripMateriaNov/{idAlumno}/{idMateria}/{fecha}
        // Para actualizar un registro existente usando la clave compuesta (ej: confirmar o cambiar estado).
        [HttpPut("{idAlumno}/{idMateria}/{fecha}")]
        public async Task<IActionResult> PutCampInscripMateriaNov(string idAlumno, int idMateria, DateTime fecha, CampInscripMateriaNov campInscripMateriaNov)
        {
            DateOnly fechaAComparar = DateOnly.FromDateTime(fecha);

            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idAlumno != campInscripMateriaNov.CimnIdAlumno ||
                idMateria != campInscripMateriaNov.CimnIdMateria ||
                fechaAComparar != campInscripMateriaNov.CimnFecha)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampInscripMateriaNovs == null)
            {
                 return NotFound("La entidad 'CampInscripMateriasNov' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campInscripMateriaNov).State = EntityState.Modified;
            _context.Entry(campInscripMateriaNov).Property(x => x.CimnIdAlumno).IsModified = false; // Previene modificación PK
            _context.Entry(campInscripMateriaNov).Property(x => x.CimnIdMateria).IsModified = false; // Previene modificación PK
            _context.Entry(campInscripMateriaNov).Property(x => x.CimnFecha).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampInscripMateriaNovExists(idAlumno, idMateria, fechaAComparar))
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

        // POST: api/CampInscripMateriaNov
        // Para crear un nuevo registro de novedad de inscripción a materia.
        [HttpPost]
        public async Task<ActionResult<CampInscripMateriaNov>> PostCampInscripMateriaNov(CampInscripMateriaNov campInscripMateriaNov)
        {
             if (_context.CampInscripMateriaNovs == null)
            {
                 return Problem("La entidad 'CampInscripMateriasNov' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(campInscripMateriaNov.CimnIdAlumno))
             {
                 return BadRequest("El IdAlumno es requerido.");
             }
             // Establecer fecha si no viene (usualmente debería venir la fecha del evento de inscripción)
             // if(campInscripMateriaNov.CimnFecha == default(DateTime))
             // {
             //     campInscripMateriaNov.CimnFecha = DateTime.UtcNow; // O DateTime.Now
             // }

            _context.CampInscripMateriaNovs.Add(campInscripMateriaNov);
            try
            {
                 // Asume que la combinación de claves primarias es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampInscripMateriaNovExists(campInscripMateriaNov.CimnIdAlumno, campInscripMateriaNov.CimnIdMateria, campInscripMateriaNov.CimnFecha))
                {
                    return Conflict("Ya existe una novedad de inscripción para este alumno/materia/fecha.");
                }
                else
                {
                    // Podría ser un error de FK si el alumno o materia no existen
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampInscripMateriaNov), new {
                 idAlumno = campInscripMateriaNov.CimnIdAlumno,
                 idMateria = campInscripMateriaNov.CimnIdMateria,
                 fecha = campInscripMateriaNov.CimnFecha // Asegúrate que el formato sea compatible con la ruta
                 }, campInscripMateriaNov);
        }

        // DELETE: api/CampInscripMateriaNov/{idAlumno}/{idMateria}/{fecha}
        // Elimina una novedad de inscripción a materia.
        [HttpDelete("{idAlumno}/{idMateria}/{fecha}")]
        public async Task<IActionResult> DeleteCampInscripMateriaNov(string idAlumno, int idMateria, DateTime fecha)
        {
             if (_context.CampInscripMateriaNovs == null)
            {
                 return NotFound("La entidad 'CampInscripMateriasNov' no está configurada en el DbContext.");
            }

            var campInscripMateriaNov = await _context.CampInscripMateriaNovs.FindAsync(idAlumno, idMateria, fecha);
            if (campInscripMateriaNov == null)
            {
                return NotFound();
            }

            _context.CampInscripMateriaNovs.Remove(campInscripMateriaNov);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampInscripMateriaNovExists(string idAlumno, int idMateria, DateOnly fecha)
        {
            //DateOnly fechaAComparar = DateOnly.FromDateTime(fecha);
            return (_context.CampInscripMateriaNovs?.Any(e =>
                e.CimnIdAlumno == idAlumno &&
                e.CimnIdMateria == idMateria &&
                e.CimnFecha == fecha)).GetValueOrDefault();
        }
    }
}