using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampInfoAcademicaPreviaNov
{
    public string CianIdAlumno { get; set; } = null!;

    public int CianIdCarrera { get; set; }

    public short CianNroTitulo { get; set; }

    public string? CianTitulo { get; set; }

    public string? CianIdNivel { get; set; }

    public string? CianIdTipoTitulo { get; set; }

    public DateOnly? CianFechagraduacion { get; set; }

    public string? CianInstituto { get; set; }

    public int? CianIdPais { get; set; }

    public int? CianIdProvincia { get; set; }

    public int? CianIdLocalidad { get; set; }

    public string? CianConvaReva { get; set; }

    public string? CianUniversidad { get; set; }

    public string? CianResolucion { get; set; }

    public DateOnly? CianFechaemision { get; set; }
}
