using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampMateriasACursar
{
    public string? CmcuIdAlumno { get; set; }

    public int? CmcuIdCarrera { get; set; }

    public int? CmcuIdMateria { get; set; }

    public int? CmcuLu { get; set; }

    public int? CmcuIdPlan { get; set; }

    public string? CmcuClMateria { get; set; }

    public string? CmcuAsignatura { get; set; }

    public short? CmcuAnio { get; set; }
}
