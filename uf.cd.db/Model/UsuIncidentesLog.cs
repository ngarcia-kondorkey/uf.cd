using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class UsuIncidentesLog
{
    public int UsilOrden { get; set; }

    public DateTime UsilFechaHora { get; set; }

    public string UsilUsuaNombre { get; set; } = null!;

    public string UsilErrorTest { get; set; } = null!;

    public string UsilProceso { get; set; } = null!;

    public string? UsilError { get; set; }

    public int? UsilEmpre { get; set; }
}
