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
    public class CampOtroController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampOtroController> _logger;

        public CampOtroController(ExtranetContext context, IConfiguration configuration, ILogger<CampOtroController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampOtro
        // Obtiene todos los registros "Otros". EXCLUYE EL CAMPO 'Pass' por seguridad.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCampOtros()
        {
            if (_context.CampOtros == null)
            {
                return NotFound("La entidad 'CampOtros' no está configurada en el DbContext.");
            }
            // Selecciona explícitamente los campos para evitar devolver el campo 'Pass'.
            // Considera paginación y filtrado.
            return await _context.CampOtros
                .Select(o => new {
                    o.Id,
                    o.Rterc,
                    o.Eterc,
                    o.Clientesap,
                    // No incluir o.Pass
                    o.Nombres,
                    o.Email
                })
                .ToListAsync();
        }

        // GET: api/CampOtro/{id}
        // Busca un registro "Otro" específico por su clave primaria Id (int). EXCLUYE EL CAMPO 'Pass'.
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetCampOtro(int id)
        {
            if (_context.CampOtros == null)
            {
                 return NotFound("La entidad 'CampOtros' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria y selecciona campos seguros.
            var campOtro = await _context.CampOtros
                .Where(o => o.Id == id)
                .Select(o => new {
                    o.Id,
                    o.Rterc,
                    o.Eterc,
                    o.Clientesap,
                    // No incluir o.Pass
                    o.Nombres,
                    o.Email
                })
                .FirstOrDefaultAsync();


            if (campOtro == null)
            {
                return NotFound();
            }

            return campOtro;
        }

        // PUT: api/CampOtro/{id}
        // Para actualizar un registro "Otro" existente.
        // ADVERTENCIA DE SEGURIDAD: Este método, tal como está, actualizaría el campo 'Pass'
        // con el valor que venga en el request. Si 'Pass' es una contraseña, NUNCA debe
        // manejarse de esta forma. Se necesita hashing seguro y probablemente un endpoint/DTO separado.
        // Se previene la modificación de la PK (Id).
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCampOtro(int id, CampOtro campOtro)
        {
             // **** ADVERTENCIA DE SEGURIDAD ****
             // El modelo CampOtro contiene un campo 'Pass'. Si esto representa una contraseña,
             // NUNCA se debe actualizar de esta manera directa. El valor recibido debe ser
             // hasheado de forma segura ANTES de guardarlo. Este código generado NO lo hace.
             // Considera eliminar el campo 'Pass' de este endpoint o manejarlo de forma segura.
             // **********************************

            if (id != campOtro.Id)
            {
                return BadRequest("El ID de la ruta no coincide con el ID del registro.");
            }

             if (_context.CampOtros == null)
            {
                 return NotFound("La entidad 'CampOtros' no está configurada en el DbContext.");
            }

            // Solo permite modificar campos que no son parte de la clave primaria (Id)
            _context.Entry(campOtro).State = EntityState.Modified;
            _context.Entry(campOtro).Property(x => x.Id).IsModified = false; // Previene modificación PK

            // Considera también prevenir la modificación directa de 'Pass' aquí si no se maneja con hashing:
            // _context.Entry(campOtro).Property(x => x.Pass).IsModified = false;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CampOtroExists(id))
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
                 return BadRequest($"No se puede modificar la clave primaria (Id). Error: {ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // POST: api/CampOtro
        // Para crear un nuevo registro "Otro".
        // ADVERTENCIA DE SEGURIDAD: Este método, tal como está, guardaría el campo 'Pass'
        // con el valor que venga en el request (probablemente texto plano). Si 'Pass' es
        // una contraseña, NUNCA debe almacenarse así. Debe ser hasheada de forma segura ANTES de guardarla.
        [HttpPost]
        public async Task<ActionResult<CampOtro>> PostCampOtro(CampOtro campOtro)
        {
             // **** ADVERTENCIA DE SEGURIDAD ****
             // El modelo CampOtro contiene un campo 'Pass'. Si esto representa una contraseña,
             // NUNCA se debe guardar directamente el valor del request. El valor recibido debe ser
             // hasheado de forma segura ANTES de llamar a Add() y SaveChangesAsync().
             // Este código generado NO realiza el hashing. Almacenar contraseñas en texto plano
             // o de forma insegura es un riesgo de seguridad grave.
             // **********************************

             if (_context.CampOtros == null)
            {
                 return Problem("La entidad 'CampOtros' no está configurada en el DbContext.");
            }

             // Asegura que Id no se envíe o sea 0 si es autogenerado por la BD
             // campOtro.Id = 0; // Descomentar si Id es Identity y quieres asegurarte

            _context.CampOtros.Add(campOtro);
            try
            {
                 // El Id probablemente será generado por la base de datos al guardar.
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                 // Manejar otros posibles errores de base de datos
                 return Problem($"Error al guardar el registro 'Otro': {ex.InnerException?.Message ?? ex.Message}");
            }

            // Devuelve una respuesta 201 Created.
            // IMPORTANTE: El objeto devuelto aquí contendría el campo 'Pass' tal como se guardó (inseguro).
            // Sería mejor devolver el resultado del GET (que excluye 'Pass') o un DTO sin 'Pass'.
            // Por simplicidad del generador, se usa CreatedAtAction estándar.
            return CreatedAtAction(nameof(GetCampOtro), new { id = campOtro.Id }, campOtro);
        }

        // DELETE: api/CampOtro/{id}
        // Elimina un registro "Otro".
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampOtro(int id)
        {
             if (_context.CampOtros == null)
            {
                 return NotFound("La entidad 'CampOtros' no está configurada en el DbContext.");
            }

            var campOtro = await _context.CampOtros.FindAsync(id);
            if (campOtro == null)
            {
                return NotFound();
            }

            _context.CampOtros.Remove(campOtro);
            await _context.SaveChangesAsync();

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su ID
        private bool CampOtroExists(int id)
        {
            return (_context.CampOtros?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}