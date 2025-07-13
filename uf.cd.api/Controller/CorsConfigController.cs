using Microsoft.AspNetCore.Cors.Infrastructure; // Para CorsOptions y CorsPolicy
using Microsoft.Extensions.Options; // Para IOptions<>
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


[ApiController]
[Route("api/[controller]")] // Ruta base para este controlador, ej: /api/CorsConfig
public class CorsConfigController : ControllerBase
{
    private readonly CorsOptions _corsOptions;
    private readonly ILogger<CorsConfigController> _logger;

    public CorsConfigController(IOptions<CorsOptions> corsOptions, ILogger<CorsConfigController> logger)
    {
        _corsOptions = corsOptions.Value;
        _logger = logger;
    }

    // Endpoint GET para obtener las políticas de CORS relevantes (la configurada y la por defecto si es diferente)
    // NOTA DE SEGURIDAD: Este endpoint expone información interna.
    // En producción, DEBES restringir el acceso a este endpoint (ej. con [Authorize]
    // para solo administradores, o eliminarlo). Se marca [AllowAnonymous] aquí
    // para que puedas probarlo fácilmente, pero TEN CUIDADO.
    [HttpGet("policies")]
    [AllowAnonymous] // Permite acceso sin autenticación - ¡ÚSALO CON PRECAUCIÓN!
    public ActionResult<IEnumerable<CorsPolicyInfo>> GetCorsPolicies()
    {
        var policiesInfoList = new List<CorsPolicyInfo>();

        // --- Obtener la política nombrada que creamos ---
        var configuredPolicyName = "ConfiguredCorsPolicy"; // Usa el nombre exacto que usaste en Program.cs
        var configuredPolicy = _corsOptions.GetPolicy(configuredPolicyName);

        if (configuredPolicy != null)
        {
            policiesInfoList.Add(MapPolicyToInfo(configuredPolicy, configuredPolicyName));
            _logger.LogInformation($"Política CORS '{configuredPolicyName}' encontrada y añadida a la lista.");
        }
        else
        {
             _logger.LogWarning($"Política CORS nombrada '{configuredPolicyName}' no encontrada en las opciones configuradas.");
        }

        // --- Comprobar si hay un nombre de política por defecto configurado ---
        var defaultPolicyName = _corsOptions.DefaultPolicyName;
        // Si hay un nombre de política por defecto Y NO es el mismo nombre que nuestra política configurada
        if (!string.IsNullOrEmpty(defaultPolicyName) && defaultPolicyName != configuredPolicyName)
        {
             // Intentar obtener la política por defecto por su nombre
             var defaultPolicy = _corsOptions.GetPolicy(defaultPolicyName);
             if(defaultPolicy != null)
             {
                 // Añadir la política por defecto a la lista, indicando que es la por defecto
                 policiesInfoList.Add(MapPolicyToInfo(defaultPolicy, $"DefaultPolicy ({defaultPolicyName})"));
                 _logger.LogInformation($"Política CORS por defecto '{defaultPolicyName}' encontrada y añadida.");
             }
             else
             {
                  _logger.LogWarning($"Política CORS por defecto con nombre '{defaultPolicyName}' configurado pero la política no fue encontrada.");
             }
        } else if (!string.IsNullOrEmpty(defaultPolicyName) && defaultPolicyName == configuredPolicyName)
        {
             _logger.LogInformation($"La política '{configuredPolicyName}' está configurada como la política por defecto.");
             // No es necesario añadirla de nuevo a la lista, ya la añadimos antes
        }
        else
        {
             _logger.LogInformation("No hay nombre de política CORS por defecto configurado.");
        }


        if (!policiesInfoList.Any())
        {
             _logger.LogWarning("No se encontraron políticas CORS relevantes para mostrar (ni la nombrada 'ConfiguredCorsPolicy' ni la política por defecto si está configurada).");
             return NotFound("No se encontraron políticas CORS configuradas relevantes en la aplicación.");
        }

        return Ok(policiesInfoList);
    }

    // Método auxiliar para mapear CorsPolicy a CorsPolicyInfo
    private CorsPolicyInfo MapPolicyToInfo(CorsPolicy policy, string policyName)
    {
       // Nota: Las propiedades policy.Origins, policy.Methods, policy.Headers
       // solo se llenan si se usaron WithOrigins, WithMethods, WithHeaders respectivamente.
       // No reflejan la lógica de SetIsOriginAllowed o AllowAny.
        return new CorsPolicyInfo
        {
            Name = policyName,
            AllowAnyOrigin = policy.AllowAnyOrigin,
            // Si AllowAnyOrigin es true o SetIsOriginAllowed se usó, Origins es irrelevante o nulo/vacío
            AllowedSpecificOrigins = policy.AllowAnyOrigin || policy.IsOriginAllowed != null ? null : policy.Origins?.ToList(), // Usar ToList() para evitar problemas de serialización si es necesario
            IsOriginAllowedCustomFunction = policy.IsOriginAllowed != null, // True si SetIsOriginAllowed se usó
            AllowAnyMethod = policy.AllowAnyMethod,
            AllowedSpecificMethods = policy.AllowAnyMethod ? null : policy.Methods?.ToList(),
            AllowAnyHeader = policy.AllowAnyHeader,
            AllowedSpecificHeaders = policy.AllowAnyHeader ? null : policy.Headers?.ToList(),
            SupportsCredentials = policy.SupportsCredentials
        };
    }

     // Opcional: Endpoint GET para obtener una política específica por nombre
    [HttpGet("policies/{policyName}")]
    [AllowAnonymous] // ¡ÚSALO CON PRECAUCIÓN!
     public ActionResult<CorsPolicyInfo> GetCorsPolicyByName(string policyName)
     {
         var policy = _corsOptions.GetPolicy(policyName);

         if (policy == null)
         {
             _logger.LogWarning($"Política CORS '{policyName}' no encontrada.");
             return NotFound($"Política CORS '{policyName}' no encontrada.");
         }

         return Ok(MapPolicyToInfo(policy, policyName));
     }
}