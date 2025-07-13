using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Threading.Tasks;
using Newtonsoft.Json;
using uf.cd.api.Common;

namespace uf.cd.api.Controller
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class CarteleraDigitalController : ControllerBase
    {
        private readonly string _connectionString;

        public CarteleraDigitalController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("UniversitasConnection");
        }

        [HttpGet("ficen")]
        public async Task<IActionResult> GetReporteCarteleraFicen(string fechaInicio, string fechaFin, int idCartelera, int estimado)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("Universitas.[Universitas].[dbo].[SP_REPORTE_CARTELERA_FICEN]", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@FECHA_DESDE", fechaInicio);
            command.Parameters.AddWithValue("@FECHA_HASTA", fechaFin);
            command.Parameters.AddWithValue("@CARTELERA", idCartelera);
            command.Parameters.AddWithValue("@ESTIMADOS", estimado);

            var dataTable = new DataTable();
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            dataTable.Load(reader);

            string json = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
            return Ok(json);
            //return Ok(dataTable);
        }

        [HttpGet]
        public async Task<IActionResult> GetReporteCartelera(string fechaIni, string fechaFin, int idSCartelera, int idCarrera, int estimado)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("Universitas.UNIVERSITAS.dbo.SP_REPORTE_CARTELERA", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@FECHA_DESDE", fechaIni);
            command.Parameters.AddWithValue("@FECHA_HASTA", fechaFin);
            command.Parameters.AddWithValue("@CARTELERA", idSCartelera);
            command.Parameters.AddWithValue("@ESTIMADOS", estimado);
            command.Parameters.AddWithValue("@ID_CARRERA", idCarrera);

            var dataTable = new DataTable();
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            dataTable.Load(reader);

            string json = JsonConvert.SerializeObject(dataTable, Formatting.Indented);
            return Ok(json);
            //return Ok(dataTable);
        }

        [HttpGet]
        public async Task<IActionResult> GetReporteCarteleraDigital(string fechaInicio = "", string fechaFin = "", int idCartelera = 1, int? idCarrera = null, int estimado = 0, int pageNumber = 1, int pageSize = 10)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand("Universitas.UNIVERSITAS.dbo.SP_REPORTE_CARTELERA_Digital", connection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@FECHA_DESDE", fechaInicio);
            command.Parameters.AddWithValue("@FECHA_HASTA", fechaFin);
            command.Parameters.AddWithValue("@CARTELERA", idCartelera);
            command.Parameters.AddWithValue("@ESTIMADOS", estimado);
            command.Parameters.AddWithValue("@ID_CARRERA", idCarrera);

            var dataTable = new DataTable();
            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();
            dataTable.Load(reader);

            dataTable.ConvertColumnNamesToLowerCase();
            string json = JsonConvert.SerializeObject(dataTable.Paginate(pageNumber, pageSize), Formatting.Indented);
            return Ok(json);
        }
    }
}