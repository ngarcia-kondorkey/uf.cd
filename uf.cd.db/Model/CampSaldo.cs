using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampSaldo
{
    public string CsalIdAlumnos { get; set; } = null!;

    public string? CsalRterc { get; set; }

    public int? CsalIdCarrera { get; set; }

    public int? CsalLu { get; set; }

    public string? CsalAlumno { get; set; }

    public string? CsalFechaFactura { get; set; }

    public string? CsalNroFactura { get; set; }

    public float? CsalTotalFactura { get; set; }

    public float? CsalSaldo { get; set; }

    public string? CsalEstadoAdminis { get; set; }
}
