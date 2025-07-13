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
    public class CampCronogramaFechaController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampCronogramaFechaController> _logger;

        public CampCronogramaFechaController(ExtranetContext context, IConfiguration configuration, ILogger<CampCronogramaFechaController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampCronogramaFecha
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampCronogramaFecha>>> GetCampCronogramaFechas() // Asumiendo DbSet se llama CampCronogramaFechas
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampCronogramaFechas == null)
            {
                return NotFound("La entidad 'CampCronogramaFechas' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todo el cronograma puede ser ineficiente. Considera paginación/filtrado.
            return await _context.CampCronogramaFechas.ToListAsync();
        }

        // GET: api/CampCronogramaFecha/{tipoExamen}/{idExamen}/{idCarrera}
        // Busca por la clave primaria compuesta asumida.
        [HttpGet("{tipoExamen}/{idExamen}/{idCarrera}")]
        public async Task<ActionResult<CampCronogramaFecha>> GetCampCronogramaFecha(string tipoExamen, int idExamen, int idCarrera)
        {
            if (_context.CampCronogramaFechas == null)
            {
                 return NotFound("La entidad 'CampCronogramaFechas' no está configurada en el DbContext.");
            }

            // FindAsync puede buscar por clave primaria compuesta directamente
            // El orden de los parámetros debe coincidir con la definición de la clave en OnModelCreating
            var campCronogramaFecha = await _context.CampCronogramaFechas.FindAsync(tipoExamen, idExamen, idCarrera);

            if (campCronogramaFecha == null)
            {
                return NotFound();
            }

            return campCronogramaFecha;
        }

        // PUT: api/CampCronogramaFecha/{tipoExamen}/{idExamen}/{idCarrera}
        // Para actualizar un registro existente usando la clave compuesta
        [HttpPut("{tipoExamen}/{idExamen}/{idCarrera}")]
        public async Task<IActionResult> PutCampCronogramaFecha(string tipoExamen, int idExamen, int idCarrera, CampCronogramaFecha campCronogramaFecha)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (tipoExamen != campCronogramaFecha.CcroTipoExamen ||
                idExamen != campCronogramaFecha.CcroIdExamen ||
                idCarrera != campCronogramaFecha.CcroIdCarrera)
            {
                return BadRequest("La clave compuesta de la ruta no coincide con la clave del objeto del cronograma.");
            }

             if (_context.CampCronogramaFechas == null)
            {
                 return NotFound("La entidad 'CampCronogramaFechas' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria
             _context.Entry(campCronogramaFecha).State = EntityState.Modified;
             // Previene la modificación de la clave primaria si se intenta
             _context.Entry(campCronogramaFecha).Property(x => x.CcroTipoExamen).IsModified = false;
             _context.Entry(campCronogramaFecha).Property(x => x.CcroIdExamen).IsModified = false;
             _context.Entry(campCronogramaFecha).Property(x => x.CcroIdCarrera).IsModified = false;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampCronogramaFechaExists(tipoExamen, idExamen, idCarrera))
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

        // POST: api/CampCronogramaFecha
        // Para crear un nuevo registro en el cronograma
        [HttpPost]
        public async Task<ActionResult<CampCronogramaFecha>> PostCampCronogramaFecha(CampCronogramaFecha campCronogramaFecha)
        {
             if (_context.CampCronogramaFechas == null)
            {
                 return Problem("La entidad 'CampCronogramaFechas' no está configurada en el DbContext.");
            }

            _context.CampCronogramaFechas.Add(campCronogramaFecha);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta)
                if (CampCronogramaFechaExists(campCronogramaFecha.CcroTipoExamen, campCronogramaFecha.CcroIdExamen, campCronogramaFecha.CcroIdCarrera))
                {
                    return Conflict("Ya existe un registro en el cronograma con esta clave compuesta.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampCronogramaFecha), new {
                tipoExamen = campCronogramaFecha.CcroTipoExamen,
                idExamen = campCronogramaFecha.CcroIdExamen,
                idCarrera = campCronogramaFecha.CcroIdCarrera
                }, campCronogramaFecha);
        }

        // DELETE: api/CampCronogramaFecha/{tipoExamen}/{idExamen}/{idCarrera}
        // Elimina por clave primaria compuesta asumida
        [HttpDelete("{tipoExamen}/{idExamen}/{idCarrera}")]
        public async Task<IActionResult> DeleteCampCronogramaFecha(string tipoExamen, int idExamen, int idCarrera)
        {
             if (_context.CampCronogramaFechas == null)
            {
                 return NotFound("La entidad 'CampCronogramaFechas' no está configurada en el DbContext.");
            }

            var campCronogramaFecha = await _context.CampCronogramaFechas.FindAsync(tipoExamen, idExamen, idCarrera);
            if (campCronogramaFecha == null)
            {
                return NotFound();
            }

            _context.CampCronogramaFechas.Remove(campCronogramaFecha);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampCronogramaFechaExists(string tipoExamen, int idExamen, int idCarrera)
        {
            // Asegúrate que CampCronogramaFechas no sea null antes de usarlo
            return (_context.CampCronogramaFechas?.Any(e =>
                e.CcroTipoExamen == tipoExamen &&
                e.CcroIdExamen == idExamen &&
                e.CcroIdCarrera == idCarrera)).GetValueOrDefault();
        }
    }
}