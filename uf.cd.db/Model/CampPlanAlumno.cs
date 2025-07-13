using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampPlanAlumno
{
    public string CpalIdAlumno { get; set; } = null!;

    public int CpalIdCarrera { get; set; }

    public int CpalIdMateria { get; set; }

    public int CpalIdPl { get; set; }

    public int CpalLu { get; set; }

    public string CpalPlanalumno { get; set; } = null!;

    public int CpalIdEstadoacademico { get; set; }

    public string CpalAnio { get; set; } = null!;

    public string CpalCodigoasignatura { get; set; } = null!;

    public string CpalAsignatura { get; set; } = null!;

    public DateTime? CpalFecha { get; set; }

    public string? CpalLibro { get; set; }

    public int? CpalFolio { get; set; }

    public string? CpalOrden { get; set; }

    public string? CpalCifras { get; set; }

    public string? CpalLetras { get; set; }

    public int? CpalIdEstadoasignatura { get; set; }
}
