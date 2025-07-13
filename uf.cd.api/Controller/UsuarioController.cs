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
    public class UsuarioController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ExtranetContext context, IConfiguration configuration, ILogger<UsuarioController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/Usuario
        // Obtiene METADATOS de todos los usuarios. EXCLUYE EL CAMPO 'UsuaPwd'.
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUsuarios()
        {
            if (_context.Usuarios == null)
            {
                return NotFound("La entidad 'Usuarios' no está configurada en el DbContext.");
            }
            // Selecciona explícitamente los campos para evitar devolver UsuaPwd.
            // Considera paginación.
            return await _context.Usuarios
                .Select(u => new {
                    u.UsuaNombre,
                    u.UsuaNota,
                    u.UsuaHabilitado,
                    u.UsuaFchAlta,
                    u.UsuaUsrAlta,
                    u.UsuaFchModi,
                    u.UsuaUsrModi,
                    u.UsuaFilasPag,
                    u.UsuaClieId,
                    u.UsuaPwd2,
                    u.UsuaSha1
                    // NO incluir u.UsuaPwd
                })
                .OrderBy(u => u.UsuaNombre)
                .ToListAsync();
        }

        // GET: api/Usuario/{nombre}
        // Busca METADATOS de un usuario específico por su clave primaria UsuaNombre (string). EXCLUYE 'UsuaPwd'.
        [HttpGet("{nombre}")]
        public async Task<ActionResult<object>> GetUsuario(string nombre)
        {
            if (_context.Usuarios == null)
            {
                 return NotFound("La entidad 'Usuarios' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria y selecciona campos seguros.
            var usuario = await _context.Usuarios
                .Where(u => u.UsuaNombre == nombre)
                .Select(u => new {
                    u.UsuaNombre,
                    u.UsuaNota,
                    u.UsuaHabilitado,
                    u.UsuaFchAlta,
                    u.UsuaUsrAlta,
                    u.UsuaFchModi,
                    u.UsuaUsrModi,
                    u.UsuaFilasPag,
                    u.UsuaClieId,
                    u.UsuaPwd2,
                    u.UsuaSha1
                    // NO incluir u.UsuaPwd
                })
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // --- POST (Crear Usuario) OMITIDO ---
        // ADVERTENCIA DE SEGURIDAD: Crear usuarios requiere un manejo SEGURO de contraseñas (hashing).
        // El modelo 'Usuario' contiene un campo 'UsuaPwd' que NUNCA debe almacenarse en texto plano.
        // Un endpoint POST estándar generado automáticamente sería INSEGURO.
        // Se necesita lógica personalizada para:
        // 1. Recibir la contraseña deseada (idealmente en un DTO separado).
        // 2. Hashear la contraseña usando un algoritmo fuerte (Argon2, bcrypt, PBKDF2).
        // 3. Guardar el HASH resultante en el campo 'UsuaPwd'.
        // 4. Validar la fortaleza de la contraseña, unicidad del nombre de usuario, etc.
        // public async Task<ActionResult<Usuario>> PostUsuario(UsuarioCreateDto usuarioDto) { ... }


        // --- PUT (Actualizar Usuario) OMITIDO ---
        // ADVERTENCIA DE SEGURIDAD: Actualizar usuarios, especialmente contraseñas, también requiere
        // un manejo SEGURO. Un endpoint PUT estándar sería INSEGURO para 'UsuaPwd'.
        // Se necesita lógica personalizada para:
        // 1. Actualizar campos que no sean la contraseña (como UsuaNota, UsuaHabilitado).
        // 2. Proveer un endpoint SEPARADO y SEGURO para el cambio de contraseña que incluya:
        //    - Verificación de la contraseña actual (opcional pero recomendado).
        //    - Hashing de la nueva contraseña.
        //    - Actualización del hash almacenado.
        // 3. Prevenir la modificación de la PK (UsuaNombre) y campos de auditoría de alta.
        // public async Task<IActionResult> PutUsuarioMetadata(string nombre, UsuarioMetadataUpdateDto metadataDto) { ... }
        // public async Task<IActionResult> ChangePassword(string nombre, ChangePasswordDto passwordDto) { ... }


        // DELETE: api/Usuario/{nombre}
        // Elimina un usuario. Considera Soft Delete (marcar UsuaHabilitado = "N") en lugar de borrar físicamente.
        [HttpDelete("{nombre}")]
        public async Task<IActionResult> DeleteUsuario(string nombre)
        {
             if (_context.Usuarios == null)
            {
                 return NotFound("La entidad 'Usuarios' no está configurada en el DbContext.");
            }

            var usuario = await _context.Usuarios.FindAsync(nombre);
            if (usuario == null)
            {
                return NotFound();
            }

            // Alternativa Soft Delete:
            // usuario.UsuaHabilitado = "N"; // O el valor que indique inactivo
            // usuario.UsuaFchModi = DateTime.UtcNow;
            // // usuario.UsuaUsrModi = User.Identity?.Name;
            // _context.Entry(usuario).State = EntityState.Modified;

            _context.Usuarios.Remove(usuario); // Eliminación física (menos común para usuarios)

            try
            {
                 await _context.SaveChangesAsync();
            }
             catch (DbUpdateException ex)
            {
                  // Capturar errores de FK si el usuario está referenciado en otras tablas
                   return Problem($"No se pudo eliminar el usuario. Es posible que tenga registros asociados. Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un usuario existe por su nombre (PK)
        private bool UsuarioExists(string nombre)
        {
             // Considerar comparación insensible a mayúsculas/minúsculas si los nombres de usuario lo son
            return (_context.Usuarios?.Any(e => e.UsuaNombre == nombre)).GetValueOrDefault();
        }
    }
}