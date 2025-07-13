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
    public class CampCarreraController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampCarreraController> _logger;

        public CampCarreraController(ExtranetContext context, IConfiguration configuration, ILogger<CampCarreraController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampCarrera
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampCarrera>>> GetCampCarreras() // Asumiendo DbSet se llama CampCarreras
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampCarreras == null)
            {
                return NotFound("La entidad 'CampCarreras' no está configurada en el DbContext.");
            }
            return await _context.CampCarreras.ToListAsync();
        }

        // GET: api/CampCarrera/{id}
        // Busca por la clave primaria CcaaIdCarrera (int)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampCarrera>> GetCampCarrera(int id)
        {
            if (_context.CampCarreras == null)
            {
                 return NotFound("La entidad 'CampCarreras' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campCarrera = await _context.CampCarreras.FindAsync(id);

            if (campCarrera == null)
            {
                return NotFound();
            }

            return campCarrera;
        }

        // PUT: api/CampCarrera/{id}
        // Para actualizar un registro existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampCarrera(int id, CampCarrera campCarrera)
        {
            // Valida que el ID de la ruta coincida con el ID del objeto
            if (id != campCarrera.CcaaIdCarrera)
            {
                return BadRequest("El ID de la ruta no coincide con el ID de la carrera.");
            }

             if (_context.CampCarreras == null)
            {
                 return NotFound("La entidad 'CampCarreras' no está configurada en el DbContext.");
            }

            _context.Entry(campCarrera).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampCarreraExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw; // Relanza la excepción si ocurrió otro error de concurrencia
                }
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampCarrera
        // Para crear un nuevo registro
        [HttpPost]
        public async Task<ActionResult<CampCarrera>> PostCampCarrera(CampCarrera campCarrera)
        {
             if (_context.CampCarreras == null)
            {
                 return Problem("La entidad 'CampCarreras' no está configurada en el DbContext.");
            }

            _context.CampCarreras.Add(campCarrera);
            try
            {
                 // Si CcaaIdCarrera es Identity (autoincremental), se generará al guardar.
                 // Si no lo es, asegúrate que venga con un valor único en el request.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si el ID NO es autogenerado y ya existe
                 // O un error de constraint, etc.
                 // Es menos común tener conflictos de PK con IDs enteros autogenerados.
                 // Considera loguear el error 'ex' o devolver un Problem() más específico.
                 return Problem($"Error al guardar la carrera: {ex.InnerException?.Message ?? ex.Message}");
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampCarrera), new { id = campCarrera.CcaaIdCarrera }, campCarrera);
        }

        // DELETE: api/CampCarrera/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampCarrera(int id)
        {
             if (_context.CampCarreras == null)
            {
                 return NotFound("La entidad 'CampCarreras' no está configurada en el DbContext.");
            }

            var campCarrera = await _context.CampCarreras.FindAsync(id);
            if (campCarrera == null)
            {
                return NotFound();
            }

            _context.CampCarreras.Remove(campCarrera);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si una carrera existe por su ID
        private bool CampCarreraExists(int id)
        {
            // Asegúrate que CampCarreras no sea null antes de usarlo
            return (_context.CampCarreras?.Any(e => e.CcaaIdCarrera == id)).GetValueOrDefault();
        }
    }
}