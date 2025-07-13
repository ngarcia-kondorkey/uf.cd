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
    public class CampLocalidadeController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampLocalidadeController> _logger;

        public CampLocalidadeController(ExtranetContext context, IConfiguration configuration, ILogger<CampLocalidadeController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampLocalidade
        // Obtiene todas las localidades.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampLocalidade>>> GetCampLocalidades()
        {
            if (_context.CampLocalidades == null)
            {
                return NotFound("La entidad 'CampLocalidades' no está configurada en el DbContext.");
            }
             // Advertencia: Devolver todas las localidades puede ser ineficiente.
             // Considera filtrar por provincia/país o implementar paginación.
            return await _context.CampLocalidades.ToListAsync();
        }

        // GET: api/CampLocalidade/{idPais}/{idProvincia}/{idLocalidad}
        // Busca una localidad específica por su clave primaria compuesta asumida.
        [HttpGet("{idPais}/{idProvincia}/{idLocalidad}")]
        public async Task<ActionResult<CampLocalidade>> GetCampLocalidade(int idPais, int idProvincia, int idLocalidad)
        {
            if (_context.CampLocalidades == null)
            {
                 return NotFound("La entidad 'CampLocalidades' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campLocalidade = await _context.CampLocalidades.FindAsync(idPais, idProvincia, idLocalidad);

            if (campLocalidade == null)
            {
                return NotFound();
            }

            return campLocalidade;
        }

        // PUT: api/CampLocalidade/{idPais}/{idProvincia}/{idLocalidad}
        // Para actualizar una localidad existente.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPut("{idPais}/{idProvincia}/{idLocalidad}")]
        public async Task<IActionResult> PutCampLocalidade(int idPais, int idProvincia, int idLocalidad, CampLocalidade campLocalidade)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (idPais != campLocalidade.ClocIdPais ||
                idProvincia != campLocalidade.ClocIdProvincia ||
                idLocalidad != campLocalidade.ClocIdLocalidad)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto.");
            }

             if (_context.CampLocalidades == null)
            {
                 return NotFound("La entidad 'CampLocalidades' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
            _context.Entry(campLocalidade).State = EntityState.Modified;
            _context.Entry(campLocalidade).Property(x => x.ClocIdPais).IsModified = false; // Previene modificación PK
            _context.Entry(campLocalidade).Property(x => x.ClocIdProvincia).IsModified = false; // Previene modificación PK
            _context.Entry(campLocalidade).Property(x => x.ClocIdLocalidad).IsModified = false; // Previene modificación PK


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampLocalidadeExists(idPais, idProvincia, idLocalidad))
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

        // POST: api/CampLocalidade
        // Para crear una nueva localidad.
        // Nota: Podría estar restringido a roles administrativos.
        [HttpPost]
        public async Task<ActionResult<CampLocalidade>> PostCampLocalidade(CampLocalidade campLocalidade)
        {
             if (_context.CampLocalidades == null)
            {
                 return Problem("La entidad 'CampLocalidades' no está configurada en el DbContext.");
            }

             // Opcional: Validaciones adicionales
             if(string.IsNullOrWhiteSpace(campLocalidade.ClocLocalidad))
             {
                 return BadRequest("El nombre de la localidad es requerido.");
             }

            _context.CampLocalidades.Add(campLocalidade);
            try
            {
                 // Asume que la combinación de claves primarias es única y se proveen valores válidos.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampLocalidadeExists(campLocalidade.ClocIdPais, campLocalidade.ClocIdProvincia, campLocalidade.ClocIdLocalidad))
                {
                    return Conflict("Ya existe una localidad con esta clave (País, Provincia, Localidad).");
                }
                else
                {
                     // Podría ser un error de FK si el país o provincia no existen
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampLocalidade), new {
                 idPais = campLocalidade.ClocIdPais,
                 idProvincia = campLocalidade.ClocIdProvincia,
                 idLocalidad = campLocalidade.ClocIdLocalidad
                 }, campLocalidade);
        }

        // DELETE: api/CampLocalidade/{idPais}/{idProvincia}/{idLocalidad}
        // Elimina una localidad.
        // Nota: Podría estar restringido o fallar si existen registros (ej: Alumnos, Institutos) asociados.
        [HttpDelete("{idPais}/{idProvincia}/{idLocalidad}")]
        public async Task<IActionResult> DeleteCampLocalidade(int idPais, int idProvincia, int idLocalidad)
        {
             if (_context.CampLocalidades == null)
            {
                 return NotFound("La entidad 'CampLocalidades' no está configurada en el DbContext.");
            }

            var campLocalidade = await _context.CampLocalidades.FindAsync(idPais, idProvincia, idLocalidad);
            if (campLocalidade == null)
            {
                return NotFound();
            }

            _context.CampLocalidades.Remove(campLocalidade);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Capturar errores de FK si no se puede borrar porque está en uso
                  return Problem($"No se pudo eliminar la localidad. Es posible que esté en uso. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampLocalidadeExists(int idPais, int idProvincia, int idLocalidad)
        {
            return (_context.CampLocalidades?.Any(e =>
                e.ClocIdPais == idPais &&
                e.ClocIdProvincia == idProvincia &&
                e.ClocIdLocalidad == idLocalidad)).GetValueOrDefault();
        }
    }
}