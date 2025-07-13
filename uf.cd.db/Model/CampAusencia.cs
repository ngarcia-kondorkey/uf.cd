using System;
using System.Collections.Generic;

namespace uf.cd.db.Model;

public partial class CampAusencia
{
    public string CausIdAlumno { get; set; } = null!;

    public int CausIdCarrera { get; set; }

    public int CausLu { get; set; }

    public string CausAsignatura { get; set; } = null!;

    public string CausCiclo { get; set; } = null!;

    public DateTime CausFecha { get; set; }

    public string CausTipo { get; set; } = null!;

    public short? CausValido { get; set; }
}
