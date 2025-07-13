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
    public class UsuAccesosLogController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuAccesosLogController> _logger;

        public UsuAccesosLogController(ExtranetContext context, IConfiguration configuration, ILogger<UsuAccesosLogController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/UsuAccesosLog
        // Obtiene todos los registros de log de acceso. EXCLUYE EL CAMPO 'UsalClave'.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsuAccesosLogs()
        {
            if (_context.UsuAccesosLogs == null)
            {
                return NotFound("La entidad 'UsuAccesosLogs' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los logs puede ser muy ineficiente.
            // Considera paginación y filtrado obligatorio en producción.
            // Selecciona explícitamente los campos para evitar devolver UsalClave.
            return await _context.UsuAccesosLogs
                .OrderByDescending(log => log.UsalFechaHora) // Ordenar por fecha es común para logs
                .Select(log => new {
                    log.UsalFechaHora,
                    log.UsalOrden,
                    log.UsalUsuaNombre,
                    // No incluir log.UsalClave,
                    log.UsalCodigoNumber,
                    log.UsalCodigoChar,
                    log.UsalVistas,
                    log.UsalHabilitado,
                    log.UsalAltaF,
                    log.UsalUsrAlta,
                    log.UsalMofiF,
                    log.UsalUsrModi,
                    log.UsalEmpre
                })
                .ToListAsync();
        }

        // GET: api/UsuAccesosLog/filter?usuario=X&fechaDesde=YYYY-MM-DD&fechaHasta=YYYY-MM-DD...
        // Obtiene logs filtrados por los parámetros proporcionados (opcionales). EXCLUYE 'UsalClave'.
        [HttpGet("filter")]
        public async Task<ActionResult<IEnumerable<object>>> GetFilteredUsuAccesosLogs(
            [FromQuery] string? usuario,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] int? empre,
            [FromQuery] string? codigoChar,
            [FromQuery] int? codigoNumber)
        {
             if (_context.UsuAccesosLogs == null)
            {
                return NotFound("La entidad 'UsuAccesosLogs' no está configurada en el DbContext.");
            }

            var query = _context.UsuAccesosLogs.AsQueryable();

            if (!string.IsNullOrEmpty(usuario))
            {
                query = query.Where(log => log.UsalUsuaNombre == usuario);
            }
             if (fechaDesde.HasValue)
            {
                 query = query.Where(log => log.UsalFechaHora >= fechaDesde.Value);
            }
            if (fechaHasta.HasValue)
            {
                 // Asegúrate de incluir la fecha hasta completa si es necesario
                 query = query.Where(log => log.UsalFechaHora <= fechaHasta.Value);
            }
            if (empre.HasValue)
            {
                query = query.Where(log => log.UsalEmpre == empre.Value);
            }
             if (!string.IsNullOrEmpty(codigoChar))
            {
                query = query.Where(log => log.UsalCodigoChar == codigoChar);
            }
            if (codigoNumber.HasValue)
            {
                query = query.Where(log => log.UsalCodigoNumber == codigoNumber.Value);
            }
            // Puedes añadir más filtros aquí si es necesario

            // Selecciona explícitamente los campos para evitar devolver UsalClave.
            return await query
                .OrderByDescending(log => log.UsalFechaHora)
                .Select(log => new {
                    log.UsalFechaHora,
                    log.UsalOrden,
                    log.UsalUsuaNombre,
                    // No incluir log.UsalClave,
                    log.UsalCodigoNumber,
                    log.UsalCodigoChar,
                    log.UsalVistas,
                    log.UsalHabilitado,
                    log.UsalAltaF,
                    log.UsalUsrAlta,
                    log.UsalMofiF,
                    log.UsalUsrModi,
                    log.UsalEmpre
                })
                .ToListAsync();
        }


        // POST: api/UsuAccesosLog
        // Para crear un nuevo registro de log de acceso.
        // Nota: Usualmente los logs se crean mediante procesos internos (ej: al iniciar sesión)
        // y no directamente vía API, pero se incluye la posibilidad.
        // ADVERTENCIA DE SEGURIDAD: Este método guarda el campo 'UsalClave' tal como viene.
        // Si representa información sensible (parte de credenciales, etc.), NO debe guardarse así.
        [HttpPost]
        public async Task<ActionResult<UsuAccesosLog>> PostUsuAccesosLog(UsuAccesosLog usuAccesosLog)
        {
             // **** ADVERTENCIA DE SEGURIDAD ****
             // El modelo UsuAccesosLog contiene un campo 'UsalClave'. Si esto representa
             // información sensible (como parte de una contraseña o token), NUNCA se debe
             // guardar directamente el valor del request. Este código generado lo haría.
             // Considera eliminar este campo del log o asegurar que no contenga datos sensibles.
             // **********************************

             if (_context.UsuAccesosLogs == null)
            {
                 return Problem("La entidad 'UsuAccesosLogs' no está configurada en el DbContext.");
            }

             // Establecer fecha/hora del log si no viene
             // if(usuAccesosLog.UsalFechaHora == default(DateTime))
             // {
             //     usuAccesosLog.UsalFechaHora = DateTime.UtcNow; // O DateTime.Now
             // }
             // Establecer usuario/fecha de alta si aplica
             // usuAccesosLog.UsalAltaF = DateTime.UtcNow;
             // usuAccesosLog.UsalUsrAlta = User.Identity?.Name; // Si hay autenticación

            _context.UsuAccesosLogs.Add(usuAccesosLog);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar excepciones específicas si es necesario
                 return Problem($"Error al guardar el log de acceso: {ex.Message}");
            }

            // Devuelve 201 Created. Como es un log, no se espera típicamente
            // recuperarlo por una clave simple, por lo que no se usa CreatedAtAction.
            // ADVERTENCIA: El objeto devuelto contiene UsalClave. Sería más seguro devolver
            // un objeto anónimo o DTO sin ese campo.
            return StatusCode(StatusCodes.Status201Created, usuAccesosLog);
        }

        // --- Métodos GET por ID, PUT y DELETE OMITIDOS ---
        // Siendo una tabla de log ('AccesosLog') sin una clave primaria clara y única
        // (UsalFechaHora podría no ser única, UsalOrden es nullable), las operaciones estándar
        // de GET por ID, PUT (modificar) y DELETE (eliminar) generalmente no se aplican o son
        // desaconsejadas para logs, que suelen ser inmutables o solo de adición.
        // La consulta se realiza típicamente filtrando por criterios relevantes.

    }
}