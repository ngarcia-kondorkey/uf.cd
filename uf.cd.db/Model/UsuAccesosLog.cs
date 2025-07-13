using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class UsuAccesosLog
{
    public DateTime UsalFechaHora { get; set; }

    public int? UsalOrden { get; set; }

    public string? UsalUsuaNombre { get; set; }

    public string? UsalClave { get; set; }

    public int? UsalCodigoNumber { get; set; }

    public string? UsalCodigoChar { get; set; }

    public string? UsalVistas { get; set; }

    public string? UsalHabilitado { get; set; }

    public DateOnly? UsalAltaF { get; set; }

    public string? UsalUsrAlta { get; set; }

    public DateOnly? UsalMofiF { get; set; }

    public string? UsalUsrModi { get; set; }

    public int? UsalEmpre { get; set; }
}
