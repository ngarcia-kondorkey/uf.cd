using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using uf.cd.db.Model; // Asumiendo que el modelo está en este namespace
using System; // Necesario para DateTime
using Microsoft.AspNetCore.Http; // Necesario para StatusCodes

namespace uf.cd.api.Controllers // Namespace especificado para el controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuIncidentesLogController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuIncidentesLogController> _logger;

        public UsuIncidentesLogController(ExtranetContext context, IConfiguration configuration, ILogger<UsuIncidentesLogController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/UsuIncidentesLog
        // Obtiene todos los registros de log de incidentes. ¡Considera paginación y filtrado para producción!
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuIncidentesLog>>> GetUsuIncidentesLogs()
        {
            // Verifica si el DbSet existe en el contexto
            if (_context.UsuIncidentesLogs == null)
            {
                return NotFound("La entidad 'UsuIncidentesLogs' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los logs puede ser muy ineficiente.
            // Ordenar por fecha y orden puede ser útil.
            return await _context.UsuIncidentesLogs
                .OrderByDescending(log => log.UsilFechaHora)
                .ThenByDescending(log => log.UsilOrden) // Asume que Orden es relevante para desempates
                .ToListAsync();
        }

        // GET: api/UsuIncidentesLog/filter?usuario=X&fechaDesde=YYYY-MM-DD&fechaHasta=YYYY-MM-DD&proceso=P...
        // Obtiene logs filtrados por los parámetros proporcionados (opcionales).
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<UsuIncidentesLog>>> GetFilteredUsuIncidentesLogs(
            [FromQuery] string? usuario,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] string? proceso,
            [FromQuery] int? empre)
        {
             if (_context.UsuIncidentesLogs == null)
            {
                return NotFound("La entidad 'UsuIncidentesLogs' no está configurada en el DbContext.");
            }

            var query = _context.UsuIncidentesLogs.AsQueryable();

            if (!string.IsNullOrEmpty(usuario))
            {
                query = query.Where(log => log.UsilUsuaNombre == usuario);
            }
             if (fechaDesde.HasValue)
            {
                 query = query.Where(log => log.UsilFechaHora >= fechaDesde.Value);
            }
            if (fechaHasta.HasValue)
            {
                 // Asegúrate de incluir la fecha hasta completa si es necesario
                 query = query.Where(log => log.UsilFechaHora <= fechaHasta.Value);
            }
            if (!string.IsNullOrEmpty(proceso))
            {
                 query = query.Where(log => log.UsilProceso == proceso);
            }
            if (empre.HasValue)
            {
                query = query.Where(log => log.UsilEmpre == empre.Value);
            }
            // Puedes añadir más filtros aquí si es necesario (ej: por UsilErrorTest)

            return await query
                .OrderByDescending(log => log.UsilFechaHora)
                .ThenByDescending(log => log.UsilOrden)
                .ToListAsync();
        }


        // POST: api/UsuIncidentesLog
        // Para crear un nuevo registro de log de incidente.
        // Nota: Usualmente los logs se crean mediante procesos internos (ej: al capturar una excepción)
        // y no directamente vía API, pero se incluye la posibilidad.
        [HttpPost]
        public async Task<ActionResult<UsuIncidentesLog>> PostUsuIncidentesLog(UsuIncidentesLog usuIncidentesLog)
        {
             if (_context.UsuIncidentesLogs == null)
            {
                 return Problem("La entidad 'UsuIncidentesLogs' no está configurada en el DbContext.");
            }

             // Opcional: Establecer la fecha/hora del log si no viene en el request
             // if(usuIncidentesLog.UsilFechaHora == default(DateTime))
             // {
             //     usuIncidentesLog.UsilFechaHora = DateTime.UtcNow; // O DateTime.Now
             // }
             // Opcional: Validar campos requeridos no nulos
             if(string.IsNullOrWhiteSpace(usuIncidentesLog.UsilUsuaNombre) ||
                string.IsNullOrWhiteSpace(usuIncidentesLog.UsilErrorTest) ||
                string.IsNullOrWhiteSpace(usuIncidentesLog.UsilProceso))
             {
                return BadRequest("UsuaNombre, ErrorTest y Proceso son requeridos.");
             }


            _context.UsuIncidentesLogs.Add(usuIncidentesLog);
            try
            {
                // Asume que la BD maneja la generación de UsilOrden si es necesario para unicidad con FechaHora,
                // o que la combinación FechaHora+Orden+Usuario es suficientemente única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar excepciones específicas si es necesario
                 return Problem($"Error al guardar el log de incidente: {ex.Message}");
            }

            // Devuelve 201 Created. Como es un log, no se espera típicamente
            // recuperarlo por una clave simple, por lo que no se usa CreatedAtAction.
            // Se devuelve el objeto creado.
            return StatusCode(StatusCodes.Status201Created, usuIncidentesLog);
        }

        // --- Métodos GET por ID, PUT y DELETE OMITIDOS ---
        // Siendo una tabla de log ('IncidentesLog') sin una clave primaria clara y única
        // estándar (no hay un 'Id', y combinaciones como FechaHora+Orden+Usuario podrían ser la clave lógica
        // pero no son ideales para un API REST CRUD estándar), las operaciones de
        // GET por ID, PUT (modificar) y DELETE (eliminar) generalmente no se aplican o son
        // desaconsejadas para logs, que suelen ser inmutables o solo de adición.
        // La consulta se realiza típicamente filtrando por criterios relevantes.

    }
}