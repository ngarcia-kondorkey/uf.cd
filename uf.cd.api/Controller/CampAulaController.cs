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
    public class CampAulaController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampAulaController> _logger;

        public CampAulaController(ExtranetContext context, IConfiguration configuration, ILogger<CampAulaController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampAula
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampAula>>> GetCampAulas() // Asumiendo DbSet se llama CampAulas
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampAulas == null)
            {
                return NotFound("La entidad 'CampAulas' no está configurada en el DbContext.");
            }
            return await _context.CampAulas.ToListAsync();
        }

        // GET: api/CampAula/{id}
        // Busca por la clave primaria CaulIdAula (string)
        [HttpGet("{id}")]
        public async Task<ActionResult<CampAula>> GetCampAula(string id)
        {
            if (_context.CampAulas == null)
            {
                 return NotFound("La entidad 'CampAulas' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria.
            var campAula = await _context.CampAulas.FindAsync(id);

            if (campAula == null)
            {
                return NotFound();
            }

            return campAula;
        }

        // PUT: api/CampAula/{id}
        // Para actualizar un registro existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampAula(string id, CampAula campAula)
        {
            // Valida que el ID de la ruta coincida con el ID del objeto
            if (id != campAula.CaulIdAula)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del aula.");
            }

             if (_context.CampAulas == null)
            {
                 return NotFound("La entidad 'CampAulas' no está configurada en el DbContext.");
            }

            _context.Entry(campAula).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampAulaExists(id))
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

        // POST: api/CampAula
        // Para crear un nuevo registro
        [HttpPost]
        public async Task<ActionResult<CampAula>> PostCampAula(CampAula campAula)
        {
             if (_context.CampAulas == null)
            {
                 return Problem("La entidad 'CampAulas' no está configurada en el DbContext.");
            }

            _context.CampAulas.Add(campAula);
            try
            {
                // Asumiendo que CaulIdAula viene en el request y no es autogenerado
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Podría ser una violación de PK si el ID ya existe
                if (CampAulaExists(campAula.CaulIdAula))
                {
                    return Conflict($"Ya existe un aula con el ID {campAula.CaulIdAula}");
                }
                else
                {
                    throw; // Relanza si es otro tipo de error de base de datos
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampAula), new { id = campAula.CaulIdAula }, campAula);
        }

        // DELETE: api/CampAula/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampAula(string id)
        {
             if (_context.CampAulas == null)
            {
                 return NotFound("La entidad 'CampAulas' no está configurada en el DbContext.");
            }

            var campAula = await _context.CampAulas.FindAsync(id);
            if (campAula == null)
            {
                return NotFound();
            }

            _context.CampAulas.Remove(campAula);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un aula existe por su ID
        private bool CampAulaExists(string id)
        {
            // Asegúrate que CampAulas no sea null antes de usarlo
            return (_context.CampAulas?.Any(e => e.CaulIdAula == id)).GetValueOrDefault();
        }
    }
}