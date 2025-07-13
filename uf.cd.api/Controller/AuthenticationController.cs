using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using uf.cd.db.Model; // Asumiendo namespace del modelo Usuario
using uf.cd.api.Models; // Namespace para DTO LoginModel
using Microsoft.Extensions.Configuration; // Para leer appsettings.json
using Microsoft.IdentityModel.Tokens; // Para SymmetricSecurityKey
using System.IdentityModel.Tokens.Jwt; // Para JwtSecurityTokenHandler
using System.Text; // Para Encoding
using System; // Para DateTime/TimeSpan
using System.Net.Http.Headers; // Para AuthenticationHeaderValue
using Serilog; // If using Serilog

namespace uf.cd.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(ExtranetContext context, IConfiguration configuration, ILogger<AuthenticationController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost("login")]
        [AllowAnonymous] // Permite acceso anónimo a este endpoint específico
        public async Task<IActionResult> Login([FromBody] LoginModel? model) // Model es ahora nullable
        {
            string? username = null;
            string? password = null;

            // --- Estrategia 1: Intentar obtener credenciales del Header (Basic Auth) ---
            try
            {
                if (Request.Headers.ContainsKey("Authorization"))
                {
                    var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]!);
                    if (authHeader.Scheme.Equals("Basic", StringComparison.OrdinalIgnoreCase) &&
                        authHeader.Parameter != null)
                    {
                        var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                        if (credentials.Length == 2)
                        {
                            username = credentials[0];
                            password = credentials[1];
                            Log.Information("Attempting login via Basic Auth header for user {Username}", username); // Ejemplo Serilog
                        }
                    }
                }
            }
            catch (FormatException ex)
            {
                Log.Warning(ex, "Error parsing Basic Auth header.");
                return BadRequest(new { message = "Formato de encabezado de autenticación básica inválido." });
            }
            catch (Exception ex) // Captura general por si acaso
            {
                 Log.Error(ex, "Unexpected error reading Basic Auth header.");
                 return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error procesando encabezado de autenticación."});
            }

            // --- Estrategia 2: Si no hay Basic Auth, intentar obtener del Body ---
            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                if (model == null || !ModelState.IsValid)
                {
                    // Si no hay Basic Auth Y el modelo es inválido o nulo
                    return BadRequest(new { message = "Credenciales no proporcionadas en el encabezado Basic Auth ni en el cuerpo de la solicitud." });
                }
                username = model.Username;
                password = model.Password;
                 Log.Information("Attempting login via Request Body for user {Username}", username); // Ejemplo Serilog
            }

            // --- Validación de existencia de credenciales ---
             if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                 // Este caso no debería ocurrir si la lógica anterior es correcta, pero por si acaso.
                 return BadRequest(new { message = "Nombre de usuario o contraseña no pueden estar vacíos." });
            }

            // --- Lógica de Autenticación y Generación JWT ---
            if (_context.Usuarios == null)
            {
                 return Problem("La configuración de usuarios no está disponible.");
            }

            // 1. Buscar usuario por nombre
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; // Mejora rendimiento
            //_context.Database.SetCommandTimeout(TimeSpan.FromSeconds(30)); // Aumentar timeout si es necesario
            try
            {
                var connectionString = _context.Database.GetConnectionString();
                Log.Information("Using connection string: {ConnectionString}", connectionString);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to retrieve connection string from DbContext.");
            }
            var user = await _context.Usuarios.Where(u => u.UsuaNombre == username).FirstOrDefaultAsync();

            // 2. Verificar si existe y está habilitado
            if (user == null || user.UsuaHabilitado?.ToUpper() != "S") // Asumiendo "S" para habilitado
            {
                Log.Warning("Login failed for user {Username}: User not found or not enabled.", username);
                await Task.Delay(1000); // Pequeña demora
                return Unauthorized(new { message = "Credenciales inválidas." });
            }
            // 2.1 Verificar si la tiene clave generada para la app Credencial Digital
            if (string.IsNullOrEmpty(user.UsuaSha1))
            {
                PasswordEncryptor passwordEncryptor = new PasswordEncryptor(_configuration["AppSettings:SecretKey"], _configuration["AppSettings:IVKey"]);
                string hashedPassword = passwordEncryptor.GetHashPassword(password);
                user.UsuaSha1 = hashedPassword; // Actualizar el hash de la contraseña
                user.UsuaPwd2 = password; // Actualizar el campo UsuaPwd también
                _context.Entry(user).State = EntityState.Modified; // Marcar como modificado
                try
                {
                    await _context.SaveChangesAsync();
                    Log.Information("User {Username} password hash updated successfully.", username);  
                }
                catch (DbUpdateException ex)
                {
                    Log.Error(ex, "Error updating password hash for user {Username}.", username);
                    return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error al actualizar el hash de la contraseña." });
                }
            }
            // 3. **** ¡¡¡VERIFICACIÓN DE CONTRASEÑA SEGURA (REQUIERE IMPLEMENTACIÓN)!!! ****
            bool isPasswordValid = VerifyPasswordHash(password, user.UsuaPwd); // <- Necesitas implementar esta función!

            if (!isPasswordValid)
            {
                Log.Warning("Login failed for user {Username}: Invalid password.", username);
                await Task.Delay(1000); // Pequeña demora
                return Unauthorized(new { message = "Credenciales inválidas." });
            }

            // 4. Generar Token JWT si la contraseña es válida
            var token = GenerateJwtToken(user);

            Log.Information("User {Username} logged in successfully and JWT generated.", username);
            return Ok(new
            {
                message = "Autenticación exitosa.",
                username = user.UsuaNombre,
                token = token.tokenString,
                expires = token.validTo
                // Puedes añadir otros datos del usuario aquí (ej: roles, nombre completo), pero NUNCA la contraseña/hash
            });
        }

        [HttpPost("logout")]
        // [Authorize] // Opcional: Podrías requerir estar autenticado (con JWT) para llamar a logout
        public IActionResult Logout()
        {
            // Para JWT/Basic Auth, el logout es principalmente una acción del lado del cliente (descartar el token/credenciales).
            // No hay una cookie de sesión que eliminar en el servidor como con CookieAuthentication.
            var username = User.Identity?.Name ?? "Usuario desconocido"; // Obtiene el nombre del token si está presente
            Log.Information("Logout requested for user {Username}. Client should discard token/credentials.", username);

            // Simplemente devolvemos OK para indicar que la señal de logout fue recibida.
            return Ok(new { message = "Señal de Logout recibida. El cliente debe descartar el token/credenciales." });
        }

        // Endpoint para verificar el estado de autenticación (basado en JWT)
        [HttpGet("status")]
        [Authorize] // Requiere un JWT válido para acceder
        public IActionResult GetStatus()
        {
            // Si llega aquí, está autenticado con JWT debido a [Authorize] y la config en Program.cs
            return Ok(new
                {
                    isAuthenticated = true,
                    username = User.Identity?.Name,
                    // Puedes extraer y devolver otros claims del token si es necesario
                    // userIdClaim = User.FindFirst("UserId")?.Value
                });
        }

        // Endpoint para devolver 401 (Usado por la config de Auth en Program.cs)
        [HttpGet("Unauthorized")]
        [HttpPost("Unauthorized")]
        [HttpPut("Unauthorized")]
        [HttpDelete("Unauthorized")]
        [AllowAnonymous]
        public IActionResult UnauthorizedResult()
        {
             // Log.Debug("Returning 401 Unauthorized from dedicated endpoint.");
            return Unauthorized(new { message = "Acceso no autorizado. Se requiere iniciar sesión o token válido." });
        }

         // Endpoint para devolver 403 (Usado por la config de Auth en Program.cs)
        [HttpGet("Forbidden")]
        [HttpPost("Forbidden")]
        [HttpPut("Forbidden")]
        [HttpDelete("Forbidden")]
        [AllowAnonymous] // Aunque el acceso está denegado, el endpoint en sí no requiere auth para devolver 403
        public IActionResult ForbiddenResult()
        {
             // Log.Debug("Returning 403 Forbidden from dedicated endpoint.");
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Permisos insuficientes para realizar esta acción." });
        }


        // --- MÉTODO DE EJEMPLO - ¡¡¡NECESITAS IMPLEMENTARLO CORRECTAMENTE!!! ---
        private bool VerifyPasswordHash(string providedPassword, string storedHash)
        {
            // **** ¡¡¡IMPLEMENTACIÓN CRÍTICA!!! ****
            // Reemplaza esto con tu lógica de verificación de hash segura (BCrypt, Argon2, etc.)
            // Console.Error.WriteLine("ADVERTENCIA: La verificación de contraseña NO es segura. Implementar hashing.");
             // Log.Warning("Password verification is using INSECURE placeholder logic!");
             
            PasswordEncryptor passwordEncryptor = new PasswordEncryptor(_configuration["AppSettings:SecretKey"], _configuration["AppSettings:IVKey"]);
            string hashedPassword = passwordEncryptor.GetHashPassword(providedPassword);
            // Comparar el hash proporcionado con el almacenado (asegúrate de que ambos estén en el mismo formato)
            // return hashedPassword == storedHash; // Comparar hashes
            if(string.IsNullOrEmpty(storedHash))
            {
                Log.Warning("Stored password hash is null or empty for user {Username}.", providedPassword);
                return false;
            }
            if(hashedPassword == storedHash)
            {
                return true;
            }
            else
            {
                Log.Warning("Password verification failed for user {storedHash}.", storedHash);
                Log.Warning("Password verification failed for user {hashedPassword}.", hashedPassword);
                return false;
            }

             return false; // ¡¡¡CAMBIAR ESTO!!! Devuelve false hasta implementar correctamente.
        }

        // --- MÉTODO PARA GENERAR EL TOKEN JWT ---
        private (string tokenString, DateTime validTo) GenerateJwtToken(Usuario user)
        {
            // Obtener configuración desde appsettings.json
            var issuer = _configuration["AppSettings:Issuer"];
            var audience = _configuration["AppSettings:Audience"];
            var secretKey = _configuration["AppSettings:SecretKey"];

            if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("Configuración JWT (Issuer, Audience, SecretKey) no encontrada o incompleta.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Definir Claims
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UsuaNombre), // Subject (standard claim for username)
                new Claim(ClaimTypes.Name, user.UsuaNombre),             // ASP.NET Core Identity standard name claim
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Unique token identifier
                // Añadir otros claims necesarios (roles, permisos, IDs, etc.)
                // new Claim(ClaimTypes.Role, "Admin"), // Ejemplo
                // new Claim("UserId", user.UsuaNombre) // Ejemplo claim personalizado
            };

            // Definir tiempo de expiración (ej: 1 hora)
            var expires = DateTime.UtcNow.AddHours(1); // Hacer configurable si es necesario

            // Crear el token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expires,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return (tokenHandler.WriteToken(token), token.ValidTo);
        }
    }
}