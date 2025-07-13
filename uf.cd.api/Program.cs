using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using uf.cd.db.Model;
using uf.cd.api.Models; // Namespace para DTO LoginModel
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors; // Incluir namespace para CORS
using System.Linq; // Necesario para Linq, usado para filtrar/procesar orígenes


// Configure Serilog for bootstrap logging (Optional - keep if using Serilog)
Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("Starting web application: uf.cd.api");

    var builder = WebApplication.CreateBuilder(args);

    // --- Configure Serilog (Optional - keep if using Serilog) ---
    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((context, services, configuration) => configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services)
        .Enrich.FromLogContext());
    // --- End Serilog Configuration ---

    // NUEVA SECCIÓN: Leer configuración de CORS desde appsettings.json
    var corsSettings = builder.Configuration.GetSection("CorsPolicy").Get<CorsPolicySettings>();

    if (corsSettings == null)
    {
        // Log a warning or error if CORS settings are missing
        Log.Warning("Sección 'CorsPolicy' no encontrada en appsettings.json. La política de CORS podría no aplicarse correctamente.");
        // O lanzar una excepción si la configuración de CORS es crítica
        // throw new ApplicationException("La configuración de CORS ('CorsPolicy') es obligatoria.");
    }

    // Configuración de CORS usando los valores de appsettings.json
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("ConfiguredCorsPolicy", // Nombre de la política, puedes cambiarlo si quieres
            policyBuilder =>
            {
                // Configurar orígenes
                if (corsSettings?.AllowedOrigins?.Length > 0)
                {
                    // Separamos orígenes específicos y orígenes con wildcard IP (como 192.168.100.*)
                    var specificOrigins = corsSettings.AllowedOrigins.Where(o => !o.EndsWith(".*")).ToArray();
                    var wildcardOrigins = corsSettings.AllowedOrigins.Where(o => o.EndsWith(".*")).ToArray();

                    if (specificOrigins.Length > 0 || wildcardOrigins.Length > 0)
                    {
                         // Usamos SetIsOriginAllowed para manejar tanto orígenes específicos como wildcards IP
                         policyBuilder.SetIsOriginAllowed(origin =>
                         {
                              // Comprobar si el origen coincide con algún origen específico
                              if (specificOrigins.Any(so => origin.Equals(so, StringComparison.OrdinalIgnoreCase)))
                              {
                                  return true;
                              }

                              // Comprobar si el origen coincide con algún origen con wildcard IP
                              if (wildcardOrigins.Any(wo => origin.StartsWith(wo.Substring(0, wo.Length - 2), StringComparison.OrdinalIgnoreCase)))
                              {
                                  // Puedes añadir lógica adicional aquí para asegurar que el resto del origen
                                  // es válido (ej. un número de IP).
                                  // Ej: return Uri.TryCreate(origin, UriKind.Absolute, out Uri uri) && uri.Host.StartsWith(wo.Substring(wo.Length -2), StringComparison.OrdinalIgnoreCase);
                                  return true;
                              }

                              return true; // El origen no está permitido por esta política
                         });
                    }
                    else
                    {
                         // Si AllowedOrigins está vacío o solo contiene strings vacíos/null,
                         // puedes decidir si no permitir ningún origen o permitir cualquiera.
                         // Si no se especifica nada, por defecto no se permite ninguno.
                         Log.Warning("La lista 'AllowedOrigins' en 'CorsPolicy' está vacía. No se permitirán orígenes.");
                    }
                }
                else
                {
                    // Si la propiedad AllowedOrigins es null o el array es vacío
                     Log.Warning("'AllowedOrigins' no está configurado en 'CorsPolicy'. La política de CORS no permitirá orígenes específicos.");
                     // Decide si en este caso quieres AllowAnyOrigin() (menos seguro)
                     // policyBuilder.AllowAnyOrigin(); // Descomentar si quieres permitir cualquier origen por defecto
                }

                // Configurar métodos
                if (corsSettings?.AllowedMethods?.Length > 0)
                {
                    policyBuilder.WithMethods(corsSettings.AllowedMethods);
                }
                else
                {
                    Log.Warning("'AllowedMethods' no está configurado en 'CorsPolicy' o está vacío. Se permitirán todos los métodos por defecto (AllowAnyMethod).");
                    policyBuilder.AllowAnyMethod(); // Por defecto, permite cualquier método si no se especifican
                }

                // Configurar cabeceras
                if (corsSettings?.AllowedHeaders?.Length > 0)
                {
                     policyBuilder.WithHeaders(corsSettings.AllowedHeaders);
                }
                else
                {
                     Log.Warning("'AllowedHeaders' no está configurado en 'CorsPolicy' o está vacío. Se permitirán todas las cabeceras por defecto (AllowAnyHeader).");
                     policyBuilder.AllowAnyHeader(); // Por defecto, permite cualquier cabecera si no se especifican
                }

                // Configurar credenciales (si lo añadiste a CorsPolicySettings y appsettings.json)
                // if (corsSettings?.AllowCredentials == true)
                // {
                //     policyBuilder.AllowCredentials();
                //     // Nota: AllowCredentials es incompatible con AllowAnyOrigin().
                //     // Si usas AllowCredentials, debes usar SetIsOriginAllowed o WithOrigins con orígenes específicos.
                // }
            });
    });
    // --- Fin Configuración de CORS ---


    // 1. Configuración del DbContext 
    // ... (mantén la configuración de DbContext, JWT, Authorization, etc.)
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    var serverVersion = new MySqlServerVersion(new Version(5, 1, 0)); // Ajusta según tu versión de MySQL

    builder.Services.AddDbContext<ExtranetContext>(options =>
        options.UseMySql(connectionString, serverVersion,options => options.UseMicrosoftJson())
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
    );

    // 2. Configuración de Autenticación JWT
    var issuer = builder.Configuration["AppSettings:Issuer"];
    var audience = builder.Configuration["AppSettings:Audience"];
    var secretKey = builder.Configuration["AppSettings:SecretKey"];

    if (string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(secretKey))
    {
        var startupLogger = Log.Logger ?? new LoggerConfiguration().WriteTo.Console().CreateLogger();
        startupLogger.Fatal("JWT Issuer, Audience, or SecretKey is missing in configuration.");
        throw new ArgumentNullException("JWT configuration (Issuer, Audience, SecretKey) must be set in AppSettings.");
    }

    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = securityKey,
        };
        options.Events = new JwtBearerEvents
        {
            OnChallenge = context => { context.HandleResponse(); context.Response.StatusCode = StatusCodes.Status401Unauthorized; context.Response.ContentType = "application/json"; return Task.CompletedTask; },
            OnForbidden = context => { context.Response.StatusCode = StatusCodes.Status403Forbidden; context.Response.ContentType = "application/json"; return Task.CompletedTask; }
        };
    });

    // 3. Configuración de Autorización
    builder.Services.AddAuthorization(options =>
    {
        options.FallbackPolicy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
    });

    // 4. Registrar Controllers
    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();

    // --- Configuración de Swagger/OpenAPI ---
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        // API Info
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1.0.0",
            Title = "uf.cd.api",
            Description = "API para gestión académica y administrativa del campus (con JWT & Basic Auth)",
            // Contact = new OpenApiContact { Name = "Equipo API" },
            // License = new OpenApiLicense { Name = "Licencia" }
        });

        // --- Define Security Scheme for JWT Bearer ---
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = @"Autenticación JWT usando el esquema Bearer.
                          Ingrese 'Bearer' [espacio] y luego su token en la entrada de texto a continuación.
                          Ejemplo: 'Bearer 12345abcdef'",
            Name = "Authorization", // Standard header name
            In = ParameterLocation.Header, // Where the token is sent
            Type = SecuritySchemeType.Http, // Type of scheme
            Scheme = "bearer", // Scheme name (lowercase)
            BearerFormat = "JWT" // Format hint
        });

        // --- Define Security Scheme for Basic Authentication ---
        options.AddSecurityDefinition("basicAuth", new OpenApiSecurityScheme
        {
            Description = "Autenticación Básica (Usuario/Contraseña).",
            Name = "Authorization", // Standard header name
            In = ParameterLocation.Header, // Where the credentials are sent
            Type = SecuritySchemeType.Http, // Type of scheme
            Scheme = "basic" // Scheme name (lowercase)
        });

        // --- Apply Security Requirements ---
        // This tells Swagger UI which security schemes can be used.
        // CORREGIDO: Usar OpenApiSecurityRequirement en lugar de OpenApiRequirement
        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            // Requirement for Bearer (JWT)
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer" // Must match the definition name above
                    },
                    Scheme = "oauth2",
                    Name = "Bearer",
                    In = ParameterLocation.Header,
                },
                new List<string>() // No specific scopes required here
            }
        });

         // CORREGIDO: Usar OpenApiSecurityRequirement en lugar de OpenApiRequirement para Basic Auth
         options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            // Requirement for Basic Auth
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "basicAuth" // Must match the definition name above
                    },
                    Scheme = "basic",
                    Name = "basicAuth",
                    In = ParameterLocation.Header,
                },
                new List<string>()
            }
        });


        // Optional: Use XML comments for descriptions
        // var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    });
    // --- Fin Configuración Swagger ---


    var app = builder.Build();

    // Configure Serilog Request Logging (Optional - keep if using Serilog)
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "uf.cd.api v1"); });
        app.UseDeveloperExceptionPage();
    }
    else if (app.Environment.IsProduction())
    {
         app.UseSwagger(); // Consider removing or securing Swagger in production
         app.UseSwaggerUI(options => { options.SwaggerEndpoint("/swagger/v1/swagger.json", "uf.cd.api v1"); });
         //app.UseExceptionHandler("/Error"); // Handle errors in production
         //app.UseHsts(); // Use HTTP Strict Transport Security
    }

    // USO DEL MIDDLEWARE DE CORS CON LA POLÍTICA CONFIGURADA
    // ¡IMPORTANTE! Este middleware debe ir ANTES de UseRouting, UseAuthentication y UseAuthorization
    // Asegúrate de usar el mismo nombre de política que definiste en AddCors
    app.UseCors("ConfiguredCorsPolicy"); // Usa la política nombrada

    //app.UseHttpsRedirection();

    app.UseRouting(); // UseRouting generalmente va DESPUÉS de UseCors

    // Habilitar Middleware de Autenticación y Autorización (¡ORDEN IMPORTANTE!)
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly: uf.cd.api");
}
finally
{
    Log.CloseAndFlush();
}