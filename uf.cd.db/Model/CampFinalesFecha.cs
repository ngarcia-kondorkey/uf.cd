using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampFinalesFecha
{
    public int CfifIdExamen { get; set; }

    public int CfifIdPeriodo { get; set; }

    public int CfifIdLlamado { get; set; }

    public string CfifAsignatura { get; set; } = null!;

    public DateTime? CfifFecha { get; set; }

    public string? CfifObservacion { get; set; }

    public int? CfifIdMateria { get; set; }
}
