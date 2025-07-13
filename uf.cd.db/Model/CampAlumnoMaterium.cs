using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampAlumnoMaterium
{
    public string CamaIdAlumno { get; set; } = null!;

    public int CamaIdCarrera { get; set; }

    public int CamaLu { get; set; }

    public int CamaIdMateria { get; set; }

    public string? CamaRegular { get; set; }

    public string? CamaFinal { get; set; }
}
