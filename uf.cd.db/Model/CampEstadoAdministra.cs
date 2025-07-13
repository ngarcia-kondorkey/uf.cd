using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampEstadoAdministra
{
    public int CeadIdEstadoadministrativo { get; set; }

    public string CeadEstadoadministrativo { get; set; } = null!;

    public string? CeadSemaforo { get; set; }
}
