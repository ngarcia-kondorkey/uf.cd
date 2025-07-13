using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampFinalesIncripcion
{
    public int CfiiIdExamen { get; set; }

    public int CfiiIdPeriodo { get; set; }

    public int CfiiIdLlamado { get; set; }

    public string CfiiIdAlumno { get; set; } = null!;

    public int CfiiIdLu { get; set; }

    public string? CfiiInscripto { get; set; }

    public DateTime? CfiiFechaIncripcion { get; set; }

    public int? CfiiIdEstadoadministrativo { get; set; }
}
