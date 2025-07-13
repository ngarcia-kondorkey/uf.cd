using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampInfoAcademicaPrevium
{
    public string CiapIdAlumno { get; set; } = null!;

    public int CiapIdCarrera { get; set; }

    public short CiapNroTitulo { get; set; }

    public string? CiapTitulo { get; set; }

    public int? CiapIdNivel { get; set; }

    public int? CiapIdTipoTitulo { get; set; }

    public DateOnly? CiapFechagraduacion { get; set; }

    public string? CiapInstituto { get; set; }

    public int? CiapIdPais { get; set; }

    public int? CiapIdProvincia { get; set; }

    public int? CiapIdLocalidad { get; set; }

    public string? CiapConvaReva { get; set; }

    public string? CiapUniversidad { get; set; }

    public string? CiapResolucion { get; set; }

    public DateOnly? CiapFechaemision { get; set; }

    public string? CiapOrigenDato { get; set; }
}
