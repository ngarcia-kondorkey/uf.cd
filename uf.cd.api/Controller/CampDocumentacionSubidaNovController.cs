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
    public class CampDocumentacionSubidaNovController : ControllerBase
    {
        private readonly ExtranetContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<CampDocumentacionSubidaNovController> _logger;

        public CampDocumentacionSubidaNovController(ExtranetContext context, IConfiguration configuration, ILogger<CampDocumentacionSubidaNovController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        // GET: api/CampDocumentacionSubidaNov
        // Obtiene METADATOS de la documentación subida. Excluye el contenido del archivo (CdsuDoc).
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetCampDocumentacionSubidaNovs()
        {
            if (_context.CampDocumentacionSubidaNovs == null)
            {
                return NotFound("La entidad 'CampDocumentacionSubidaNovs' no está configurada en el DbContext.");
            }

            // Selecciona explícitamente los campos para evitar cargar CdsuDoc (byte[])
            return await _context.CampDocumentacionSubidaNovs
                .Select(doc => new {
                    doc.CdsuIdAlumno,
                    doc.CdsuId,
                    doc.CdsuIdTipoadjunto,
                    doc.CdsuFechasubida,
                    doc.CdsuNota,
                    doc.CdsuNombre,
                    doc.CdsuTipo,
                    doc.CdsuFecharecibida
                    // NO incluir doc.CdsuDoc aquí
                })
                .ToListAsync();
        }

        // GET: api/CampDocumentacionSubidaNov/{idAlumno}/{id}/{idTipoadjunto}
        // Obtiene METADATOS de un documento específico. Excluye el contenido del archivo (CdsuDoc).
        // Busca por la clave primaria compuesta asumida.
        [HttpGet("{idAlumno}/{id}/{idTipoadjunto}")]
        public async Task<ActionResult<object>> GetCampDocumentacionSubidaNov(string idAlumno, int id, int idTipoadjunto)
        {
            if (_context.CampDocumentacionSubidaNovs == null)
            {
                 return NotFound("La entidad 'CampDocumentacionSubidaNovs' no está configurada en el DbContext.");
            }

            // Busca por la clave primaria asumida y selecciona los metadatos
            var campDocMetadata = await _context.CampDocumentacionSubidaNovs
                .Where(doc => doc.CdsuIdAlumno == idAlumno && doc.CdsuId == id && doc.CdsuIdTipoadjunto == idTipoadjunto)
                 .Select(doc => new {
                    doc.CdsuIdAlumno,
                    doc.CdsuId,
                    doc.CdsuIdTipoadjunto,
                    doc.CdsuFechasubida,
                    doc.CdsuNota,
                    doc.CdsuNombre,
                    doc.CdsuTipo,
                    doc.CdsuFecharecibida
                    // NO incluir doc.CdsuDoc aquí
                })
                .FirstOrDefaultAsync();


            if (campDocMetadata == null)
            {
                return NotFound();
            }

            return campDocMetadata;
        }

        // GET: api/CampDocumentacionSubidaNov/{idAlumno}/{id}/{idTipoadjunto}/download
        // Endpoint específico para descargar el contenido del archivo.
        [HttpGet("{idAlumno}/{id}/{idTipoadjunto}/download")]
        public async Task<IActionResult> DownloadCampDocumentacionSubidaNov(string idAlumno, int id, int idTipoadjunto)
        {
             if (_context.CampDocumentacionSubidaNovs == null)
            {
                 return NotFound("La entidad 'CampDocumentacionSubidaNovs' no está configurada en el DbContext.");
            }

            // Busca el registro completo incluyendo el contenido del archivo
             var campDoc = await _context.CampDocumentacionSubidaNovs
                .Where(doc => doc.CdsuIdAlumno == idAlumno && doc.CdsuId == id && doc.CdsuIdTipoadjunto == idTipoadjunto)
                .Select(doc => new {
                    doc.CdsuDoc, // Necesitamos el contenido
                    doc.CdsuTipo, // Necesitamos el MIME type
                    doc.CdsuNombre // Necesitamos el nombre del archivo
                })
                .FirstOrDefaultAsync();

            if (campDoc == null || campDoc.CdsuDoc == null)
            {
                return NotFound("Documento no encontrado o sin contenido.");
            }

            // Devuelve el archivo usando FileResult
            return File(campDoc.CdsuDoc, campDoc.CdsuTipo ?? "application/octet-stream", campDoc.CdsuNombre);
        }


        // --- POST (Crear) OMITIDO ---
        // La creación de registros que incluyen subida de archivos (byte[]) requiere
        // un manejo especial en ASP.NET Core, usualmente usando IFormFile y [FromForm].
        // Un endpoint POST estándar que acepta JSON no es adecuado para subir archivos.
        // Se necesitaría un método como:
        // public async Task<ActionResult<CampDocumentacionSubidaNov>> PostCampDocumentacionSubidaNov([FromForm] CampDocumentacionSubidaNovCreateModel model)
        // donde CampDocumentacionSubidaNovCreateModel tendría una propiedad IFormFile para el archivo.


        // --- PUT (Actualizar) OMITIDO / PARCIAL ---
        // Similar al POST, actualizar el contenido del archivo (CdsuDoc) requeriría IFormFile.
        // Se podría implementar un PUT para actualizar *solo* los metadatos (CdsuNota, CdsuFecharecibida),
        // pero actualizar el archivo en sí mismo generalmente se maneja como DELETE + POST nuevo,
        // o con un endpoint específico que acepte IFormFile.


        // DELETE: api/CampDocumentacionSubidaNov/{idAlumno}/{id}/{idTipoadjunto}
        // Elimina el registro de metadatos. No elimina el archivo si está almacenado externamente.
        [HttpDelete("{idAlumno}/{id}/{idTipoadjunto}")]
        public async Task<IActionResult> DeleteCampDocumentacionSubidaNov(string idAlumno, int id, int idTipoadjunto)
        {
             if (_context.CampDocumentacionSubidaNovs == null)
            {
                 return NotFound("La entidad 'CampDocumentacionSubidaNovs' no está configurada en el DbContext.");
            }

            // Busca por clave primaria compuesta asumida
            var campDoc = await _context.CampDocumentacionSubidaNovs.FindAsync(idAlumno, id, idTipoadjunto);
            if (campDoc == null)
            {
                return NotFound();
            }

            _context.CampDocumentacionSubidaNovs.Remove(campDoc);
            await _context.SaveChangesAsync();

            // Nota: Si el archivo CdsuDoc estuviera almacenado en un sistema de archivos
            // o blob storage externo, aquí también deberías añadir la lógica para eliminarlo de allí.

            return NoContent(); // Indica éxito sin contenido que devolver
        }

        // Método privado para verificar si un registro existe por su clave compuesta asumida
        private bool CampDocumentacionSubidaNovExists(string idAlumno, int id, int idTipoadjunto)
        {
            // Asegúrate que CampDocumentacionSubidaNovs no sea null antes de usarlo
            return (_context.CampDocumentacionSubidaNovs?.Any(e =>
                e.CdsuIdAlumno == idAlumno &&
                e.CdsuId == id &&
                e.CdsuIdTipoadjunto == idTipoadjunto)).GetValueOrDefault();
        }
    }
}