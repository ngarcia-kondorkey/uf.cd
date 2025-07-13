using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampEstadoAcademico
{
    public int CeacIdEstadoacademico { get; set; }

    public string CeacEstadoacademico { get; set; } = null!;

    public string? CeacSemaforo { get; set; }
}
