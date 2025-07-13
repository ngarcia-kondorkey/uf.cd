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
    public class ParametroController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ParametroController> _logger;

        public ParametroController(ExtranetContext context, IConfiguration configuration, ILogger<ParametroController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/Parametro
        // Obtiene todos los parámetros del sistema.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parametro>>> GetParametros()
        {
            if (_context.Parametros == null)
            {
                return NotFound("La entidad 'Parametros' no está configurada en el DbContext.");
            }
            // Considera filtrar u ordenar si la lista es muy grande.
            return await _context.Parametros.ToListAsync();
        }

        // GET: api/Parametro/{empre}/{clave}
        // Busca un parámetro específico por la clave primaria compuesta asumida (Empre, Clave).
        // Nota: Esta implementación asume que ParaAtributo NO es parte de la clave única.
        // Si ParaAtributo es parte de la clave, este endpoint y la lógica necesitarían ajustes.
        [HttpGet("{empre}/{clave}")]
        public async Task<ActionResult<Parametro>> GetParametro(int empre, string clave)
        {
            if (_context.Parametros == null)
            {
                 return NotFound("La entidad 'Parametros' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria compuesta asumida (Empre, Clave).
            // FindAsync buscará usando las claves primarias definidas en el DbContext.
            // Si ParaAtributo es parte de la PK en el DbContext, FindAsync requerirá ese valor también.
            var parametro = await _context.Parametros.FindAsync(empre, clave);

            // Si FindAsync no funciona como se espera (porque ParaAtributo es parte de la PK),
            // podrías necesitar buscar así (asumiendo que la combinación Empre+Clave debería dar un único resultado o el "principal"):
            // var parametro = await _context.Parametros
            //                            .FirstOrDefaultAsync(p => p.ParaEmpre == empre && p.ParaClave == clave);


            if (parametro == null)
            {
                // Podría ser útil devolver una lista si Empre+Clave no es único debido a ParaAtributo:
                // var parametros = await _context.Parametros
                //     .Where(p => p.ParaEmpre == empre && p.ParaClave == clave).ToListAsync();
                // if (!parametros.Any()) return NotFound();
                // return Ok(parametros);
                return NotFound($"No se encontró parámetro para Empre={empre}, Clave={clave}");
            }

            return parametro;
        }

        // PUT: api/Parametro/{empre}/{clave}
        // Para actualizar un parámetro existente usando la clave compuesta asumida (Empre, Clave).
        // ADVERTENCIA: Si la clave real incluye ParaAtributo, este método podría actualizar
        // el registro incorrecto o fallar si múltiples registros coinciden con Empre+Clave.
        [HttpPut("{empre}/{clave}")]
        public async Task<IActionResult> PutParametro(int empre, string clave, Parametro parametro)
        {
            // Valida que la clave compuesta de la ruta coincida con la del objeto
            if (empre != parametro.ParaEmpre || clave != parametro.ParaClave)
            {
                return BadRequest("La clave (Empre, Clave) de la ruta no coincide con la clave del objeto.");
            }

             if (_context.Parametros == null)
            {
                 return NotFound("La entidad 'Parametros' no está configurada en el DbContext.");
            }

            DateOnly fechaAComparar = DateOnly.FromDateTime(DateTime.UtcNow);
            // Establecer fecha y usuario de modificación
            parametro.ParaFchModi = fechaAComparar; // O DateTime.Now
            // parametro.ParaUsrModi = User.Identity?.Name; // Obtener usuario autenticado si aplica

            // Solo permite modificar campos que no son parte de la clave primaria asumida
            _context.Entry(parametro).State = EntityState.Modified;
            _context.Entry(parametro).Property(x => x.ParaEmpre).IsModified = false; // Previene modificación PK
            _context.Entry(parametro).Property(x => x.ParaClave).IsModified = false; // Previene modificación PK
            // Si ParaAtributo es parte de la PK, también prevenir su modificación aquí.


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // La verificación de existencia también asume la clave Empre+Clave
                if (!ParametroExists(empre, clave))
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

        // POST: api/Parametro
        // Para crear un nuevo parámetro.
        [HttpPost]
        public async Task<ActionResult<Parametro>> PostParametro(Parametro parametro)
        {
             if (_context.Parametros == null)
            {
                 return Problem("La entidad 'Parametros' no está configurada en el DbContext.");
            }

            DateOnly fechaAComparar = DateOnly.FromDateTime(DateTime.UtcNow);

             // Establecer fecha y usuario de alta
             parametro.ParaFchAlta = fechaAComparar; // O DateTime.Now
             // parametro.ParaUsrAlta = User.Identity?.Name; // Obtener usuario autenticado si aplica
             parametro.ParaFchModi = null; // Asegurar que fecha modi sea null al crear
             parametro.ParaUsrModi = null;


            _context.Parametros.Add(parametro);
            try
            {
                 // Asume que la combinación de claves primarias (Empre, Clave, [Atributo]) es única.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Verifica si el registro ya existe (violación de PK compuesta asumida Empre+Clave)
                if (ParametroExists(parametro.ParaEmpre, parametro.ParaClave)) // Ajustar si la PK real es diferente
                {
                    return Conflict($"Ya existe un parámetro con la clave (Empre={parametro.ParaEmpre}, Clave={parametro.ParaClave}). Considere el campo Atributo si es parte de la clave.");
                }
                else
                {
                    throw;
                }
            }

            // Devuelve una respuesta 201 Created con la ubicación del nuevo recurso
            // Nota: El routeValues asume la PK Empre+Clave. Ajustar si es diferente.
            return CreatedAtAction(nameof(GetParametro), new {
                 empre = parametro.ParaEmpre,
                 clave = parametro.ParaClave
                 }, parametro);
        }

        // DELETE: api/Parametro/{empre}/{clave}
        // Elimina un parámetro.
        // ADVERTENCIA: Si la clave real incluye ParaAtributo, este método podría eliminar
        // el registro incorrecto o fallar si múltiples registros coinciden con Empre+Clave.
        [HttpDelete("{empre}/{clave}")]
        public async Task<IActionResult> DeleteParametro(int empre, string clave)
        {
             if (_context.Parametros == null)
            {
                 return NotFound("La entidad 'Parametros' no está configurada en el DbContext.");
            }

            // Busca asumiendo la PK Empre+Clave. Puede ser ambiguo si ParaAtributo es parte de la clave real.
            var parametro = await _context.Parametros.FindAsync(empre, clave);

            // Alternativa si Empre+Clave no es única:
            // var parametrosAEliminar = await _context.Parametros
            //     .Where(p => p.ParaEmpre == empre && p.ParaClave == clave).ToListAsync();
            // if (!parametrosAEliminar.Any()) return NotFound();
            // _context.Parametros.RemoveRange(parametrosAEliminar);

            if (parametro == null)
            {
                return NotFound($"No se encontró parámetro para Empre={empre}, Clave={clave}");
            }

            _context.Parametros.Remove(parametro);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por la clave compuesta asumida (Empre, Clave)
        private bool ParametroExists(int empre, string clave)
        {
            // Esta verificación también ignora ParaAtributo. Ajustar si es parte de la PK.
            return (_context.Parametros?.Any(e =>
                e.ParaEmpre == empre &&
                e.ParaClave == clave)).GetValueOrDefault();
        }
    }
}