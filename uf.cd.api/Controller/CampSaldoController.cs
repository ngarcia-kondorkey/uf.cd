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
    public class CampSaldoController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampSaldoController> _logger;

        public CampSaldoController(ExtranetContext context, IConfiguration configuration, ILogger<CampSaldoController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampSaldo
        // Obtiene todos los registros de saldo.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CampSaldo>>> GetCampSaldos()
        {
            if (_context.CampSaldos == null)
            {
                return NotFound("La entidad 'CampSaldos' no está configurada en el DbContext.");
            }
            // Advertencia: Devolver todos los saldos puede ser ineficiente y exponer datos.
            // Considera aplicar filtros obligatorios o paginación.
            return await _context.CampSaldos.ToListAsync();
        }

        // GET: api/CampSaldo/{idAlumno}
        // Busca un registro de saldo por la clave primaria asumida CsalIdAlumnos (string)
        // Nota: Si un alumno puede tener múltiples saldos (ej: por carrera o factura), esta lógica es incorrecta.
        [HttpGet("{idAlumno}")]
        public async Task<ActionResult<CampSaldo>> GetCampSaldo(string idAlumno)
        {
            if (_context.CampSaldos == null)
            {
                 return NotFound("La entidad 'CampSaldos' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria asumida.
            // Si esta no es la PK real, FindAsync fallará o devolverá un resultado inesperado.
            var campSaldo = await _context.CampSaldos.FindAsync(idAlumno);

            if (campSaldo == null)
            {
                return NotFound($"No se encontró saldo para el alumno con ID {idAlumno}");
            }

            return campSaldo;
        }

        // PUT: api/CampSaldo/{idAlumno}
        // Para actualizar un registro de saldo existente.
        [HttpPut("{idAlumno}")]
        public async Task<IActionResult> PutCampSaldo(string idAlumno, CampSaldo campSaldo)
        {
            // Valida que el ID de la ruta coincida con el ID del objeto
            if (idAlumno != campSaldo.CsalIdAlumnos)
            {
                return BadRequest("El ID de alumno de la ruta no coincide con el ID del objeto saldo.");
            }

             if (_context.CampSaldos == null)
            {
                 return NotFound("La entidad 'CampSaldos' no está configurada en el DbContext.");
            }

             // Advertencia: CsalFechaFactura es string?, CsalTotalFactura/CsalSaldo son float?. Considerar validación/conversión.

            // Solo permite modificar campos que no son parte de la clave primaria (CsalIdAlumnos)
            _context.Entry(campSaldo).State = EntityState.Modified;
             _context.Entry(campSaldo).Property(x => x.CsalIdAlumnos).IsModified = false; // Previene modificación PK

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampSaldoExists(idAlumno))
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
                 return BadRequest($"No se puede modificar la clave primaria (CsalIdAlumnos). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampSaldo
        // Para crear un nuevo registro de saldo.
        [HttpPost]
        public async Task<ActionResult<CampSaldo>> PostCampSaldo(CampSaldo campSaldo)
        {
             if (_context.CampSaldos == null)
            {
                 return Problem("La entidad 'CampSaldos' no está configurada en el DbContext.");
            }

            // Advertencia: CsalFechaFactura es string?, CsalTotalFactura/CsalSaldo son float?. Considerar validación/conversión.

            _context.CampSaldos.Add(campSaldo);
            try
            {
                 // Asume que CsalIdAlumnos debe ser único si es la PK.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Podría ser una violación de PK si CsalIdAlumnos debe ser único y ya existe
                 if (CampSaldoExists(campSaldo.CsalIdAlumnos))
                 {
                     return Conflict($"Ya existe un registro de saldo para el alumno con ID {campSaldo.CsalIdAlumnos}");
                 }
                 else
                 {
                     return Problem($"Error al guardar el saldo: {ex.InnerException?.Message ?? ex.Message}");
                 }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            return CreatedAtAction(nameof(GetCampSaldo), new { idAlumno = campSaldo.CsalIdAlumnos }, campSaldo);
        }

        // DELETE: api/CampSaldo/{idAlumno}
        // Elimina un registro de saldo.
        // Nota: Eliminar saldos puede no ser una operación común; quizás se actualiza a cero o se inactiva.
        [HttpDelete("{idAlumno}")]
        public async Task<IActionResult> DeleteCampSaldo(string idAlumno)
        {
             if (_context.CampSaldos == null)
            {
                 return NotFound("La entidad 'CampSaldos' no está configurada en el DbContext.");
            }

            var campSaldo = await _context.CampSaldos.FindAsync(idAlumno);
            if (campSaldo == null)
            {
                return NotFound($"No se encontró saldo para el alumno con ID {idAlumno}");
            }

            _context.CampSaldos.Remove(campSaldo);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su ID (asumiendo CsalIdAlumnos es PK aquí)
        private bool CampSaldoExists(string idAlumno)
        {
            return (_context.CampSaldos?.Any(e => e.CsalIdAlumnos == idAlumno)).GetValueOrDefault();
        }
    }
}