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
    public class CampPagosLogSponsorController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampPagosLogSponsorController> _logger;

        public CampPagosLogSponsorController(ExtranetContext context, IConfiguration configuration, ILogger<CampPagosLogSponsorController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampPagosLogSponsor
        // Obtiene todos los registros de log de pagos de sponsor. ¡Considera paginación y filtrado para producción!
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampPagosLogSponsor>>> GetCampPagosLogsSponsor()
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.CampPagosLogSponsors == null)
            {
                return NotFound("La entidad 'CampPagosLogsSponsor' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los logs puede ser muy ineficiente.
            // Ordenar por fecha puede ser útil.
            return await _context.CampPagosLogSponsors.OrderByDescending(log => log.Fecha).ToListAsync();
        }

        // GET: api/CampPagosLogSponsor/filter?alumno=X&factura=Y&estado=Z&fechaDesde=YYYY-MM-DD&fechaHasta=YYYY-MM-DD
        // Obtiene logs filtrados por los parámetros proporcionados (opcionales)
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<CampPagosLogSponsor>>> GetFilteredCampPagosLogsSponsor(
            [FromQuery] string? alumno,
            [FromQuery] string? factura,
            [FromQuery] string? estado,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta)
        {
             if (_context.CampPagosLogSponsors == null)
            {
                return NotFound("La entidad 'CampPagosLogsSponsor' no está configurada en el DbContext.");
            }

            var query = _context.CampPagosLogSponsors.AsQueryable();

            if (!string.IsNullOrEmpty(alumno))
            {
                // Considera usar Contains o verificar IDs si 'Alumno' guarda un ID o nombre parcial.
                query = query.Where(log => log.Alumno == alumno);
            }
            if (!string.IsNullOrEmpty(factura))
            {
                query = query.Where(log => log.Factura == factura);
            }
            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(log => log.Estado == estado);
            }
             if (fechaDesde.HasValue)
            {
                 query = query.Where(log => log.Fecha >= fechaDesde.Value);
            }
            if (fechaHasta.HasValue)
            {
                 // Asegúrate de incluir la fecha hasta completa si es necesario (ej: sumar un día si es solo fecha)
                 query = query.Where(log => log.Fecha <= fechaHasta.Value);
            }

            return await query.OrderByDescending(log => log.Fecha).ToListAsync();
        }


        // POST: api/CampPagosLogSponsor
        // Para crear un nuevo registro de log de pago de sponsor.
        // Nota: Usualmente los logs se crean mediante procesos internos
        // y no directamente vía API, pero se incluye la posibilidad.
        [HttpPost]
        public async Task<ActionResult<CampPagosLogSponsor>> PostCampPagosLogSponsor(CampPagosLogSponsor campPagosLogSponsor)
        {
             if (_context.CampPagosLogSponsors == null)
            {
                 return Problem("La entidad 'CampPagosLogsSponsor' no está configurada en el DbContext.");
            }

             // Opcional: Establecer la fecha del log si no viene en el request
             // if (!campPagosLogSponsor.Fecha.HasValue)
             // {
             //     campPagosLogSponsor.Fecha = DateTime.UtcNow; // O DateTime.Now
             // }
             // Opcional: Asegurar que el Id sea 0 si es autogenerado por BD
             // campPagosLogSponsor.Id = 0;

             // Advertencia: El campo 'Monto' es string?. Considera validarlo o convertirlo si es necesario.

            _context.CampPagosLogSponsors.Add(campPagosLogSponsor);
            try
            {
                 // El Id probablemente será generado por la base de datos al guardar.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar excepciones específicas si es necesario
                 return Problem($"Error al guardar el log de pago de sponsor: {ex.Message}");
            }

            // Devuelve 201 Created. Como es un log, no se espera típicamente
            // recuperarlo por su ID secuencial, por lo que no se usa CreatedAtAction.
            // Se devuelve el objeto creado (incluyendo el ID generado por la BD).
            return StatusCode(StatusCodes.Status201Created, campPagosLogSponsor);
        }

        // --- Métodos GET por ID, PUT y DELETE OMITIDOS ---
        // Siendo una tabla de log ('PagosLogSponsor') con un ID probablemente autoincremental,
        // las operaciones estándar de GET por ID, PUT (modificar) y DELETE (eliminar)
        // generalmente no se aplican o son desaconsejadas para logs, que suelen ser inmutables.
        // La consulta se realiza típicamente filtrando por criterios relevantes (alumno, factura, fecha, estado).

        // Si se necesitara un GET por ID:
        /*
        [HttpGet("{id}")]
        public async Task<ActionResult<CampPagosLogSponsor>> GetCampPagosLogSponsorById(int id)
        {
            if (_context.CampPagosLogsSponsor == null) { return NotFound(); }
            var logEntry = await _context.CampPagosLogsSponsor.FindAsync(id);
            if (logEntry == null) { return NotFound(); }
            return logEntry;
        }
        */

    }
}