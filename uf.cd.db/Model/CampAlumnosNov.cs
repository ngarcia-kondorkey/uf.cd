using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampAlumnosNov
{
    public string CanvIdAlumno { get; set; } = null!;

    public int CanvIdCarrera { get; set; }

    public int CanvLu { get; set; }

    public int? CanvAnioinicio { get; set; }

    public string CanvApellido { get; set; } = null!;

    public string CanvNombre { get; set; } = null!;

    public int CanvIdTipodoc { get; set; }

    public string CanvNrodoc { get; set; } = null!;

    public string CanvIdSexo { get; set; } = null!;

    public int? CanvIdNacionalidad { get; set; }

    public DateOnly CanvFechanac { get; set; }

    public string? CanvLugarnacimiento { get; set; }

    public string? CanvDistritonacimiento { get; set; }

    public int CanvIdPaisnac { get; set; }

    public string? CanvCalle { get; set; }

    public string? CanvNro { get; set; }

    public string? CanvPiso { get; set; }

    public string? CanvDto { get; set; }

    public string? CanvCodigopostal { get; set; }

    public int? CanvIdLocalidad { get; set; }

    public int? CanvIdProvincia { get; set; }

    public int? CanvIdPais { get; set; }

    public string? CanvTe { get; set; }

    public string? CanvCelular { get; set; }

    public string? CanvEmail { get; set; }

    public string? CanvEmail2 { get; set; }

    public int? CanvIdEstadoacademico { get; set; }

    public DateOnly? CanvFechabaja { get; set; }

    public DateOnly? CanvFechagraduacion { get; set; }

    public string? CanvCuil { get; set; }

    public string? CanvMatricula { get; set; }

    public string CanvExtranjero { get; set; } = null!;

    public int CanvIdEstadoadministrativo { get; set; }

    public int? CanvIdPaisemision { get; set; }

    public string? CanvCallePro { get; set; }

    public string? CanvNroPro { get; set; }

    public string? CanvPisoPro { get; set; }

    public string? CanvDtoPro { get; set; }

    public string? CanvCodigopostalPro { get; set; }

    public int? CanvIdLocalidadPro { get; set; }

    public int? CanvIdProvinciaPro { get; set; }

    public int? CanvIdPaisPro { get; set; }

    public short? Estado { get; set; }
}
