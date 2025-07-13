using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uf.cd.db.Model; 


namespace uf.cd.api.Controllers 
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdmisionesController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AdmisionesController> _logger;

        public AdmisionesController(ExtranetContext context, IConfiguration configuration, ILogger<AdmisionesController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las admisiones.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Admisione>>> GetAdmisiones()
        {
            return await _context.Admisiones.ToListAsync();
        }

        /// <summary>
        /// Obtiene una admisión específica por su ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Admisione>> GetAdmisione(int id)
        {
            var admisione = await _context.Admisiones.FindAsync(id);

            if (admisione == null)
            {
                return NotFound();
            }

            return admisione;
        }

        /// <summary>
        /// Crea una nueva admisión.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Admisione>> PostAdmisione(Admisione admisione)
        {
            _context.Admisiones.Add(admisione);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdmisione), new { id = admisione.Id }, admisione);
        }

        /// <summary>
        /// Actualiza una admisión existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAdmisione(int id, Admisione admisione)
        {
            if (id != admisione.Id)
            {
                return BadRequest();
            }

            _context.Entry(admisione).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdmisioneExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina una admisión.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdmisione(int id)
        {
            var admisione = await _context.Admisiones.FindAsync(id);
            if (admisione == null)
            {
                return NotFound();
            }

            _context.Admisiones.Remove(admisione);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AdmisioneExists(int id)
        {
            return _context.Admisiones.Any(e => e.Id == id);
        }
    }
}