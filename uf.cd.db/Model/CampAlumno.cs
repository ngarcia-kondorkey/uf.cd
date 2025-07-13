using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampAlumno
{
    public string CaluIdAlumno { get; set; } = null!;

    public int CaluIdCarrera { get; set; }

    public int CaluLu { get; set; }

    public int? CaluAnioinicio { get; set; }

    public string CaluApellido { get; set; } = null!;

    public string CaluNombre { get; set; } = null!;

    public int CaluIdTipodoc { get; set; }

    public string CaluNrodoc { get; set; } = null!;

    public string CaluIdSexo { get; set; } = null!;

    public int? CaluIdNacionalidad { get; set; }

    public DateTime CaluFechanac { get; set; }

    public string? CaluLugarnacimiento { get; set; }

    public string? CaluDistritonacimiento { get; set; }

    public int CaluIdPaisnac { get; set; }

    public string? CaluCalle { get; set; }

    public string? CaluNro { get; set; }

    public string? CaluPiso { get; set; }

    public string? CaluDto { get; set; }

    public string? CaluCodigopostal { get; set; }

    public int? CaluIdLocalidad { get; set; }

    public int? CaluIdProvincia { get; set; }

    public int? CaluIdPais { get; set; }

    public string? CaluTe { get; set; }

    public string? CaluCelular { get; set; }

    public string? CaluEmail { get; set; }

    public string? CaluEmail2 { get; set; }

    public int? CaluIdEstadoacademico { get; set; }

    public DateTime? CaluFechabaja { get; set; }

    public DateTime? CaluFechagraduacion { get; set; }

    public string? CaluCuil { get; set; }

    public string? CaluMatricula { get; set; }

    public string CaluExtranjero { get; set; } = null!;

    public int CaluIdEstadoadministrativo { get; set; }

    public int? CaluIdPaisemision { get; set; }

    public string? CaluCallePro { get; set; }

    public string? CaluNroPro { get; set; }

    public string? CaluPisoPro { get; set; }

    public string? CaluDtoPro { get; set; }

    public string? CaluCodigopostalPro { get; set; }

    public int? CaluIdLocalidadPro { get; set; }

    public int? CaluIdProvinciaPro { get; set; }

    public int? CaluIdPaisPro { get; set; }

    public string? CaluCohorte { get; set; }

    public string? CaluRterc { get; set; }

    public string? CaluEterc { get; set; }

    public string? CaluClientesap { get; set; }

    public string? CaluEsAlumno { get; set; }
}
